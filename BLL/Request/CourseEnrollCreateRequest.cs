using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Service;
using DLL.Model;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

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
            RuleFor(x => x.CourseId).NotNull().NotEmpty()
                .MustAsync(CourseIdExists).WithMessage("The given course Id doesn't exist in our system.");
            RuleFor(x => x.StudentId).NotNull().NotEmpty()
                .MustAsync(StudentIdExists).WithMessage("The given student Id doesn't exist in our system.");
        }

        private async Task<bool> CourseIdExists(long courseId, CancellationToken token)
        {
            if (courseId == 0)
            {
                return true;
            }
            var courseService = _serviceProvider.GetRequiredService<ICourseService>();
            return await courseService.IsCourseIdExistsAsync(courseId);
        }

        private async Task<bool> StudentIdExists(long studentId, CancellationToken token)
        {
            if (studentId == 0)
            {
                return true;
            }
            var studentService = _serviceProvider.GetRequiredService<IStudentService>();
            return await studentService.IsStudentIdExistsAsync(studentId);
        }
    }
}