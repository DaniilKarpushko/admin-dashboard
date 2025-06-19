using AdminService.Api.Extensions;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(builder.Configuration["FrontendHost"])
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddOptions(builder.Configuration);
builder.Services.AddBearerAuthentication(builder.Configuration);

builder.Services.AddDatabaseConnection();
builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddDatabaseMigrations();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.None,
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.ApplyMigrations();

app.Run();