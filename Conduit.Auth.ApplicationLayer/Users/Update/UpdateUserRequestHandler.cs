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
using FluentValidation;
using MediatR;

namespace Conduit.Auth.ApplicationLayer.Users.Update
{
    public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest,
        Outcome<UserResponse>>
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IPasswordManager _passwordManager;
        private readonly ITokenProvider _tokenProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateUserRequest> _validator;

        public UpdateUserRequestHandler(
            IUnitOfWork unitOfWork,
            ITokenProvider tokenProvider,
            IPasswordManager passwordManager,
            IValidator<UpdateUserRequest> validator,
            ICurrentUserProvider currentUserProvider)
        {
            _unitOfWork = unitOfWork;
            _tokenProvider = tokenProvider;
            _passwordManager = passwordManager;
            _validator = validator;
            _currentUserProvider = currentUserProvider;
        }

        #region IRequestHandler<UpdateUserRequest,Outcome<UserResponse>> Members

        public async Task<Outcome<UserResponse>> Handle(
            UpdateUserRequest request,
            CancellationToken cancellationToken)
        {
            var validationResult =
                await _validator.ValidateAsync(request, cancellationToken);
            var user =
                await _currentUserProvider.GetCurrentUserAsync(
                    cancellationToken);
            if (user is null)
            {
                return Outcome.New<UserResponse>(OutcomeType.Banned);
            }

            if (!validationResult.IsValid)
            {
                return Outcome.Reject<UserResponse>(validationResult);
            }

            user = await UpdateUserAsync(request, user, cancellationToken);
            var token =
                await _tokenProvider.CreateTokenAsync(user, cancellationToken);
            var response = new UserResponse(user, token);
            return Outcome.New(OutcomeType.Successful, response);
        }

        #endregion

        private async Task<User> UpdateUserAsync(
            UpdateUserRequest request,
            User source,
            CancellationToken cancellationToken)
        {
            var model = request.User;
            var newUser = source with
            {
                Email = model.Email ?? source.Email,
                Password = model.Password ?? source.Password,
                Biography = model.Bio ?? source.Biography,
                Image = model.Image ?? source.Image
            };
            var user = await _unitOfWork.HashPasswordAndUpdateUserAsync(newUser,
                _passwordManager, cancellationToken);
            return user;
        }
    }
}
