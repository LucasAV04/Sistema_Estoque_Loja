using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using Infrastructure.Data.Connection;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Repositories.MySql;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(ProdutoProfile).Assembly);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sistema Estoque API",
        Version = "v1"
    });

    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Digite sua API Key",
        Type = SecuritySchemeType.ApiKey,
        Name = "X-Api-Key",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});



builder.Services.AddCors(options =>
{
    options.AddPolicy("MinhaPolitica", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


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



app.UseCors("MinhaPolitica");



app.UseHttpsRedirection();



app.Use(async (context, next) =>
{
    
    if (context.Request.Path.StartsWithSegments("/swagger"))
    {
        await next();
        return;
    }

    var apiKey = builder.Configuration["ApiKey"];

    if (!context.Request.Headers.TryGetValue("X-Api-Key", out var receivedKey))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("API Key ausente.");
        return;
    }

    if (receivedKey != apiKey)
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("API Key inválida.");
        return;
    }

    await next();
});



app.MapControllers();

app.Run();