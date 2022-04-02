using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users;
using Conduit.Auth.Domain.Services.DataAccess;
using Conduit.Auth.Domain.Users.Repositories;
using Conduit.Auth.Domain.Users.Services;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Conduit.Auth.ApplicationLayer.Users.Update;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator(
        IImageChecker imageChecker,
        IUnitOfWork unitOfWork,
        ICurrentUserProvider currentUserProvider,
        IStringLocalizer stringLocalizer)
    {
        RuleFor(x => x.User)
            .SetValidator(
                new UserUpdateModelValidator(imageChecker, stringLocalizer));
        RuleFor(x => x.User.Email).UniqueEmail(
            unitOfWork.GetRequiredRepository<IUsersFindByEmailRepository>(),
            stringLocalizer, currentUserProvider);
        RuleFor(x => x.User.Username).UniqueUsername(
            unitOfWork.GetRequiredRepository<IUsersFindByUsernameRepository>(),
            stringLocalizer, currentUserProvider);
    }
}
