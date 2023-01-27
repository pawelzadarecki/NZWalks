using FluentValidation;

namespace NZWalks.Api.Validators
{
    public class LoginRequestValidator: AbstractValidator<Models.DTO.LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Passward).NotEmpty();
        }
    }
}
