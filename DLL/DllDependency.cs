using DLL.MongoReport;
using DLL.UnitOfWorks;
using Microsoft.Extensions.DependencyInjection;

namespace DLL
{
    public static class DllDependency
    {
        public static void RegisterDllServices(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            MongoDbDependency.RegisterServices(services);
        }
    }
}