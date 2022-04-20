using System;
using System.Linq;
using System.Linq.Expressions;
using CatalogoAPI.Context;
using CatalogoAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CatalogoAPI.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected CatalogoDbContext _context;

        public Repository(CatalogoDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public async Task<T> GetById(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity) 
        {
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity) 
        {
            _context.Set<T>().Remove(entity);
        }
    }
}