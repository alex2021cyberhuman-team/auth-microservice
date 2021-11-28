using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Users.Services;
using FluentValidation;

namespace Conduit.Auth.ApplicationLayer.Users.Update
{
    public class UserUpdateModelValidator : AbstractValidator<UpdateUserModel>
    {
        public UserUpdateModelValidator(IImageChecker imageChecker)
        {
            RuleFor(x => x.Username).ValidUsername();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Bio).MaximumLength(500);
            RuleFor(x => x.Image).ValidImageUrl(imageChecker);
            RuleFor(x => x.Password).ValidPassword();
        }
    }
}
