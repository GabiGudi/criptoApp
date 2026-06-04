using CriptoAppAPI.Interfaces;
using CriptoAppAPI.Models;
using CriptoAppAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controladores
builder.Services.AddControllers();

// Base de datos
builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient<ICriptoYaServicio, CriptoYaServicio>();
builder.Services.AddScoped<ITransaccionServicio, TransaccionServicio>();

// CORS - permite que el frontend se comunique con la API
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

//app.UseHttpsRedirection();
app.UseCors("PermitirTodo");
app.UseAuthorization();
app.MapControllers();

app.Run();