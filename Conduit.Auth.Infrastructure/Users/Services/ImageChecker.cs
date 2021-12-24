using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Users.Services;

namespace Conduit.Auth.Infrastructure.Users.Services;

public class ImageChecker : IImageChecker
{
    private readonly HttpClient _client;

    public ImageChecker(
        HttpClient client)
    {
        _client = client;
    }
    
    async Task<bool> IImageChecker.CheckImageAsync(
        string url,
        CancellationToken cancellationToken)
    {
        var message = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await _client.SendAsync(message,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        return response.Content.Headers.ContentLength <= 10_000_000;
    }
}
