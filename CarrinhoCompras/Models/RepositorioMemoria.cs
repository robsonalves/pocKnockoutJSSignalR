using System;
using System.Collections.Concurrent;
using System.Linq;

namespace CarrinhoCompras.Models
{
    public class RepositorioMemoria<T> : IRepositorio<T> where T : Entidade
    {
        public static ConcurrentDictionary<int, T> RepositorioMem = new ConcurrentDictionary<int, T>();

        public IQueryable<T> ItensCompra
        {
            get { return RepositorioMem.Values.AsQueryable(); }
        }

        public T Get(int id)
        {
            if (!RepositorioMem.ContainsKey(id))
            {
                return null;
            }

            T entidade;
            var resultado = RepositorioMem.TryGetValue(id, out entidade);
            return !resultado ? null : entidade;
        }

        public T Add(T entidade)
        {
            if (entidade == null)
            {
                throw new ArgumentNullException("entidade");
            }

            var id = RepositorioMem.Count > 0 ? RepositorioMem.Last().Key : 0;
            id++;
            entidade.Id = id;

            var resultadoado = RepositorioMem.TryAdd(id, entidade);
            return resultadoado == false ? null : entidade;
        }

        public T Delete(int id)
        {
            if (!RepositorioMem.ContainsKey(id))
            {
                return null;
            }

            T removed;
            var resultado = RepositorioMem.TryRemove(id, out removed);
            return !resultado ? null : removed;
        }

        public T Update(T entidade)
        {
            if (entidade == null)
            {
                throw new ArgumentNullException("entidade");
            }

            if (!RepositorioMem.ContainsKey(entidade.Id))
            {
                return null;
            }

            RepositorioMem[entidade.Id] = entidade;
            return entidade;
        }
    }
}