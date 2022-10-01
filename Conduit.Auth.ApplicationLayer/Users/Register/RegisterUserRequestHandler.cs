using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.DomainLayer.Services;
using Conduit.Auth.DomainLayer.Services.ApplicationLayer.Users.Tokens;
using Conduit.Auth.DomainLayer.Services.DataAccess;
using Conduit.Auth.DomainLayer.Users;
using Conduit.Auth.DomainLayer.Users.Passwords;
using Conduit.Auth.DomainLayer.Users.Repositories;
using Conduit.Shared.Events.Models.Users;
using Conduit.Shared.Events.Services;
using Conduit.Shared.Outcomes;
using FluentValidation;
using MediatR;

namespace Conduit.Auth.ApplicationLayer.Users.Register;

public class RegisterUserRequestHandler : IRequestHandler<
    RegisterUserRequest, Outcome<UserResponse>>
{
    private readonly IEventProducer<RegisterUserEventModel> _eventProducer;
    private readonly IIdManager _idManager;
    private readonly IPasswordManager _passwordManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RegisterUserRequest> _validator;

    public RegisterUserRequestHandler(
        IUnitOfWork unitOfWork,
        ITokenProvider tokenProvider,
        IPasswordManager passwordManager,
        IValidator<RegisterUserRequest> validator,
        IIdManager idManager,
        IEventProducer<RegisterUserEventModel> eventProducer)
    {
        _unitOfWork = unitOfWork;
        _tokenProvider = tokenProvider;
        _passwordManager = passwordManager;
        _validator = validator;
        _idManager = idManager;
        _eventProducer = eventProducer;
    }

    public async Task<Outcome<UserResponse>> Handle(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var validationOutcome = await ValidateAsync(request, cancellationToken);
        if (!validationOutcome)
        {
            return validationOutcome;
        }

        var user = await CreateUserAsync(request, cancellationToken);

        await ProduceRegisterUserEventAsync(user);

        return await _tokenProvider.CreateUserResponseAsync(user,
            cancellationToken);
    }

    private async Task<Outcome<UserResponse>> ValidateAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult =
            await _validator.ValidateAsync(request, cancellationToken);
        return !validationResult.IsValid
            ? Outcome.Reject<UserResponse>(validationResult)
            : Outcome.New<UserResponse>();
    }

    private async Task<User> CreateUserAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var newUser = new User(_idManager.GenerateId(), request.User.Username,
            request.User.Email, request.User.Password, request.User.Image,
            request.User.Bio).WithHashedPassword(_passwordManager);

        var user =
            await _unitOfWork.CreateUserAsync(newUser, cancellationToken);

        return user;
    }

    private async Task ProduceRegisterUserEventAsync(
        User user)
    {
        var registerUserEventModel = new RegisterUserEventModel
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Image = user.Image,
            Biography = user.Biography
        };

        await _eventProducer.ProduceEventAsync(registerUserEventModel);
    }
}
