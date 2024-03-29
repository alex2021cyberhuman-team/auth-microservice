using System;
using System.Reflection;

namespace Conduit.Auth.DomainLayer.Services.DataAccess;

public static class UnitOfWorkExtensions
{
    private static readonly MethodInfo _genericMethod =
        typeof(IUnitOfWork).GetMethod(nameof(IUnitOfWork.GetRepository),
            BindingFlags.Public | BindingFlags.Instance)!;

    public static object? GetRepository(
        this IUnitOfWork unitOfWork,
        Type repositoryType)
    {
        if (!typeof(IRepository).IsAssignableFrom(repositoryType))
        {
            throw new ArgumentException("Invalid repositoryType",
                nameof(repositoryType));
        }

        var repository = _genericMethod.MakeGenericMethod(repositoryType)
            .Invoke(unitOfWork, Array.Empty<object>());
        return repository;
    }

    public static TRepository GetRequiredRepository<TRepository>(
        this IUnitOfWork unitOfWork) where TRepository : IRepository
    {
        return unitOfWork.GetRepository<TRepository>() ??
               throw new InvalidOperationException(
                   $"Cannot access to {typeof(TRepository)} in {unitOfWork.GetType()}");
    }
}
