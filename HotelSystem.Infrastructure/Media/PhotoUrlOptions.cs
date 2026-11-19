namespace HotelSystem.Infrastructure.Media;

public sealed class PhotoUrlOptions
{
    public string PublicBaseUrl { get; set; } = string.Empty;
    public string RoutePrefix { get; set; } = "/api/media";

    internal bool IsValid() => PublicBaseUrl is not null && RoutePrefix is not null && (PublicBaseUrl.Length == 0 ||
        (Uri.TryCreate(PublicBaseUrl, UriKind.Absolute, out var uri)
         && uri.Scheme is "http" or "https" && uri.Query.Length == 0
         && uri.Fragment.Length == 0 && uri.UserInfo.Length == 0))
        && RoutePrefix.StartsWith('/') && !RoutePrefix.StartsWith("//")
        && !RoutePrefix.Contains('\\') && !RoutePrefix.Contains('?') && !RoutePrefix.Contains('#')
        && !RoutePrefix.Split('/').Any(x => x is "." or "..");
}
