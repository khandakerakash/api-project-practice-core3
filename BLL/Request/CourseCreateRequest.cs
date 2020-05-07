using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Service;
using DLL.Model;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Request
{
    public class CourseCreateRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class CourseCreateRequestValidator : AbstractValidator<CourseCreateRequest>
    {
        private readonly IServiceProvider _serviceProvider;

        public CourseCreateRequestValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            
            RuleFor(x => x.Code).NotNull().NotEmpty().MinimumLength(2).MaximumLength(25)
                .MustAsync(IsCourseCodeExistsAsync).WithMessage("The course with a given code already exists in our system");
            RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(5).MaximumLength(255)
                .MustAsync(IsCourseNameExistsAsync).WithMessage("The course with a given name already exists in our system");
        }

        private async Task<bool> IsCourseCodeExistsAsync(string code, CancellationToken token)
        {
            if (string.IsNullOrEmpty(code))
            {
                return true;
            }
            var courseService = _serviceProvider.GetRequiredService<ICourseService>();
            return await courseService.IsCourseCodeExistsAsync(code);
        }

        private async Task<bool> IsCourseNameExistsAsync(string name, CancellationToken token)
        {
            if (string.IsNullOrEmpty(name))
            {
                return true;
            }
            var courseService = _serviceProvider.GetRequiredService<ICourseService>();
            return await courseService.IsCourseNameExistsAsync(name);
        }
    }
}