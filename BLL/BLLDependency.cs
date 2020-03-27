using BLL.Request;
using BLL.Service;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class BLLDependency
    {
        public static void RegisterBLLServices(IServiceCollection services)
        {
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<StudentCreateRequest, StudentCreateRequest>();
            services.AddTransient<IValidator<DepartmentCreateRequest>, DepartmentCreateRequestValidator>();
            
            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<DepartmentCreateRequest, DepartmentCreateRequest>();
        }
    }
}