using HotelSystem.Application.Common.Models;
using MediatR;
namespace HotelSystem.Application.Features.Photos.Queries.GetPhotoFile;
public sealed record GetPhotoFileQuery(string StorageKey) : IRequest<PhotoFile>;
