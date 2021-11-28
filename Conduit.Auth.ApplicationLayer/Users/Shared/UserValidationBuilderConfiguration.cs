using Conduit.Auth.Domain.Services.ApplicationLayer.Users;
using Conduit.Auth.Domain.Users.Repositories;
using Conduit.Auth.Domain.Users.Services;
using FluentValidation;

namespace Conduit.Auth.ApplicationLayer.Users.Shared
{
    public static class UserValidationBuilderConfiguration
    {
        public static IRuleBuilder<TModel, string?> UniqueEmail<TModel>(
            this IRuleBuilder<TModel, string?> ruleBuilder,
            IUsersFindByEmailRepository findByEmailRepository,
            ICurrentUserProvider? currentUserProvider = null)
        {
            return ruleBuilder.SetAsyncValidator(
                new UniqueEmailValidator<TModel>(
                    findByEmailRepository,
                    currentUserProvider));
        }

        public static IRuleBuilder<TModel, string?> UniqueUsername<TModel>(
            this IRuleBuilder<TModel, string?> ruleBuilder,
            IUsersFindByUsernameRepository findByUsernameRepository,
            ICurrentUserProvider? currentUserProvider = null)
        {
            return ruleBuilder.SetAsyncValidator(
                new UniqueUsernameValidator<TModel>(
                    findByUsernameRepository,
                    currentUserProvider));
        }

        public static IRuleBuilder<TModel, string?> ValidImageUrl<TModel>(
            this IRuleBuilder<TModel, string?> ruleBuilder,
            IImageChecker imageChecker)
        {
            return ruleBuilder.MaximumLength(1000)
                .SetAsyncValidator(
                    new ImagePropertyValidator<TModel>(imageChecker));
        }

        public static IRuleBuilder<TModel, string?> ValidUsername<TModel>(
            this IRuleBuilder<TModel, string?> ruleBuilder)
        {
            return ruleBuilder.MinimumLength(8)
                .MaximumLength(500)
                .Matches(UserValidationConfiguration.AcceptedUsernameRegex);
        }

        public static IRuleBuilder<TModel, string?> ValidPassword<TModel>(
            this IRuleBuilder<TModel, string?> ruleBuilder)
        {
            return ruleBuilder.MinimumLength(8)
                .MaximumLength(500)
                .Matches(UserValidationConfiguration.AcceptedPasswordRegex);
        }
    }
}
