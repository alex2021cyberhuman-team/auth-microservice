using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.DomainLayer.Services.DataAccess;

namespace Conduit.Auth.DomainLayer.Users.Repositories;

public interface IUsersFindByUsernameRepository : IRepository
{
    Task<User?> FindByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);
}
