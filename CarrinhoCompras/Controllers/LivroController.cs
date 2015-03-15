using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CarrinhoCompras.Models;

namespace CarrinhoCompras.Controllers
{
    public class LivroController : ApiController
    {
        private readonly IRepositorio<Livro> _repositorio;

        public LivroController() : this(new RepositorioMemoria<Livro>())
        {
        }

        public LivroController(IRepositorio<Livro> repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<Livro> Get()
        {
            return _repositorio.ItensCompra;
        }
    }
}