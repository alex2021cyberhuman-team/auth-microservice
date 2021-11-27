using System.ComponentModel.DataAnnotations;

namespace Conduit.Auth.ApplicationLayer.Users.Update
{
    public class UpdateUserModel
    {
        public string? Username { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [DataType(DataType.ImageUrl)]
        public string? Image { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Bio { get; set; }
    }
}
