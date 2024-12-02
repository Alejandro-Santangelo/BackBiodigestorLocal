using Microsoft.EntityFrameworkCore;
using Biodigestor.Models;
using Biodigestor.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Biodigestor.Data
{
    public class BiodigestorContext : IdentityDbContext<ApplicationUser>
    {
        public BiodigestorContext(DbContextOptions<BiodigestorContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Domicilio> Domicilios { get; set; }
        public DbSet<Biodigestor.Models.Biodigestor> BiodigestorEntities { get; set; }
        public DbSet<Acumulador> Acumuladores { get; set; }
        public DbSet<SensorTemperatura> SensoresTemperatura { get; set; }
        public DbSet<SensorHumedad> SensoresHumedad { get; set; }
        public DbSet<SensorPresion> SensoresPresion { get; set; }
        public DbSet<ValvulaAgua> ValvulasAgua { get; set; }
        public DbSet<ValvulaPresion> ValvulasPresion { get; set; }
        public DbSet<Agitador> Agitadores { get; set; }
        public DbSet<UsuarioRegistradoModel> UsuariosRegistrados { get; set; }
        
        public DbSet<Calentador> Calentadores { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Registro> Registros { get; set; }
        
        
        public DbSet<Personal> Personal { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

             modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.AcceptedTerms)
                      .IsRequired()
                      .HasDefaultValue(false); // Valor por defecto
            });
           

            // Configuración para Cliente
            modelBuilder.Entity<Cliente>()
                .HasKey(c => c.NumeroCliente);

            // Configuración para Domicilio
            modelBuilder.Entity<Domicilio>()
                .HasKey(d => d.NumeroMedidor);

            // Configuración para Factura
            modelBuilder.Entity<Factura>()
                .HasKey(f => f.NumeroFactura);

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Facturas)
                .WithOne(f => f.Cliente)
                .HasForeignKey(f => f.NumeroCliente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Domicilio>()
                .HasMany(d => d.Facturas)
                .WithOne(f => f.Domicilio)
                .HasForeignKey(f => f.NumeroMedidor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Domicilio>()
                .HasOne(d => d.Cliente)
                .WithMany(c => c.Domicilios)
                .HasForeignKey(d => d.NumeroCliente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Cliente)
                .WithMany(c => c.Facturas)
                .HasForeignKey(f => f.NumeroCliente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Domicilio)
                .WithMany(d => d.Facturas)
                .HasForeignKey(f => f.NumeroMedidor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SensorHumedad>()
                .ToTable("SensoresHumedad");

            modelBuilder.Entity<SensorPresion>()
                .ToTable("SensoresPresion");

            modelBuilder.Entity<SensorTemperatura>()
                .ToTable("SensoresTemperatura");

            modelBuilder.Entity<Factura>()
                .Property(f => f.ConsumoMensual)
                .HasColumnType("decimal(10, 2)");

            modelBuilder.Entity<Factura>()
                .Property(f => f.ConsumoTotal)
                .HasColumnType("decimal(10, 2)");

            modelBuilder.Entity<SensorHumedad>()
                .HasKey(sh => sh.IdSensor);

            modelBuilder.Entity<SensorTemperatura>()
                .HasKey(st => st.IdSensor);

            modelBuilder.Entity<SensorPresion>()
                .HasKey(sp => sp.IdSensor);

            modelBuilder.Entity<Biodigestor.Models.Biodigestor>()
                .HasKey(b => b.IdBiodigestor);

            modelBuilder.Entity<Agitador>()
                .HasKey(a => a.IdAgitador);

            modelBuilder.Entity<Calentador>()
                .HasKey(c => c.IdCalentador);

            modelBuilder.Entity<Acumulador>()
                .HasKey(a => a.IdAcumulador);

            modelBuilder.Entity<ValvulaAgua>()
                .HasKey(v => v.IdValvulaAgua);

            modelBuilder.Entity<ValvulaPresion>()
                .HasKey(v => v.IdValvulaPresion);
        }
    }
}



