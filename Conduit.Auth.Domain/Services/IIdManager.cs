using System;

namespace Conduit.Auth.Domain.Services;

public interface IIdManager
{
    Guid GenerateId();
}
