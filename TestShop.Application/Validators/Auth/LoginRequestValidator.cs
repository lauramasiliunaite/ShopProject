using FluentValidation;
using TestShop.Application.DTOs.Auth;

namespace TestShop.Application.Validators.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(64);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }
}
