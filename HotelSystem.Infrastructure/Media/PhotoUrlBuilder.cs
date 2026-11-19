using System.Text;
using Microsoft.Extensions.Options;

namespace HotelSystem.Infrastructure.Media;

public sealed class PhotoUrlBuilder(IOptions<PhotoUrlOptions> options)
{
    public string Build(string storageKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(storageKey);
        var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(storageKey)).TrimEnd('=')
            .Replace('+', '-').Replace('/', '_');
        var config = options.Value;
        return config.PublicBaseUrl.TrimEnd('/') + config.RoutePrefix.TrimEnd('/') + "/" + token;
    }
}
