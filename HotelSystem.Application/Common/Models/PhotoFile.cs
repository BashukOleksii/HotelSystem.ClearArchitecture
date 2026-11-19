namespace HotelSystem.Application.Common.Models;

// The caller owns Content and must dispose it (an ASP.NET FileResult does this).
public sealed record PhotoFile(Stream Content, string ContentType);
