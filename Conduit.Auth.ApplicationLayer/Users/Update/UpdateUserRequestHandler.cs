using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users.Tokens;
using Conduit.Auth.Domain.Services.DataAccess;
using Conduit.Auth.Domain.Users;
using Conduit.Auth.Domain.Users.Passwords;
using Conduit.Auth.Domain.Users.Repositories;
using Conduit.Shared.Events.Models.Users.Update;
using Conduit.Shared.Events.Services;
using FluentValidation;
using MediatR;

namespace Conduit.Auth.ApplicationLayer.Users.Update;

public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest,
    Outcome<UserResponse>>
{
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IEventProducer<UpdateUserEventModel> _eventProducer;
    private readonly IPasswordManager _passwordManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateUserRequest> _validator;

    public UpdateUserRequestHandler(
        IUnitOfWork unitOfWork,
        ITokenProvider tokenProvider,
        IPasswordManager passwordManager,
        IValidator<UpdateUserRequest> validator,
        ICurrentUserProvider currentUserProvider,
        IEventProducer<UpdateUserEventModel> eventProducer)
    {
        _unitOfWork = unitOfWork;
        _tokenProvider = tokenProvider;
        _passwordManager = passwordManager;
        _validator = validator;
        _currentUserProvider = currentUserProvider;
        _eventProducer = eventProducer;
    }

    public async Task<Outcome<UserResponse>> Handle(
        UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult =
            await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Outcome.Reject<UserResponse>(validationResult);
        }

        var user =
            await _currentUserProvider.GetCurrentUserAsync(cancellationToken);
        if (user is null)
        {
            return Outcome.New<UserResponse>(OutcomeType.Banned);
        }

        user = await UpdateUserAsync(request, user, cancellationToken);

        await ProduceUpdateUserEventAsync(user);

        return await _tokenProvider.CreateUserResponseAsync(user,
            cancellationToken);
    }

    private async Task<User> UpdateUserAsync(
        UpdateUserRequest request,
        User source,
        CancellationToken cancellationToken)
    {
        var model = request.User;
        var newUser = source with
        {
            Email = model.Email ?? source.Email,
            Biography = model.Bio ?? source.Biography,
            Image = model.Image ?? source.Image
        };
        var isPasswordUpdated =
            string.IsNullOrWhiteSpace(model.Password) == false &&
            model.Password != source.Password;
        if (isPasswordUpdated)
        {
            newUser = newUser.WithHashedPassword(_passwordManager);
        }

        var user =
            await _unitOfWork.UpdateUserAsync(newUser, cancellationToken);

        return user;
    }

    private async Task ProduceUpdateUserEventAsync(
        User user)
    {
        var updateUserEventModel = new UpdateUserEventModel
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Image = user.Image,
            Biography = user.Biography
        };

        await _eventProducer.ProduceEventAsync(updateUserEventModel);
    }
}
