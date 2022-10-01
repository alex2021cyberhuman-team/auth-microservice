using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.DomainLayer.Services.ApplicationLayer.Users;
using Conduit.Auth.DomainLayer.Services.DataAccess;
using Conduit.Auth.DomainLayer.Users;
using Conduit.Auth.DomainLayer.Users.Repositories;
using Microsoft.AspNetCore.Http;

namespace Conduit.Auth.InfrastructureLayer.Users.Services;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private User? _requestUser;

    public CurrentUserProvider(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
    }

    public Task<Guid?> GetCurrentUserIdAsync(
        CancellationToken cancellationToken = default)
    {
        var claimsPrincipal = _httpContextAccessor.HttpContext.User;
        var identity = claimsPrincipal.Identity;
        var idClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
        if (identity is not
            {
                IsAuthenticated: true
            } ||
            !Guid.TryParse(idClaim?.Value, out var id))
        {
            return Task.FromResult<Guid?>(null);
        }

        return Task.FromResult<Guid?>(id);
    }

    public async Task<User?> GetCurrentUserAsync(
        CancellationToken cancellationToken = default)
    {
        var id = await GetCurrentUserIdAsync(cancellationToken);
        if (!id.HasValue)
        {
            return null;
        }

        if (_requestUser is null || _requestUser.Id != id.Value)
        {
            _requestUser = await _unitOfWork
                .GetRequiredRepository<IUsersFindByIdRepository>()
                .FindByIdAsync(id.Value, cancellationToken);
        }

        return _requestUser;
    }
}
