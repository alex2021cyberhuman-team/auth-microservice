using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.DataAccess;

namespace Conduit.Auth.Domain.Users.Repositories;

public interface IUsersFindByUsernameRepository : IRepository
{
    Task<User?> FindByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);
}
