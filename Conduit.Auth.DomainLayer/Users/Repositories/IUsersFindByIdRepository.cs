using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.DomainLayer.Services.DataAccess;

namespace Conduit.Auth.DomainLayer.Users.Repositories;

public interface IUsersFindByIdRepository : IRepository
{
    Task<User?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
