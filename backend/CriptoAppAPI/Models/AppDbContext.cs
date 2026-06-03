using Microsoft.EntityFrameworkCore;

namespace CriptoAppAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Una transacción pertenece a un cliente
            modelBuilder.Entity<Transaccion>()
                .HasOne(t => t.cliente)
                .WithMany()
                .HasForeignKey(t => t.cliente_id)
                .IsRequired(false); 
        }
    }
}