using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Service;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Request
{
    public class StudentCreateRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string RollNo { get; set; }
    }
    
    public class StudentCreateRequestValidator : AbstractValidator<StudentCreateRequest> {
        private readonly IServiceProvider _serviceProvider;
        public StudentCreateRequestValidator(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;

            RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(5).MaximumLength(25);
            RuleFor(x => x.Email).NotNull().NotEmpty().MinimumLength(10).MaximumLength(25).EmailAddress()
                .MustAsync(EmailIsExists).WithMessage("The student with a given email already exists in our system.");
            RuleFor(x => x.RollNo).NotNull().NotEmpty().MinimumLength(5).MaximumLength(25)
                .MustAsync(RollNoIsExists).WithMessage("The student with a given roll no already exists in our system");
        }
        
        private async Task<bool> EmailIsExists(string email, CancellationToken token)
        {
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }
            var studentService = _serviceProvider.GetRequiredService<IStudentService>();
            return await studentService.IsEmailExistsAsync(email);
        }
            
        private async Task<bool> RollNoIsExists(string rollNo, CancellationToken token)
        {
            if (string.IsNullOrEmpty(rollNo))
            {
                return true;
            }
            var studentService = _serviceProvider.GetRequiredService<IStudentService>();
            return await studentService.IsRollNoExistsAsync(rollNo);
        }
    }
}