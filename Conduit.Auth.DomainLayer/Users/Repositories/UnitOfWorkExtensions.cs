using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.DomainLayer.Services.DataAccess;
using Conduit.Auth.DomainLayer.Users.Passwords;

namespace Conduit.Auth.DomainLayer.Users.Repositories;

public static class UnitOfWorkExtensions
{
    public static async Task<User> UpdateUserAsync(
        this IUnitOfWork unitOfWork,
        User newUser,
        CancellationToken cancellationToken = default)
    {
        var repository =
            unitOfWork.GetRequiredRepository<IUsersWriteRepository>();
        var user = await repository.UpdateAsync(newUser, cancellationToken);
        return user;
    }

    public static async Task<User> CreateUserAsync(
        this IUnitOfWork unitOfWork,
        User newUser,
        CancellationToken cancellationToken = default)
    {
        var repository =
            unitOfWork.GetRequiredRepository<IUsersWriteRepository>();
        var user = await repository.CreateAsync(newUser, cancellationToken);
        return user;
    }

    public static User WithHashedPassword(
        this User user,
        IPasswordManager passwordManager)
    {
        return user with
        {
            Password = passwordManager.HashPassword(user.Password, user)
        };
    }

    public static async Task<User?> FindUserByPasswordEmailAsync(
        this IUnitOfWork unitOfWork,
        string plainPassword,
        string email,
        IPasswordManager passwordManager,
        CancellationToken cancellationToken = default)
    {
        var repository = unitOfWork
            .GetRequiredRepository<IUsersFindByEmailRepository>();
        var user = await repository.FindByEmailAsync(email, cancellationToken);
        return user is null ||
               passwordManager.VerifyPassword(plainPassword, user)
            ? user
            : null;
    }

    public static async Task<User?> FindUserByEmailAsync(
        this IUnitOfWork unitOfWork,
        string email,
        CancellationToken cancellationToken = default)
    {
        var repository = unitOfWork
            .GetRequiredRepository<IUsersFindByEmailRepository>();
        var user = await repository.FindByEmailAsync(email, cancellationToken);
        return user;
    }

    public static async Task<User?> FindUserByUsernameAsync(
        this IUnitOfWork unitOfWork,
        string username,
        CancellationToken cancellationToken = default)
    {
        var repository = unitOfWork
            .GetRequiredRepository<IUsersFindByUsernameRepository>();
        var user = await repository.FindByUsernameAsync(
            username, cancellationToken);
        return user;
    }

    public static async Task<User?> FindUserByPrincipalAsync(this IUnitOfWork unitOfWork,
        ClaimsPrincipal principal,
        string claimType = ClaimTypes.NameIdentifier,
        CancellationToken cancellationToken = default)
    {
        var id = Guid.Parse(principal.FindFirst(claimType)!.Value);
        var repository = unitOfWork
            .GetRequiredRepository<IUsersFindByIdRepository>();
        var user = await repository.FindByIdAsync(
            id, cancellationToken);
        return user;
    }
}
