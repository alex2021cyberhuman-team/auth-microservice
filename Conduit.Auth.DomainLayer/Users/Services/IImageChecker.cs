using System.Threading;
using System.Threading.Tasks;

namespace Conduit.Auth.DomainLayer.Users.Services;

public interface IImageChecker
{
    Task<bool> CheckImageAsync(
        string url,
        CancellationToken cancellationToken = default);
}
