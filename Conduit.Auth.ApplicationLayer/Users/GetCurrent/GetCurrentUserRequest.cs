using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes;
using MediatR;

namespace Conduit.Auth.ApplicationLayer.Users.GetCurrent;

public class GetCurrentUserRequest : IRequest<Outcome<UserResponse>>
{
}
