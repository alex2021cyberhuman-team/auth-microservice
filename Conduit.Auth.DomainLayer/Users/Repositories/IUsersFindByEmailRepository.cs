using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.DomainLayer.Services.DataAccess;

namespace Conduit.Auth.DomainLayer.Users.Repositories;

public interface IUsersFindByEmailRepository : IRepository
{
    Task<User?> FindByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);
}
