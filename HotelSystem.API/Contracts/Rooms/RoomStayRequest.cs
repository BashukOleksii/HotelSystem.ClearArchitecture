using System.ComponentModel.DataAnnotations;
using HotelSystem.Application.Features.Rooms.Queries;

namespace HotelSystem.API.Contracts.Rooms;

public class RoomStayRequest : IValidatableObject
{
    public DateOnly? CheckIn { get; set; }
    public DateOnly? CheckOut { get; set; }

    public RoomStay? ToStay() => CheckIn.HasValue && CheckOut.HasValue ? new(CheckIn.Value, CheckOut.Value) : null;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CheckIn.HasValue != CheckOut.HasValue)
            yield return new ValidationResult("Передайте разом checkIn та checkOut.", [nameof(CheckIn), nameof(CheckOut)]);
    }
}
