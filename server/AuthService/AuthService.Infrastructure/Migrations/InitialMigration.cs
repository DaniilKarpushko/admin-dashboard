using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;

namespace AuthService.Infrustructure.Migrations;

[Migration(version: 1, description: "Initial migration")]
public class InitialMigration : IMigration
{
    public void GetUpExpressions(IMigrationContext context)
    {
        context.Expressions.Add(new ExecuteSqlStatementExpression
        {
            SqlStatement = """
                           create table users (
                               id uuid primary key,
                               email text not null,
                               name text not null,
                               password text not null,
                               salt text not null
                           );
                           create table refresh_tokens (
                               token text primary key,
                               expiration_date timestamp without time zone not null,
                               user_id uuid not null
                           );

                           create type role as enum ('Admin', 'User');

                           create table user_roles (
                               user_id uuid not null references users(id),
                               role role not null,
                               primary key (user_id, role)
                           );

                           insert into users(id, email, name, password, salt)
                           values ('bafd4d76-dcf6-4d63-bb4d-78bd655aa901',
                                   'admin@mirra.dev',
                                   'Daniil',
                                   'ADE9C8982F71B71AE95913A951FF5B70E04D66FCEB0BEE9F8649B26485E17C3126A6E7F9965BE832B45859992FF2EAEF82DF0D994B0209DA0DFFD9BFC1AFD34295AF6BEE10DF05522FD2B538BB86BCD48F75347C4B24A180D464D9B14AC77CEA79A5CFD431494878BCB5792999A41C4C8D55640DCEEB9205E7AC9929A0CA63DDDA8D1A3C46371D82692B055FCA0F7DC8A847C6DA31D2665D44F993B9AD3233CE726C90BD1B3EE1CEE53981B7C0968811474D5EEB9FB2C6F5E6375A39BBF3B95BCF3D917AADD4380DDF44983978B788ADD152CBE232EEB51B91696DB278B514BC9BB510F74CF021F57886C6C09743670750AC5B73E446BF8AE9CC0D0A33A8AA69',
                                   'aaaa');
                           """
        });
    }

    public void GetDownExpressions(IMigrationContext context)
    {
        context.Expressions.Add(new ExecuteSqlStatementExpression()
        {
            SqlStatement = """
                           drop table if exists user_roles;
                           drop table if exists refresh_tokens;
                           drop table if exists users;
                           drop type if exists role; 
                           """
        });
    }

    public string ConnectionString => throw new NotSupportedException();
}