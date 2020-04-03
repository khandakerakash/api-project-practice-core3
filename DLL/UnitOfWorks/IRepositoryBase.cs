using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DLL.DbContext;
using DLL.Model;
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
    }

    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public RepositoryBase(ApplicationDbContext context)
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
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }
        
        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> expression)
        {
            if (expression != null)
                return await _context.Set<T>().ToListAsync();

            return await _context.Set<T>().ToListAsync();
        }
    }
}