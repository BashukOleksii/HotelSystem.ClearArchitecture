namespace HotelSystem.Application.Common.Exceptions;

public sealed class AuthenticationFailedException()
    : Exception("Не вдалося підтвердити облікові дані.");
