using Infrastructure.Data.Connection;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Repositories.MSql;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<MySqlConnectionFactory>();
builder.Services.AddScoped<IProdutoRepository, ProdutoMySqlRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();