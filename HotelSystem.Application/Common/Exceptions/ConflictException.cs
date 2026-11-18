namespace HotelSystem.Application.Common.Exceptions;

public sealed class ConflictException(string message) : Exception(message);
