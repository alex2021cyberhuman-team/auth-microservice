using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users.Tokens;
using MediatR;

namespace Conduit.Auth.ApplicationLayer.Users.GetCurrent
{
    public class GetCurrentUserRequestHandler : IRequestHandler<
        GetCurrentUserRequest, Outcome<UserResponse>>
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly ITokenProvider _tokenProvider;

        public GetCurrentUserRequestHandler(
            ITokenProvider tokenProvider,
            ICurrentUserProvider currentUserProvider)
        {
            _tokenProvider = tokenProvider;
            _currentUserProvider = currentUserProvider;
        }

        #region IRequestHandler<GetCurrentUserRequest,Outcome<UserResponse>> Members

        public async Task<Outcome<UserResponse>> Handle(
            GetCurrentUserRequest request,
            CancellationToken cancellationToken)
        {
            var user =
                await _currentUserProvider.GetCurrentUserAsync(
                    cancellationToken);
            if (user is null)
            {
                return Outcome.New<UserResponse>(OutcomeType.Banned);
            }

            var token =
                await _tokenProvider.CreateTokenAsync(user, cancellationToken);
            var response = new UserResponse(user, token);
            return Outcome.New(OutcomeType.Successful, response);
        }

        #endregion
    }
}
