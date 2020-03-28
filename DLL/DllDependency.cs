using DLL.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace DLL
{
    public static class DllDependency
    {
        public static void RegisterDllServices(IServiceCollection services)
        {
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
        }
    }
}