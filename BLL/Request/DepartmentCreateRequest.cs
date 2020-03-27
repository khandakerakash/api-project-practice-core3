using FluentValidation;

namespace BLL.Request
{
    public class DepartmentCreateRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
    
    public class DepartmentCreateRequestValidator : AbstractValidator<DepartmentCreateRequest> {
        public DepartmentCreateRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Code).NotNull().NotEmpty();
        }
    }
}