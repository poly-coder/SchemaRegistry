using FluentValidation;

namespace Pico.Domain.FluentValidation;

public static class PicoDomainValidationExtensions
{
    public static async Task<T> ValidateWithAsync<T>(
        this T value,
        IValidator<T> validator,
        CancellationToken cancel = default
    )
    {
        await validator.ValidateAndThrowAsync(value, cancellationToken: cancel);

        return value;
    }

    public static T ValidateWith<T>(this T value, IValidator<T> validator)
    {
        validator.ValidateAndThrow(value);

        return value;
    }
}
