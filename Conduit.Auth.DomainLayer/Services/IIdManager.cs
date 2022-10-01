using System;

namespace Conduit.Auth.DomainLayer.Services;

public interface IIdManager
{
    Guid GenerateId();
}
