using Tutorial8.Config;
using Tutorial8.Infrastructure;
using Tutorial8.Repositories;
using Tutorial8.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<ITripsService, TripsService>();
builder.Services.AddScoped<ITripsRepository, TripsRepository>();
builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await initializer.InitializeAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();
app.MapControllers();
await app.RunAsync();