using Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext repositoryContext;

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        public async Task Create(T entity)
        {
            await repositoryContext.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            repositoryContext.Set<T>().Remove(entity);
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return !trackChanges ? repositoryContext.Set<T>().Where(expression).AsNoTracking() : repositoryContext.Set<T>().Where(expression);
        }

        public IQueryable<T> GetAll(bool trackChanges)
        {
            return !trackChanges ? repositoryContext.Set<T>().AsNoTracking() : repositoryContext.Set<T>();
        }

        public void Update(T entity)
        {
            repositoryContext.Set<T>().Update(entity);
        }
    }
}
