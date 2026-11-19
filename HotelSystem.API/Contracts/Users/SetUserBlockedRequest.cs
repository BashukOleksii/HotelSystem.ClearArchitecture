using System.ComponentModel.DataAnnotations;
namespace HotelSystem.API.Contracts.Users;
public sealed record SetUserBlockedRequest([Required] bool? IsBlocked);
