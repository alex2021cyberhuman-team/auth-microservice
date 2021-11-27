using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Users.Services;
using FluentValidation;

namespace Conduit.Auth.ApplicationLayer.Users.Register
{
    public class RegisterUserModelValidator
        : AbstractValidator<RegisterUserModel>
    {
        public RegisterUserModelValidator(IImageChecker imageChecker)
        {
            RuleFor(x => x.Username).ValidUsername().NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Bio).MaximumLength(500);
            RuleFor(x => x.Image).ValidImageUrl(imageChecker);
            RuleFor(x => x.Password).ValidPassword().NotEmpty();
        }
    }
}
