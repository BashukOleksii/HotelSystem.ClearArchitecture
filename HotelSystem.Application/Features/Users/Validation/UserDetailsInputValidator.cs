using System.Net.Mail;
using FluentValidation;
using HotelSystem.Application.Features.Users.DTOs;

namespace HotelSystem.Application.Features.Users.Validation;

public sealed class UserDetailsInputValidator : AbstractValidator<UserDetailsInput>
{
    public UserDetailsInputValidator()
    {
        RuleFor(x => x.Username).Must(x => x is not null && x.Trim().Length is >= 3 and <= 50)
            .WithMessage("Ім'я користувача має містити від 3 до 50 символів після обрізання пробілів.");
        RuleFor(x => x.Email).Must(x => x is not null && x.Trim().Length <= 254
            && MailAddress.TryCreate(x.Trim(), out var address) && address.Address == x.Trim())
            .WithMessage("Вкажіть коректну адресу електронної пошти до 254 символів.");
        RuleFor(x => x.PhoneNumber).Must(x => x is null || x.Trim().Length <= 30)
            .WithMessage("Телефон має містити до 30 символів.");
    }
}
