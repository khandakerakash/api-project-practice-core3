using BLL.Request;
using BLL.Service;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class BLLDependency
    {
        public static void RegisterBLLServices(IServiceCollection services)
        {
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IValidator<StudentCreateRequest>, StudentCreateRequestValidator>();
            services.AddTransient<IValidator<StudentUpdateRequest>, StudentUpdateRequestValidator>();

            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<DepartmentCreateRequest, DepartmentCreateRequest>();
            services.AddTransient<IValidator<DepartmentCreateRequest>, DepartmentCreateRequestValidator>();
        }
    }
}