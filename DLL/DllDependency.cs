using DLL.Repository;
using DLL.UnitOfWorks;
using Microsoft.Extensions.DependencyInjection;

namespace DLL
{
    public static class DllDependency
    {
        public static void RegisterDllServices(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            // services.AddTransient<IStudentRepository, StudentRepository>();
            // services.AddTransient<IDepartmentRepository, DepartmentRepository>();
        }
    }
}