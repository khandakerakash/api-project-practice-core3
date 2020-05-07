using FluentValidation;

namespace BLL.Request
{
    public class CourseUpdateRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class CourseUpdateRequestValidator : AbstractValidator<CourseUpdateRequest>
    {
        public CourseUpdateRequestValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty().MinimumLength(2).MaximumLength(25);
            RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(5).MaximumLength(255);
        }
    }
}