using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.DataAccess;

namespace Conduit.Auth.Domain.Users.Repositories
{
    public interface IUsersFindByEmailRepository : IRepository
    {
        Task<User?> FindByEmailAsync(
            string email,
            CancellationToken cancellationToken = default);
    }
}
