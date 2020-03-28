using BLL.Request;
using BLL.Service;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class BllDependency
    {
        public static void RegisterBllServices(IServiceCollection services)
        {
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IValidator<StudentCreateRequest>, StudentCreateRequestValidator>();
            services.AddTransient<IValidator<StudentUpdateRequest>, StudentUpdateRequestValidator>();

            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<IValidator<DepartmentCreateRequest>, DepartmentCreateRequestValidator>();
            services.AddTransient<IValidator<DepartmentUpdateRequest>, DepartmentUpdateRequestValidator>();
        }
    }
}