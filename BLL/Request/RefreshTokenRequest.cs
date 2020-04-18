using FluentValidation;

namespace BLL.Request
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
    }
    
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.Token).NotNull().NotEmpty();
        }
    }
}