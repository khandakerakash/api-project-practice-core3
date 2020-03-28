using FluentValidation;

namespace BLL.Request
{
    public class DepartmentUpdateRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
    
    public class DepartmentUpdateRequestValidator : AbstractValidator<DepartmentUpdateRequest> {
        public DepartmentUpdateRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(2).MaximumLength(50);
            RuleFor(x => x.Code).NotNull().NotEmpty().MinimumLength(2).MaximumLength(12);
        }
    }
}