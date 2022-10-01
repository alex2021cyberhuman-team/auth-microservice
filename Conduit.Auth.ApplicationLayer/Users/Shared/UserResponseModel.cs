using System.ComponentModel.DataAnnotations;
using Conduit.Auth.DomainLayer.Services.ApplicationLayer.Users.Tokens;
using Conduit.Auth.DomainLayer.Users;

namespace Conduit.Auth.ApplicationLayer.Users.Shared;

public class UserResponseModel
{
    public UserResponseModel(
        User user,
        TokenOutput token)
    {
        Username = user.Username;
        Email = user.Email;
        Image = user.Image;
        Bio = user.Biography;
        Token = token.AccessToken;
    }

    [Required]
    [DataType(DataType.Text)]
    public string Token { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [DataType(DataType.ImageUrl)]
    public string? Image { get; init; }

    [DataType(DataType.MultilineText)]
    public string? Bio { get; init; }
}
