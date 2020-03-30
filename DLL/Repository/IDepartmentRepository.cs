using DLL.DbContext;
using DLL.Model;
using DLL.UnitOfWorks;

namespace DLL.Repository
{
    public interface IDepartmentRepository : IRepositoryBase<Department>
    {
        
    }

    public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}