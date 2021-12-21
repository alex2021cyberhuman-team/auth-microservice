using System;
using Conduit.Auth.Domain.Services;

namespace Conduit.Auth.Infrastructure.Services;

public class IdManager : IIdManager
{
    #region IIdManager Members

    public Guid GenerateId()
    {
        return Guid.NewGuid();
    }

    #endregion
}
