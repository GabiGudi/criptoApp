using CriptoAppAPI.Interfaces;
using CriptoAppAPI.Models;
using CriptoAppAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient<ICriptoYaServicio, CriptoYaServicio>();
builder.Services.AddScoped<ITransaccionServicio, TransaccionServicio>();
builder.Services.AddScoped<IClienteServicio, ClienteServicio>();

builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy("PermitirTodo", politica =>
    {
        politica.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("PermitirTodo");
app.UseAuthorization();
app.MapControllers();

app.Run();