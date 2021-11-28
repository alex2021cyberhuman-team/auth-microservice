using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Users.Services;
using FluentValidation;
using FluentValidation.Validators;

namespace Conduit.Auth.ApplicationLayer.Users.Shared
{
    public class ImagePropertyValidator<T> : AsyncPropertyValidator<T, string?>
    {
        private readonly IImageChecker _imageChecker;

        public ImagePropertyValidator(IImageChecker imageChecker)
        {
            _imageChecker = imageChecker;
        }

        public override string Name => nameof(ImagePropertyValidator<T>);

        public override async Task<bool> IsValidAsync(
            ValidationContext<T> context,
            string? value,
            CancellationToken cancellation)
        {
            if (value is null)
            {
                return true;
            }

            if (await _imageChecker.CheckImageAsync(value, cancellation))
            {
                return true;
            }

            context.AddFailure(
                $"Cannot access this url or invalid format: {value}");
            return false;
        }
    }
}
