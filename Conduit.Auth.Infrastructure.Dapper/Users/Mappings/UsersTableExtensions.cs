using System.Collections.Generic;
using Conduit.Auth.Domain.Users;

namespace Conduit.Auth.Infrastructure.Dapper.Users.Mappings
{
    public static class UsersTableExtensions
    {
        public static IEnumerable<KeyValuePair<string, object>> AsColumns(
            this User user)
        {
            yield return new(UsersColumns.Id, user.Id);
            yield return new(UsersColumns.Username, user.Username);
            yield return new(UsersColumns.Email, user.Email);
            yield return new(UsersColumns.Password, user.Password);
            if (user.Image is not null)
            {
                yield return new(UsersColumns.Image, user.Image);
            }

            if (user.Biography is not null)
            {
                yield return new(UsersColumns.Biography, user.Biography);
            }
        }
    }
}
