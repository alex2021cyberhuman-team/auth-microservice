using System.Globalization;
using System.Threading;
using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.DomainLayer.Users.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Conduit.Auth.ApplicationLayer.Users.Register;

public class RegisterUserModelValidator : AbstractValidator<RegisterUserModel>
{
    private const string UserUsername = "UserUsername";
    private const string UserPassword = "UserPassword";
    private const string UserEmail = "UserEmail";
    private const string UserBio = "UserBio";

    public RegisterUserModelValidator(
        IImageChecker imageChecker,
        IStringLocalizer stringLocalizer)
    {
        RuleFor(x => x.Username).ValidUsername(stringLocalizer).NotEmpty()
            .WithName(stringLocalizer.GetString(UserUsername));
        RuleFor(x => x.Email).NotEmpty()
            .WithName(stringLocalizer.GetString(UserEmail)).EmailAddress()
            .WithName(stringLocalizer.GetString(UserEmail));
        RuleFor(x => x.Bio).MaximumLength(500)
            .WithName(stringLocalizer.GetString(UserBio));
        RuleFor(x => x.Image).ValidImageUrl(imageChecker, stringLocalizer);
        RuleFor(x => x.Password).ValidPassword(stringLocalizer).NotEmpty()
            .WithName(stringLocalizer.GetString(UserPassword));
    }
}
