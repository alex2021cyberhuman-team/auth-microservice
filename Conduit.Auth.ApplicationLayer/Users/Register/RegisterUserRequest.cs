using System.ComponentModel.DataAnnotations;
using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Shared.Outcomes;
using MediatR;

namespace Conduit.Auth.ApplicationLayer.Users.Register;

public class RegisterUserRequest : IRequest<Outcome<UserResponse>>
{
    public RegisterUserRequest(
        RegisterUserModel user)
    {
        User = user;
    }

    [Required]
    public RegisterUserModel User { get; set; }
}
