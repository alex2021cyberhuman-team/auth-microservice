using Conduit.Auth.Infrastructure.Dapper.Users.Mappings;
using FluentMigrator;

namespace Conduit.Auth.Infrastructure.Dapper.Migrations
{
    [Migration(0)]
    public class M000_CreateTable_Users : Migration
    {
        public override void Up()
        {
            Create.Table($"\"{UsersColumns.TableName}\"")
                .WithColumn($"\"{UsersColumns.Id}\"")
                .AsGuid()
                .NotNullable()
                .PrimaryKey()
                .WithColumn($"\"{UsersColumns.Username}\"")
                .AsString(1000)
                .NotNullable()
                .Unique()
                .WithColumn($"\"{UsersColumns.Email}\"")
                .AsString(1000)
                .NotNullable()
                .Unique()
                .WithColumn($"\"{UsersColumns.Password}\"")
                .AsString(1000)
                .NotNullable()
                .WithColumn($"\"{UsersColumns.Image}\"")
                .AsString(1000)
                .Nullable()
                .WithColumn($"\"{UsersColumns.Biography}\"")
                .AsString(1000)
                .Nullable();
            
            this.Execute.Sql($@"
ALTER TABLE ""{UsersColumns.TableName}""
ADD COLUMN ""{UsersColumns.EmailLower}""
varchar(1000) GENERATED ALWAYS AS 
(lower(""{UsersColumns.Email}"")) STORED;");
            
            this.Execute.Sql($@"
CREATE UNIQUE INDEX ON ""{UsersColumns.TableName}"" 
(""{UsersColumns.EmailLower}"");");
        }

        public override void Down()
        {
            Delete.Table($"\"{UsersColumns.TableName}\"");
        }
    }
}
