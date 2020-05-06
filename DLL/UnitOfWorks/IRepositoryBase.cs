using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DLL.DbContext;
using Microsoft.EntityFrameworkCore;

namespace DLL.UnitOfWorks
{
    public interface IRepositoryBase<T> where T : class
    {
        void Delete(T entity);
        void Update(T entity);
        Task CreateAsync(T entity);
        Task<T> FindSingleAsync(Expression<Func<T, bool>> expression);
        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> expression = null);
        IQueryable<T> QueryAll(Expression<Func<T, bool>> expression = null);
    }

    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        protected RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        
        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }
        
        public async Task<T> FindSingleAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(expression);
        }
        
        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> expression)
        {
            return expression != null
                ? await _context.Set<T>().Where(expression).AsNoTracking().ToListAsync()
                : await _context.Set<T>().AsNoTracking().ToListAsync();
        }
        
        public IQueryable<T> QueryAll(Expression<Func<T, bool>> expression)
        {
            return expression != null
                ? _context.Set<T>().AsQueryable().Where(expression).AsNoTracking()
                : _context.Set<T>().AsQueryable().AsNoTracking();
        }
    }
}