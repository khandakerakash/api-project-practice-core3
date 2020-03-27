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
        Task DeleteAsync(Student student);
        Task<Student> IsEmailExistsAsync(string email);
        Task<Student> IsRollNoExistsAsync(string rollNo);
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
           _context.Update(student);
           await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Student student)
        {
            _context.Remove(student);
            await _context.SaveChangesAsync();
        }

        public async Task<Student> IsEmailExistsAsync(string email)
        {
            return await _context.Students.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Student> IsRollNoExistsAsync(string rollNo)
        {
            return await _context.Students.FirstOrDefaultAsync(x => x.RollNo == rollNo);
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