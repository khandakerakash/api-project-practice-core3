using System;
using System.Threading.Tasks;
using DLL.DbContext;
using DLL.Repository;

namespace DLL.UnitOfWorks
{
    public interface IUnitOfWork
    {
        // We have to added the all `Repository` here
        IStudentRepository StudentRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        // All `Repository` End
        Task<bool> AppSaveChangesAsync();
        void Dispose();
    }

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool disposed = false;

        private IStudentRepository _studentRepository;
        private IDepartmentRepository _departmentRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IStudentRepository StudentRepository => 
            _studentRepository ??= new StudentRepository(_context);

        public IDepartmentRepository DepartmentRepository =>
            _departmentRepository ??= new DepartmentRepository(_context);

        public async Task<bool> AppSaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

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