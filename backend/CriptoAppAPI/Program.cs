using CriptoAppAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controladores
builder.Services.AddControllers();

// Base de datos
builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseHttpsRedirection();
app.UseCors("PermitirTodo");
app.UseAuthorization();
app.MapControllers();

app.Run();