using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users;
using Conduit.Auth.Domain.Users.Repositories;
using FluentValidation;
using FluentValidation.Validators;

namespace Conduit.Auth.ApplicationLayer.Users.Shared
{
    public class UniqueUsernameValidator<T> : AsyncPropertyValidator<T, string?>
    {
        private readonly ICurrentUserProvider? _currentUserProvider;
        private readonly IUsersFindByUsernameRepository _usernameRepository;

        public UniqueUsernameValidator(
            IUsersFindByUsernameRepository usernameRepository,
            ICurrentUserProvider? currentUserProvider = null)
        {
            _usernameRepository = usernameRepository;
            _currentUserProvider = currentUserProvider;
        }

        public override string Name => nameof(UniqueUsernameValidator<T>);

        public override async Task<bool> IsValidAsync(
            ValidationContext<T> context,
            string? value,
            CancellationToken cancellation)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            var user =
                await _usernameRepository.FindByUsernameAsync(
                    value,
                    cancellation);

            return await user.CheckCurrentUser(
                _currentUserProvider,
                cancellation);
        }
    }
}
