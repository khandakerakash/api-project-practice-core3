using System;
using DLL.Model;
using FluentValidation;

namespace BLL.Request
{
    public class CourseEnrollCreateRequest
    {
        public long CourseId { get; set; }
        public long StudentId { get; set; }
    }

    public class CourseEnrollCreateValidator : AbstractValidator<CourseEnrollCreateRequest>
    {
        private readonly IServiceProvider _serviceProvider;

        public CourseEnrollCreateValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleFor(x => x.CourseId).NotNull().NotEmpty();
            RuleFor(x => x.StudentId).NotNull().NotEmpty();
        }
    }
}