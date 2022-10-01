using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.DomainLayer.Services.DataAccess;
using Conduit.Auth.DomainLayer.Users.Repositories;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Conduit.Auth.ApplicationLayer.Users.Register;

public class
    RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator(
        IUnitOfWork unitOfWork,
        IStringLocalizer stringLocalizer,
        IValidator<RegisterUserModel> userModelValidator)
    {
        RuleFor(x => x.User).SetValidator(userModelValidator);
        RuleFor(x => x.User.Email).UniqueEmail(
            unitOfWork.GetRequiredRepository<IUsersFindByEmailRepository>(),
            stringLocalizer);
        RuleFor(x => x.User.Username).UniqueUsername(
            unitOfWork.GetRequiredRepository<IUsersFindByUsernameRepository>(),
            stringLocalizer);
    }
}
