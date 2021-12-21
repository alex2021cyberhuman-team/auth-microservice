using System.ComponentModel.DataAnnotations;

namespace Conduit.Auth.ApplicationLayer.Users.Register;

public class RegisterUserModel
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [DataType(DataType.ImageUrl)]
    public string? Image { get; set; }

    [DataType(DataType.MultilineText)]
    public string? Bio { get; set; }
}
