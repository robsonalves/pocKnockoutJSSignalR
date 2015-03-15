using System;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace CarrinhoCompras.Models
{
    public class EfRepositorioMemsitorio<T> : IRepositorio<T> where T : Entidade
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public EfRepositorioMemsitorio(DbContext dbContext)
        {
            _dbContext = dbContext;

            if (dbContext == null)
            {

                throw new ArgumentNullException("dbContext");
            }

            _dbSet = dbContext.Set<T>();
        }

        public T Add(T entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public T Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) return null;

            _dbSet.Remove(entity);
            _dbContext.SaveChanges();

            return entity;

        }

        public T Get(int id)
        {
            return _dbSet.Find(id);
        }

        public T Update(T entity)
        {
            if (entity == null) return null;

            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return entity;
        }

        public IQueryable<T> ItensCompra
        {
            get { return _dbSet; }
        }

        public class LivroStoreContext : DbContext
        {
            public DbSet<Livro> Livros { get; set; }
            public DbSet<Ordem> Ordems { get; set; }
        }
    }
}