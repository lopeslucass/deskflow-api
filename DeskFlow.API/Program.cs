using DeskFlow.API.Data;
using Microsoft.EntityFrameworkCore;
using DeskFlow.API.Repositories;
using DeskFlow.API.Services;
using DeskFlow.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<CategoriaRepository>();
builder.Services.AddScoped<CategoriaService>();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

