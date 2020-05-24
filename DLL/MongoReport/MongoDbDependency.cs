using DLL.MongoReport.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace DLL.MongoReport
{
    public class MongoDbDependency
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(MongoDbContext));
            services.AddTransient<IDepartmentStudentMongoRepository, DepartmentStudentMongoRepository>();
        }
    }
}