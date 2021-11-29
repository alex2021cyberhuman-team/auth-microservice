using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users;
using Conduit.Auth.Domain.Services.DataAccess;
using Conduit.Auth.Domain.Users.Repositories;
using Conduit.Auth.Domain.Users.Services;
using FluentValidation;

namespace Conduit.Auth.ApplicationLayer.Users.Update
{
    public class
        UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator(
            IImageChecker imageChecker,
            IUnitOfWork unitOfWork,
            ICurrentUserProvider currentUserProvider)
        {
            RuleFor(x => x.User)
                .SetValidator(new UserUpdateModelValidator(imageChecker));
            RuleFor(x => x.User.Email).UniqueEmail(
                unitOfWork.GetRequiredRepository<IUsersFindByEmailRepository>(),
                currentUserProvider);
            RuleFor(x => x.User.Username).UniqueUsername(
                unitOfWork
                    .GetRequiredRepository<IUsersFindByUsernameRepository>(),
                currentUserProvider);
        }
    }
}
