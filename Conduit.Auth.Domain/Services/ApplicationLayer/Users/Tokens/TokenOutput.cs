using System.ComponentModel.DataAnnotations;

namespace Conduit.Auth.Domain.Services.ApplicationLayer.Users.Tokens;

public class TokenOutput
{
    public TokenOutput(
        string accessToken)
    {
        AccessToken = accessToken;
    }

    [Required]
    [DataType(DataType.Text)]
    public string AccessToken { get; set; }
}
