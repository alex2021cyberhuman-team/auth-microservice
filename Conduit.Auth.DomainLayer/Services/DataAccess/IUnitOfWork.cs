namespace Conduit.Auth.DomainLayer.Services.DataAccess;

public interface IUnitOfWork
{
    TRepository? GetRepository<TRepository>() where TRepository : IRepository;
}
