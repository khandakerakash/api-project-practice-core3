using DLL.DbContext;
using DLL.Model;
using DLL.UnitOfWorks;

namespace DLL.Repository
{
    public interface ICourseRepository : IRepositoryBase<Course>
    {
        
    }

    public class CourseRepository : RepositoryBase<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}