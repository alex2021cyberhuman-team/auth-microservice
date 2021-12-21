using Conduit.Auth.Domain.Users;
using Dapper.FluentMap.Mapping;

namespace Conduit.Auth.Infrastructure.Dapper.Users.Mappings;

public class UsersTableMap : EntityMap<User>
{
    public UsersTableMap()
    {
        Map(user => user.Id).ToColumn(UsersColumns.Id);
        Map(user => user.Username).ToColumn(UsersColumns.Username);
        Map(user => user.Email).ToColumn(UsersColumns.Email);
        Map(user => user.Password).ToColumn(UsersColumns.Password);
        Map(user => user.Image).ToColumn(UsersColumns.Image);
        Map(user => user.Biography).ToColumn(UsersColumns.Biography);
    }
}
