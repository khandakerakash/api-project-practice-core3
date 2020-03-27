using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Service;
using FluentValidation;

namespace BLL.Request
{
    public class StudentCreateRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string RollNo { get; set; }
        
        public class StudentCreateRequestRequestValidator : AbstractValidator<StudentCreateRequest> {
            private readonly IStudentService _studentService;
            public StudentCreateRequestRequestValidator(IStudentService studentService)
            {
                _studentService = studentService;
                RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(5).MaximumLength(25);
                RuleFor(x => x.Email).NotNull().NotEmpty().MinimumLength(10).MaximumLength(25)
                    .MustAsync(EmailIsExists).WithMessage("The student with given email already exists in our system.");
                RuleFor(x => x.RollNo).NotNull().NotEmpty().MinimumLength(5).MaximumLength(25)
                    .MustAsync(RollNoIsExists).WithMessage("The student with given roll no already exists in our system");
            }

            private async Task<bool> EmailIsExists(string email, CancellationToken token)
            {
                if (string.IsNullOrEmpty(email))
                {
                    return true;
                }

                return await _studentService.IsEmailExistsAsync(email);
            }
            
            private async Task<bool> RollNoIsExists(string rollNo, CancellationToken token)
            {
                if (string.IsNullOrEmpty(rollNo))
                {
                    return true;
                }

                return await _studentService.IsRollNoExistsAsync(rollNo);
            }
        }
    }
}