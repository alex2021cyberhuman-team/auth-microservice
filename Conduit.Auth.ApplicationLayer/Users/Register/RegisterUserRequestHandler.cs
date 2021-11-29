using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Services;
using Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users.Tokens;
using Conduit.Auth.Domain.Services.DataAccess;
using Conduit.Auth.Domain.Users;
using Conduit.Auth.Domain.Users.Passwords;
using Conduit.Auth.Domain.Users.Repositories;
using FluentValidation;
using MediatR;

namespace Conduit.Auth.ApplicationLayer.Users.Register
{
    public class RegisterUserRequestHandler : IRequestHandler<
        RegisterUserRequest, Outcome<UserResponse>>
    {
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
            IIdManager idManager)
        {
            _unitOfWork = unitOfWork;
            _tokenProvider = tokenProvider;
            _passwordManager = passwordManager;
            _validator = validator;
            _idManager = idManager;
        }

        #region IRequestHandler<RegisterUserRequest,Outcome<UserResponse>> Members

        public async Task<Outcome<UserResponse>> Handle(
            RegisterUserRequest request,
            CancellationToken cancellationToken)
        {
            var validationOutcome =
                await ValidateAsync(request, cancellationToken);
            if (!validationOutcome)
            {
                return validationOutcome;
            }

            var user = await CreateUserAsync(request, cancellationToken);

            return await CreateUserResponseAsync(user, cancellationToken);
        }

        #endregion

        private async Task<Outcome<UserResponse>> CreateUserResponseAsync(
            User user,
            CancellationToken cancellationToken)
        {
            var token =
                await _tokenProvider.CreateTokenAsync(user, cancellationToken);
            var response = new UserResponse(user, token);
            return Outcome.New(OutcomeType.Successful, response);
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
            var newUser = new User(_idManager.GenerateId(),
                    request.User.Username, request.User.Email,
                    request.User.Password, request.User.Image, request.User.Bio)
                .WithHashedPassword(_passwordManager);

            var user = await _unitOfWork.CreateUserAsync(
                newUser, cancellationToken);

            return user;
        }
    }
}
