using HotelSystem.Domain.Enums;
using HotelSystem.Domain.Exceptions;
namespace HotelSystem.Domain.Rules;
public static class RoomRules
{
    public const int MaximumNumberLength = 20;
    public const int MaximumPhotoCount = 10;
    public const decimal MaximumPrice = 9_999_999_999_999_999.99m;
    public static int MaximumCapacity(RoomType type) => type switch
    {
        RoomType.Single => 1,
        RoomType.Double or RoomType.Twin => 2,
        RoomType.Suite => 4,
        RoomType.Family => 5,
        _ => throw new DomainValidationException(nameof(type), "Невідомий тип номера.")
    };
}
