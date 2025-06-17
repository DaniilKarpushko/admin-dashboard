using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;

namespace AdminService.Infrastructure.Migrations;

[Migration(version: 1, description: "Initial migration")]
public class InitialMigration : IMigration
{
    public void GetUpExpressions(IMigrationContext context)
    {
        context.Expressions.Add(new ExecuteSqlStatementExpression
        {
            SqlStatement = """
                               create table clients (
                                   id serial primary key,
                                   name varchar(100) not null,
                                   email varchar(200) not null,
                                   balance_t numeric(18, 2) not null default 0
                               );
                           
                               create table payments (
                                   id serial primary key,
                                   client_id integer not null references clients(id),
                                   total numeric(18, 2) not null,
                                   token_amount integer not null,
                                   created_at timestamp with time zone not null
                               );
                           
                               create table rate_history (
                                   id serial primary key,
                                   rate numeric(18, 2) not null,
                                   created_at timestamp with time zone not null
                               );
                           
                               insert into clients (name, email, balance_t) values
                               ('Ivan', 'ivan@mail.ru', 123.23),
                               ('Anatoly', 'tolya@gmail.com', 24.00),
                               ('Alexandr', 'sahek@mail.ru', 223.20),
                               ('Alexandr', 'SAHEK@mail.ru', 10.00),
                               ('Irina', 'ir4ka@yandex.ru', 123.32),
                               ('Roman', 'rrr@gmail.com', 1.00);
                           
                               insert into payments (client_id, total, token_amount, created_at) values
                               (1, 100.00, 10, now() - interval '5 days'),
                               (2, 50.00, 5, now() - interval '4 days'),
                               (3, 75.00, 8, now() - interval '3 days'),
                               (4, 125.00, 15, now() - interval '2 days'),
                               (1, 200.00, 20, now() - interval '1 days');
                           
                               insert into rate_history (rate, created_at) values
                               (1.10, now() - interval '6 days'),
                               (1.15, now() - interval '5 days'),
                               (1.18, now() - interval '4 days'),
                               (1.21, now() - interval '3 days'),
                               (1.25, now() - interval '2 days'),
                               (1.30, now() - interval '1 days');
                           """,
        });
    }

    public void GetDownExpressions(IMigrationContext context)
    {
        context.Expressions.Add(new ExecuteSqlStatementExpression
        {
            SqlStatement = """
                           drop table if exists rate_history;
                           drop table if exists payments;
                           drop table if exists clients; 
                           """,
        });
    }

    public string ConnectionString => throw new NotSupportedException();
}