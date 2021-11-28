using System.Data;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Conduit.Auth.Infrastructure.Dapper.Extensions
{
    public static class SqlKataExtensions
    {
        public static QueryFactory Get(
            this IDbConnection connection,
            Compiler compiler)
        {
            return new(connection, compiler, connection.ConnectionTimeout);
        }
    }
}
