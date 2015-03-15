using System.Web;
using Newtonsoft.Json;

namespace CarrinhoCompras.Models
{
    public class Livro : Entidade
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public double Preco { get; set; }
        public int Quantidade { get; set; }
        public int QuantidadeItensSelecionados { get; set; }
        public string Categoria { get; set; }
    }
}