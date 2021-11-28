using System;
using System.ComponentModel.DataAnnotations;

namespace Conduit.Auth.Domain.Users
{
    public record User
    {
        public User()
        {
        }

        public User(
            Guid id,
            string username,
            string email,
            string password,
            string? image = default,
            string? biography = default)
        {
            Id = id;
            Username = username;
            Email = email;
            Password = password;
            Image = image;
            Biography = biography;
        }

        [Key]
        public Guid Id { get; init; }

        [Required]
        public string Username { get; init; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; init; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; init; } = string.Empty;

        [DataType(DataType.ImageUrl)]
        public string? Image { get; init; }

        [DataType(DataType.MultilineText)]
        public string? Biography { get; init; }
    }
}
