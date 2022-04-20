using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using System;

namespace CatalogoAPI.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        Task<T> GetById (Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}