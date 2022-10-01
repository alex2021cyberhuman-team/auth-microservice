using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Shared.Outcomes;
using MediatR;

namespace Conduit.Auth.ApplicationLayer.Users.GetCurrent;

public class GetCurrentUserRequest : IRequest<Outcome<UserResponse>>
{
}
