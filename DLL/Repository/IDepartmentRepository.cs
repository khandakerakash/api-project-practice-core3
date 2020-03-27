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
        Task DeleteAsync(long id);
        Task<int> SaveChangesAsync();
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
            var departmentToUpdate =
                await _context.Departments.FirstOrDefaultAsync(x => x.DepartmentId == department.DepartmentId);
            
            if (departmentToUpdate != null)
            {
                departmentToUpdate.Name = department.Name;
                departmentToUpdate.Code = department.Code;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(long id)
        {
            var departmentToDelete = await _context.Departments.FirstOrDefaultAsync(x => x.DepartmentId == id);
            if (departmentToDelete != null)
            {
                _context.Remove(departmentToDelete);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<int> SaveChangesAsync()
        {
            var complete = await _context.SaveChangesAsync();

            await RemoveTrackedEntries();

            return complete;
        }
        
        private async Task RemoveTrackedEntries()
        {
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
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