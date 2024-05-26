using CQRS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Json;

namespace CQRS.Infrastructure.Data.Context;

public class ApplicationDbContext : DbContext
{
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpClientFactory httpClientFactory) : base(options)
    {
        
    }
    public DbSet<Bank> Banks { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BANK;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar la clave primaria para Bank
        modelBuilder.Entity<Bank>()
            .HasKey(b => b.Bic);

        // Si es necesario, puedes configurar otras propiedades aquí
        modelBuilder.Entity<Bank>()
            .Property(b => b.Name)
            .HasMaxLength(100);

        modelBuilder.Entity<Bank>()
            .Property(b => b.Country)
            .HasMaxLength(50);
    }
}