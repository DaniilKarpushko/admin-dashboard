using AuthService.Grpc.Extensions;
using AuthService.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions(builder.Configuration);

builder.Services.AddDatabaseMigrations();
builder.Services.AddDatabaseConnection();
builder.Services.AddBearerAuthentication(builder.Configuration);

builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddUseCases();

builder.Services.AddGrpc();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.Run();