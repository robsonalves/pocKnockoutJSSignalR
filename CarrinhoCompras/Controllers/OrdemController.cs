using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CarrinhoCompras.Models;

namespace CarrinhoCompras.Controllers
{
    public class OrdemController : BaseController
    {
        private readonly IRepositorio<Livro> _livroRepositorio;
        private readonly IRepositorio<Ordem> _ordemRepositorio;

        public OrdemController() : this(new RepositorioMemoria<Livro>(), new RepositorioMemoria<Ordem>())
        {}

        public OrdemController(IRepositorio<Livro> livroRepositorio, IRepositorio<Ordem> orderRepositorio)
        {
            _livroRepositorio = livroRepositorio;
            _ordemRepositorio = orderRepositorio;
        }

        public IEnumerable<Ordem> GetAll()
        {
            return _ordemRepositorio.ItensCompra;
        }

        public void Put(int id, Aprovado _aprovadaCompra)
        {
            if (!ModelState.IsValid) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));
            var ordem = _ordemRepositorio.Get(id);
            if (ordem.Aprovado == _aprovadaCompra.AprovadaCompra) return;

            ordem.Aprovado = _aprovadaCompra.AprovadaCompra;
            _ordemRepositorio.Update(ordem);

            CarrinhoComprasHub.Value.Clients.Client(ordem.NomeCliente).orderAprovado(ordem);

            if (!ordem.Aprovado)
            {
                if (!ValidaQuantidadeItens(ordem)) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "A quantidade de itens é inválida!"));

                foreach (var livro in ordem.Livros)
                {   
                    var livroUnitario = _livroRepositorio.Get(livro.Id);
                    livroUnitario.Quantidade += livro.QuantidadeItensSelecionados;
                    _livroRepositorio.Update(livroUnitario);
                    CarrinhoComprasHub.Value.Clients.All.updateProdutoCount(livroUnitario);
                }
            }
        }

        public void Post(Ordem ordem)
        {
            if (!ModelState.IsValid) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));
            if (!ValidaQuantidadeItens(ordem)) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Sua compra possui uma quantidade de itens inválidos!."));

            foreach (var livro in ordem.Livros)
            {
                var livroUnitario = _livroRepositorio.Get(livro.Id);
                livroUnitario.Quantidade -= livro.QuantidadeItensSelecionados;
                _livroRepositorio.Update(livroUnitario);
                CarrinhoComprasHub.Value.Clients.All.updateProdutoCount(livroUnitario);
            }

            var adicionado = _ordemRepositorio.Add(ordem);
            AdminHub.Value.Clients.All.ordemCompraEntregue(adicionado);
        }

        private bool ValidaQuantidadeItens(Ordem ordem)
        {
            foreach (var livro in ordem.Livros)
            {
                var livroUnitario = _livroRepositorio.Get(livro.Id);
                if (livroUnitario.Quantidade < livro.QuantidadeItensSelecionados) return false;
            }

            return true;
        }
    }
}