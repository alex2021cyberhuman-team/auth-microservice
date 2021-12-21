using System;
using System.ComponentModel.DataAnnotations;
using Conduit.Auth.Domain.Users;

namespace Conduit.Auth.Infrastructure.MongoDB.Users.Dtos;

public class UserDto
{
    public UserDto()
    {
    }

    public UserDto(
        User user)
    {
        Id = user.Id;
        Username = user.Username;
        Email = user.Email;
        Password = user.Password;
        Image = user.Image;
        Biography = user.Biography;
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

    public static explicit operator User(
        UserDto dto)
    {
        return new()
        {
            Id = dto.Id,
            Username = dto.Username,
            Email = dto.Email,
            Password = dto.Password,
            Image = dto.Image,
            Biography = dto.Biography
        };
    }
}
