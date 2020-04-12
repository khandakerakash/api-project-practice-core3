using FluentValidation;

namespace BLL.Request
{
    public class StudentUpdateRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string RollNo { get; set; }
        public long DepartmentId { get; set; }
    }

    public class StudentUpdateRequestValidator : AbstractValidator<StudentUpdateRequest>
    {
        public StudentUpdateRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(5).MaximumLength(25);
            RuleFor(x => x.Email).NotNull().NotEmpty().MinimumLength(10).MaximumLength(25).EmailAddress();
            RuleFor(x => x.RollNo).NotNull().NotEmpty().MinimumLength(5).MaximumLength(25);
            RuleFor(x => x.DepartmentId).NotNull().NotEmpty();
        }
    }
}