using Conduit.Auth.Domain.Services.ApplicationLayer.Users;
using Conduit.Auth.Domain.Users.Repositories;
using Conduit.Auth.Domain.Users.Services;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Conduit.Auth.ApplicationLayer.Users.Shared;

public static class UserValidationBuilderConfiguration
{
    private const string UserUsername = "UserUsername";
    private const string UserPassword = "UserPassword";
    private const string UserImage = "UserImage";
    private const string UsernameShouldMatch = "UsernameShouldMatch";
    private const string PasswordShouldMatch = "PasswordShouldMatch";
    private const string UsernameShouldBeUnique = "UsernameShouldBeUnique";
    private const string EmailShouldBeUnique = "EmailShouldBeUnique";
    private const string UserImageShouldExists = "UserImageShouldExists";

    public static IRuleBuilder<TModel, string?> UniqueEmail<TModel>(
        this IRuleBuilder<TModel, string?> ruleBuilder,
        IUsersFindByEmailRepository findByEmailRepository,
        IStringLocalizer stringLocalizer,
        ICurrentUserProvider? currentUserProvider = null)
    {
        return ruleBuilder
            .SetAsyncValidator(
                new UniqueEmailValidator<TModel>(findByEmailRepository,
                    currentUserProvider))
            .WithMessage(stringLocalizer.GetString(EmailShouldBeUnique));
    }

    public static IRuleBuilder<TModel, string?> UniqueUsername<TModel>(
        this IRuleBuilder<TModel, string?> ruleBuilder,
        IUsersFindByUsernameRepository findByUsernameRepository,
        IStringLocalizer stringLocalizer,
        ICurrentUserProvider? currentUserProvider = null)
    {
        return ruleBuilder
            .SetAsyncValidator(
                new UniqueUsernameValidator<TModel>(findByUsernameRepository,
                    currentUserProvider))
            .WithMessage(stringLocalizer.GetString(UsernameShouldBeUnique));
    }

    public static IRuleBuilder<TModel, string?> ValidImageUrl<TModel>(
        this IRuleBuilder<TModel, string?> ruleBuilder,
        IImageChecker imageChecker,
        IStringLocalizer stringLocalizer)
    {
        return ruleBuilder.MaximumLength(1000)
            .WithName(stringLocalizer.GetString(UserImage))
            .SetAsyncValidator(new ImagePropertyValidator<TModel>(imageChecker))
            .WithMessage(stringLocalizer.GetString(UserImageShouldExists));
    }

    public static IRuleBuilder<TModel, string?> ValidUsername<TModel>(
        this IRuleBuilder<TModel, string?> ruleBuilder,
        IStringLocalizer stringLocalizer)
    {
        return ruleBuilder.MinimumLength(8)
            .WithName(stringLocalizer.GetString(UserUsername))
            .MaximumLength(500)
            .WithName(stringLocalizer.GetString(UserUsername))
            .Matches(UserValidationConfiguration.AcceptedUsernameRegex)
            .WithMessage(stringLocalizer.GetString(UsernameShouldMatch));
    }

    public static IRuleBuilder<TModel, string?> ValidPassword<TModel>(
        this IRuleBuilder<TModel, string?> ruleBuilder,
        IStringLocalizer stringLocalizer)
    {
        return ruleBuilder.MinimumLength(8)
            .WithName(stringLocalizer.GetString(UserPassword))
            .MaximumLength(500)
            .WithName(stringLocalizer.GetString(UserPassword))
            .Matches(UserValidationConfiguration.AcceptedPasswordRegex)
            .WithMessage(stringLocalizer.GetString(PasswordShouldMatch));
    }
}
