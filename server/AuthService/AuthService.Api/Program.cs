using AuthService.Api.Extensions;
using AuthService.Infrastructure.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(builder.Configuration["FrontendHost"])
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddOptions(builder.Configuration);

builder.Services.AddDatabaseMigrations();
builder.Services.AddDatabaseConnection();
builder.Services.AddBearerAuthentication(builder.Configuration);

builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddUseCases();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.ApplyMigrations();

app.Run();