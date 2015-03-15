using System.Linq;

namespace CarrinhoCompras.Models
{
    public interface IRepositorio<T>
        where T : Entidade
    {
        T Add(T entidade);
        T Delete(int id);
        T Get(int id);
        T Update(T entidade);
        IQueryable<T> ItensCompra { get; }
    }
}