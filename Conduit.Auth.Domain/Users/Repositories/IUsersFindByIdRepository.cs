using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.DataAccess;

namespace Conduit.Auth.Domain.Users.Repositories
{
    public interface IUsersFindByIdRepository : IRepository
    {
        Task<User?> FindByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
