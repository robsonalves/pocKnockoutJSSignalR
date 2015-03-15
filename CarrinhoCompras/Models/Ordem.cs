using System.Collections.Generic;

namespace CarrinhoCompras.Models
{
    public class Ordem : Entidade
    {
        public string NomeCliente { get; set; }
        public double ValorTotal { get; set; }
        public bool Aprovado { get; set; }

        public virtual ICollection<Livro> Livros { get; set; }
    }
}