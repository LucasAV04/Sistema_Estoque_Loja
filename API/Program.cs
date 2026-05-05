using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using Infrastructure.Data.Connection;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Repositories.MySql;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(typeof(ProdutoProfile).Assembly);


builder.Services.AddScoped<MySqlConnectionFactory>();

builder.Services.AddScoped<IProdutoRepository, ProdutoMySqlRepository>();
builder.Services.AddScoped<IEstoqueRepository, EstoqueMySqlRepository>();
builder.Services.AddScoped<IMovimentacaoEstoqueRepository, MovimentacaoEstoqueMySqlRepository>();

builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IEstoqueService, EstoqueService>();
builder.Services.AddScoped<IMovimentacaoEstoqueService, MovimentacaoEstoqueService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();