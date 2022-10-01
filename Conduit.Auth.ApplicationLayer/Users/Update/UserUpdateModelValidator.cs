using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.DomainLayer.Users.Services;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Conduit.Auth.ApplicationLayer.Users.Update;

public class UserUpdateModelValidator : AbstractValidator<UpdateUserModel>
{
    private const string UserEmail = "UserEmail";
    private const string UserBio = "UserBio";

    public UserUpdateModelValidator(
        IImageChecker imageChecker,
        IStringLocalizer stringLocalizer)
    {
        RuleFor(x => x.Username).ValidUsername(stringLocalizer);
        RuleFor(x => x.Email).EmailAddress()
            .WithName(stringLocalizer.GetString(UserEmail));
        RuleFor(x => x.Bio).MaximumLength(500)
            .WithName(stringLocalizer.GetString(UserBio));
        RuleFor(x => x.Image).ValidImageUrl(imageChecker, stringLocalizer);
        RuleFor(x => x.Password).ValidPassword(stringLocalizer);
    }
}
