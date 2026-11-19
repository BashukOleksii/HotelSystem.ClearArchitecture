using System.ComponentModel.DataAnnotations;
using HotelSystem.Domain.Enums;

namespace HotelSystem.API.Contracts.Hotels;

public sealed record ModerateHotelRequest([Required] HotelModerationStatus? Status);
