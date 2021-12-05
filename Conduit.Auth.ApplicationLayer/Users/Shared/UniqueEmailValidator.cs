using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users;
using Conduit.Auth.Domain.Users.Repositories;
using FluentValidation;
using FluentValidation.Validators;

namespace Conduit.Auth.ApplicationLayer.Users.Shared;

public class UniqueEmailValidator<T> : AsyncPropertyValidator<T, string?>
{
    private readonly ICurrentUserProvider? _currentUserProvider;
    private readonly IUsersFindByEmailRepository _findByEmailRepository;

    public UniqueEmailValidator(
        IUsersFindByEmailRepository findByEmailRepository,
        ICurrentUserProvider? currentUserProvider = null)
    {
        _findByEmailRepository = findByEmailRepository;
        _currentUserProvider = currentUserProvider;
    }

    public override string Name => nameof(UniqueEmailValidator<T>);

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
            await _findByEmailRepository.FindByEmailAsync(value, cancellation);

        return await user.CheckCurrentUser(_currentUserProvider, cancellation);
    }
}
