using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DLL.DbContext;
using DLL.Model;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> FindAllAsync();
        Task<Department> FindOneAsync(long id);
        Task CreateAsync(Department department);
        Task UpdateAsync(Department department);
        Task DeleteAsync(Department department);
        Task<Department> IsNameExistsAsync(string name);
        Task<Department> IsCodeExistsAsync(string code);
        void Dispose();
    }

    public class DepartmentRepository : IDepartmentRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Department>> FindAllAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department> FindOneAsync(long id)
        {
            return await _context.Departments.FirstOrDefaultAsync(x => x.DepartmentId.Equals(id));
        }

        public async Task CreateAsync(Department department)
        {
            await _context.AddAsync(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Department department)
        {
            _context.Remove(department);
            await _context.SaveChangesAsync();
        }

        public async Task<Department> IsNameExistsAsync(string name)
        {
            return await _context.Departments.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Department> IsCodeExistsAsync(string code)
        {
            return await _context.Departments.FirstOrDefaultAsync(x => x.Code == code);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}