using System.Text;
using HotelSystem.Application.Features.Photos.Queries.GetPhotoFile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace HotelSystem.API.Controllers;

[ApiController]
[Route("api/media")]
[AllowAnonymous]
public sealed class MediaController(ISender sender) : ControllerBase
{
    [HttpGet("{token}")]
    public async Task<IActionResult> Get(string token, CancellationToken ct)
    {
        if (token.Length > 128) return NotFound();
        string key;
        try { key = new UTF8Encoding(false, true).GetString(WebEncoders.Base64UrlDecode(token)); }
        catch (Exception error) when (error is FormatException or ArgumentException) { return NotFound(); }
        var photo = await sender.Send(new GetPhotoFileQuery(key), ct);
        return File(photo.Content, photo.ContentType, enableRangeProcessing: true);
    }
}
