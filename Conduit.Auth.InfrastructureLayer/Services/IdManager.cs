using System;
using Conduit.Auth.DomainLayer.Services;

namespace Conduit.Auth.InfrastructureLayer.Services;

public class IdManager : IIdManager
{
    public Guid GenerateId()
    {
        return Guid.NewGuid();
    }
}
