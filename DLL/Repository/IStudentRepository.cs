using DLL.DbContext;
using DLL.Model;
using DLL.UnitOfWorks;

namespace DLL.Repository
{
    public interface IStudentRepository : IRepositoryBase<Student>
    {
    }

    public class StudentRepository : RepositoryBase<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}