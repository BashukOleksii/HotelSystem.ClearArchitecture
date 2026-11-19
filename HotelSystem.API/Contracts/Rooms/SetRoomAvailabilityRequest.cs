using System.ComponentModel.DataAnnotations;

namespace HotelSystem.API.Contracts.Rooms;

public sealed record SetRoomAvailabilityRequest([Required] bool? IsAvailable);
