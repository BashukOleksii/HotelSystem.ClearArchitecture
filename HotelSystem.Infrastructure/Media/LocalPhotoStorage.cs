using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Results;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Common.Validation;
using Microsoft.Extensions.Options;

namespace HotelSystem.Infrastructure.Media;

public sealed class LocalPhotoStorage : IPhotoStorage
{
    private static readonly Regex KeyFormat = new(@"\Aphotos/[0-9a-f]{32}\.(jpg|png|webp)\z", RegexOptions.NonBacktracking);
    private readonly string _root;
    private readonly PhotoUrlBuilder _urls;

    public LocalPhotoStorage(IOptions<PhotoStorageOptions> options, PhotoUrlBuilder urls)
    {
        var config = options.Value;
        if (!config.IsValid()) throw new OptionsValidationException(Options.DefaultName, typeof(PhotoStorageOptions), ["Некоректні налаштування PhotoStorage."]);
        _root = Path.GetFullPath(config.RootDirectory);
        Directory.CreateDirectory(_root);
        _urls = urls;
    }

    public async Task<StoredPhoto> UploadAsync(PhotoUpload upload, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(upload);
        await new PhotoUploadValidator().ValidateAndThrowAsync(upload, cancellationToken);
        var contentType = upload.ContentType.Trim().ToLowerInvariant();
        var extension = contentType switch { "image/jpeg" => ".jpg", "image/png" => ".png", _ => ".webp" };
        var id = Guid.NewGuid().ToString("N");
        var key = "photos/" + id + extension;
        var temporaryPath = Path.Combine(_root, id + ".tmp");
        try
        {
            using var content = new MemoryStream();
            var buffer = new byte[81920];
            int read;
            while ((read = await upload.Content.ReadAsync(buffer, cancellationToken)) > 0)
            {
                if (content.Length + read > PhotoUpload.MaximumBytes) throw Invalid("Фотографія перевищує 5 MiB.");
                await content.WriteAsync(buffer.AsMemory(0, read), cancellationToken);
            }
            var bytes = content.ToArray();
            if (!MatchesSignature(bytes, contentType)) throw Invalid("Вміст файлу не відповідає заявленому формату фотографії.");

            await using (var output = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, FileOptions.Asynchronous))
            {
                await output.WriteAsync(bytes, cancellationToken);
                await output.FlushAsync(cancellationToken);
            }
            cancellationToken.ThrowIfCancellationRequested();
            File.Move(temporaryPath, ResolvePath(key), overwrite: false);
            return new(key, _urls.Build(key));
        }
        finally { if (File.Exists(temporaryPath)) File.Delete(temporaryPath); }
    }

    public Task<PhotoFile?> OpenReadAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var path = ResolvePath(storageKey);
        var contentType = Path.GetExtension(path) switch { ".jpg" => "image/jpeg", ".png" => "image/png", _ => "image/webp" };
        try
        {
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete, 81920, FileOptions.Asynchronous);
            return Task.FromResult<PhotoFile?>(new(stream, contentType));
        }
        catch (FileNotFoundException) { return Task.FromResult<PhotoFile?>(null); }
        catch (DirectoryNotFoundException) { return Task.FromResult<PhotoFile?>(null); }
    }

    public Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        File.Delete(ResolvePath(storageKey)); 
        return Task.CompletedTask;
    }

    public static bool IsManagedKey(string? key) => key is { Length: <= 45 } && KeyFormat.IsMatch(key);

    private string ResolvePath(string key)
    {
        if (!IsManagedKey(key)) throw new ArgumentException("Некоректний ключ фотографії.", nameof(key));
        return Path.Combine(_root, key[7..]);
    }

    private static bool MatchesSignature(byte[] bytes, string type) => type switch
    {
        "image/jpeg" => bytes.Length >= 3 && bytes[0] == 0xff && bytes[1] == 0xd8 && bytes[2] == 0xff,
        "image/png" => bytes.Length >= 8 && bytes.AsSpan(0, 8).SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }),
        "image/webp" => bytes.Length >= 12 && bytes.AsSpan(0, 4).SequenceEqual("RIFF"u8) && bytes.AsSpan(8, 4).SequenceEqual("WEBP"u8),
        _ => false
    };

    private static ValidationException Invalid(string message) => new([new ValidationFailure("Upload", message)]);
}
