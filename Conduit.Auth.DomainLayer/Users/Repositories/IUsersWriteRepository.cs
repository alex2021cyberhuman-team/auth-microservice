using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.DomainLayer.Services.DataAccess;

namespace Conduit.Auth.DomainLayer.Users.Repositories;

public interface IUsersWriteRepository : IRepository
{
    Task<User> CreateAsync(
        User user,
        CancellationToken cancellationToken = default);

    Task<User> UpdateAsync(
        User user,
        CancellationToken cancellationToken = default);
}
