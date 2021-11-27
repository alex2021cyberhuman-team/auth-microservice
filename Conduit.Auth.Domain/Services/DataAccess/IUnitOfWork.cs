namespace Conduit.Auth.Domain.Services.DataAccess
{
    public interface IUnitOfWork
    {
        TRepository? GetRepository<TRepository>()
            where TRepository : IRepository;
    }
}
