using DLL.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace DLL
{
    public static class DLLDependency
    {
        public static void RegisterDLLServices(IServiceCollection services)
        {
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
        }
    }
}