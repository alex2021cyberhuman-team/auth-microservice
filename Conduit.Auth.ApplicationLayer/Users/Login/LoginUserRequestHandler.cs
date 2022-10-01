using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.DomainLayer.Services.ApplicationLayer.Users.Tokens;
using Conduit.Auth.DomainLayer.Services.DataAccess;
using Conduit.Auth.DomainLayer.Users.Passwords;
using Conduit.Auth.DomainLayer.Users.Repositories;
using Conduit.Shared.Outcomes;
using MediatR;

namespace Conduit.Auth.ApplicationLayer.Users.Login;

public class LoginUserRequestHandler : IRequestHandler<LoginUserRequest,
    Outcome<UserResponse>>
{
    private readonly IPasswordManager _passwordManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;

    public LoginUserRequestHandler(
        IUnitOfWork unitOfWork,
        ITokenProvider tokenProvider,
        IPasswordManager passwordManager)
    {
        _unitOfWork = unitOfWork;
        _tokenProvider = tokenProvider;
        _passwordManager = passwordManager;
    }

    public async Task<Outcome<UserResponse>> Handle(
        LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.FindUserByPasswordEmailAsync(
            request.User.Password, request.User.Email, _passwordManager,
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
}
