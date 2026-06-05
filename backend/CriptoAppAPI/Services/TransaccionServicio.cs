using CriptoAppAPI.DTOs;
using CriptoAppAPI.Interfaces;
using CriptoAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CriptoAppAPI.Services
{
    public class TransaccionServicio : ITransaccionServicio
    {
        private readonly AppDbContext _contexto;
        private readonly ICriptoYaServicio _criptoYaServicio;

        public TransaccionServicio(AppDbContext contexto, ICriptoYaServicio criptoYaServicio)
        {
            _contexto = contexto;
            _criptoYaServicio = criptoYaServicio;
        }

        public async Task<List<TransaccionDTO>> ObtenerTodas()
        {
            return await _contexto.Transacciones
                .OrderByDescending(t => t.datetime)
                .Select(t => new TransaccionDTO
                {
                    id = t.id,
                    crypto_code = t.crypto_code,
                    action = t.action,
                    crypto_amount = t.crypto_amount,
                    money = t.money,
                    datetime = t.datetime
                })
                .ToListAsync();
        }

        public async Task<TransaccionDTO> Crear(CrearTransaccionDTO dto)
        {
            
            decimal precio = await _criptoYaServicio.ObtenerPrecio(dto.crypto_code);

            decimal total = precio * dto.crypto_amount;

            var transaccion = new Transaccion
            {
                crypto_code = dto.crypto_code,
                action = dto.action,
                crypto_amount = dto.crypto_amount,
                money = total,
                datetime = dto.datetime
            };

            _contexto.Transacciones.Add(transaccion);
            await _contexto.SaveChangesAsync();

            return new TransaccionDTO
            {
                id = transaccion.id,
                crypto_code = transaccion.crypto_code,
                action = transaccion.action,
                crypto_amount = transaccion.crypto_amount,
                money = transaccion.money,
                datetime = transaccion.datetime
            };
        }
    }
}
