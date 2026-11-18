using FluentValidation;

namespace HotelSystem.Application.Features.Hotels.Queries.GetMyHotels;

public sealed class GetMyHotelsQueryValidator
    : AbstractValidator<GetMyHotelsQuery>
{
    public GetMyHotelsQueryValidator()
    {
        RuleFor(query => query.ActorId)
            .NotEmpty()
            .WithMessage(
                "Потрібно визначити виконавця операції.");

        RuleFor(query => query.Search)
            .MaximumLength(100)
            .WithMessage(
                "Пошуковий запит має містити до 100 символів.");

        RuleFor(query => query.City)
            .MaximumLength(100)
            .WithMessage(
                "Назва міста має містити до 100 символів.");

        RuleFor(query => query.Status)
            .IsInEnum()
            .When(query => query.Status.HasValue)
            .WithMessage(
                "Невідомий статус модерації.");

        RuleFor(query => query.Page)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage(
                "Номер сторінки має бути додатним.")
            .Must((query, page) =>
                query.PageSize <= 0
                || ((long)page - 1) * query.PageSize <= int.MaxValue)
            .WithMessage(
                "Номер сторінки завеликий.");

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage(
                "Розмір сторінки має бути від 1 до 100.");
    }
}