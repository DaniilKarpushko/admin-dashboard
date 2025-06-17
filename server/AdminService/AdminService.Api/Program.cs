using AdminService.Api.Extensions;

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.ApplyMigrations();

app.Run();