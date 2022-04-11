using System;
using Conduit.Auth.Domain.Users;
using MongoDB.Bson.Serialization.Attributes;

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

    [BsonId]
    public Guid Id { get; init; }


    public string Username { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string? Image { get; init; }

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
