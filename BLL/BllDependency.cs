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
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost";
                options.InstanceName = "PracticeApiDB";
            });
            
            services.AddTransient<ITestService, TestService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<ICourseEnrollService, CourseEnrollService>();
            
            AllValidationDependency(services);
        }

        private static void AllValidationDependency(IServiceCollection services)
        {
            services.AddTransient<IValidator<RefreshTokenRequest>, RefreshTokenRequestValidator>();
            services.AddTransient<IValidator<StudentCreateRequest>, StudentCreateRequestValidator>();
            services.AddTransient<IValidator<StudentUpdateRequest>, StudentUpdateRequestValidator>();
            services.AddTransient<IValidator<DepartmentCreateRequest>, DepartmentCreateRequestValidator>();
            services.AddTransient<IValidator<DepartmentUpdateRequest>, DepartmentUpdateRequestValidator>();
            services.AddTransient<IValidator<CourseCreateRequest>, CourseCreateRequestValidator>();
            services.AddTransient<IValidator<CourseUpdateRequest>, CourseUpdateRequestValidator>();
            services.AddTransient<IValidator<CourseEnrollCreateRequest>, CourseEnrollCreateValidator>();
        }
    }
}