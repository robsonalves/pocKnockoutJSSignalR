using System.Net;
using System.Net.Http;
using System.Web.Http;
using CarrinhoCompras.Models;

namespace CarrinhoCompras.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IRepositorio<Livro> _repositorio;

        public AdminController() : this(new RepositorioMemoria<Livro>()){}

        public AdminController(IRepositorio<Livro> repositorio)
        {
            _repositorio = repositorio;
        }

        public void Put(int id, Livro livro)
        {
            if (!ModelState.IsValid) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));

            livro.Id = id;
            var newLivro = _repositorio.Update(livro);
            CarrinhoComprasHub.Value.Clients.All.updateprodutoCount(newLivro);
        }        
    }
}