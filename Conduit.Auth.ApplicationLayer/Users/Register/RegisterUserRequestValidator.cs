using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Services.DataAccess;
using Conduit.Auth.Domain.Users.Repositories;
using Conduit.Auth.Domain.Users.Services;
using FluentValidation;

namespace Conduit.Auth.ApplicationLayer.Users.Register
{
    public class RegisterUserRequestValidator
        : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator(
            IImageChecker imageChecker,
            IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.User)
                .SetValidator(new RegisterUserModelValidator(imageChecker));
            RuleFor(x => x.User.Email)
                .UniqueEmail(
                    unitOfWork
                        .GetRequiredRepository<IUsersFindByEmailRepository>());
            RuleFor(x => x.User.Username)
                .UniqueUsername(
                    unitOfWork
                        .GetRequiredRepository<
                            IUsersFindByUsernameRepository>());
        }
    }
}
