using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes;
using MediatR;

namespace Conduit.Auth.ApplicationLayer.Users.Update
{
    public class UpdateUserRequest : IRequest<Outcome<UserResponse>>
    {
        public UpdateUserRequest(
            UpdateUserModel user)
        {
            User = user;
        }

        public UpdateUserModel User { get; set; }
    }
}
