using Infrastructure.Data.Connection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<MySqlConnectionFactory>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();