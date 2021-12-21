using System;
using Conduit.Auth.Domain.Services.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Auth.Infrastructure.Dapper.DependencyInjection;

public class ServiceProviderUnitOfWork : IUnitOfWork
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceProviderUnitOfWork(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    #region IUnitOfWork Members

    public TRepository? GetRepository<TRepository>()
        where TRepository : IRepository
    {
        var service = _serviceProvider.GetService<TRepository>();
        return service;
    }

    #endregion
}
