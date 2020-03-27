using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DLL.DbContext;
using DLL.Model;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> FindAllAsync();
        Task<Student> FindOneAsync(long id);
        Task CreateAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(long id);
        Task<int> SaveChangesAsync();
        void Dispose();
    }

    public class StudentRepository : IStudentRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Student>> FindAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> FindOneAsync(long id)
        {
            return await _context.Students.FirstOrDefaultAsync(x => x.StudentId.Equals(id));
        }

        public async Task CreateAsync(Student student)
        {
            await _context.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            var studentToUpdate = await _context.Students.FirstOrDefaultAsync(x => x.StudentId == student.StudentId);
            if (studentToUpdate != null)
            {
                studentToUpdate.Name = student.Name;
                studentToUpdate.Email = student.Email;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(long id)
        {
            var studentToDelete = await _context.Students.FirstOrDefaultAsync(x => x.StudentId == id);
            if (studentToDelete != null)
            {
                _context.Students.Remove(studentToDelete);
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