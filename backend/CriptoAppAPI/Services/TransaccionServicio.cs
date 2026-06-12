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
                    datetime = t.datetime,
                     cliente_id = t.cliente_id
                })
                .ToListAsync();
        }

        public async Task<TransaccionDTO> Crear(CrearTransaccionDTO dto)
        {
            if (dto.action == "sale")
            {
                var transacciones = await _contexto.Transacciones
                    .Where(t => t.cliente_id == dto.cliente_id && t.crypto_code == dto.crypto_code)
                    .ToListAsync();

                decimal totalCompras = transacciones
                    .Where(t => t.action == "purchase")
                    .Sum(t => t.crypto_amount);

                decimal totalVentas = transacciones
                    .Where(t => t.action == "sale")
                    .Sum(t => t.crypto_amount);

                decimal stockActual = totalCompras - totalVentas;

                if (dto.crypto_amount > stockActual)
                {
                    throw new InvalidOperationException($"No tenés suficiente {dto.crypto_code}. " + $"Stock actual: {stockActual}, intentás vender: {dto.crypto_amount}.");
                }
            }
            
            decimal precio = await _criptoYaServicio.ObtenerPrecio(dto.crypto_code);

            decimal total = precio * dto.crypto_amount;

            var transaccion = new Transaccion
            {
                crypto_code = dto.crypto_code,
                action = dto.action,
                crypto_amount = dto.crypto_amount,
                money = total,
                datetime = dto.datetime,
                cliente_id = dto.cliente_id
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
                datetime = transaccion.datetime,
                cliente_id = transaccion.cliente_id
            };
        }

        public async Task<TransaccionDTO?> ObtenerPorId(int id)
        {
            var transaccion = await _contexto.Transacciones.FindAsync(id);

            if (transaccion == null) return null;

            return new TransaccionDTO
            {
                id = transaccion.id,
                crypto_code = transaccion.crypto_code,
                action = transaccion.action,
                crypto_amount = transaccion.crypto_amount,
                money = transaccion.money,
                datetime = transaccion.datetime,
                cliente_id = transaccion.cliente_id
            };
        }

        public async Task<TransaccionDTO?> Actualizar(int id, ActualizarTransaccionDTO dto)
        {
            var transaccion = await _contexto.Transacciones.FindAsync(id);

            if (transaccion == null) return null;

            if (dto.crypto_code != null) transaccion.crypto_code = dto.crypto_code;
            if (dto.action != null) transaccion.action = dto.action;
            if (dto.crypto_amount != null) transaccion.crypto_amount = dto.crypto_amount.Value;
            if (dto.money != null) transaccion.money = dto.money.Value;
            if (dto.datetime != null) transaccion.datetime = dto.datetime.Value;

            await _contexto.SaveChangesAsync();

            return new TransaccionDTO
            {
                id = transaccion.id,
                crypto_code = transaccion.crypto_code,
                action = transaccion.action,
                crypto_amount = transaccion.crypto_amount,
                money = transaccion.money,
                datetime = transaccion.datetime,
                cliente_id = transaccion.cliente_id
            };
        }

        public async Task<bool> Eliminar(int id)
        {
            var transaccion = await _contexto.Transacciones.FindAsync(id);

            if (transaccion == null) return false;

            _contexto.Transacciones.Remove(transaccion);
            await _contexto.SaveChangesAsync();

            return true;
        }
    }
}
