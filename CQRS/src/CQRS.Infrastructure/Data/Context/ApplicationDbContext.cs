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

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries().Where(x => x.Entity is Bank && (x.State == EntityState.Added || x.State == EntityState.Modified));
        foreach (var entry in entries)
        {
            var bank = (Bank)entry.Entity;
            var now = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                bank.CreatedDateTime = now;
                bank.UpdatedDateTime = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                bank.UpdatedDateTime = now;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
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

        modelBuilder.Entity<Bank>()
            .Property(x => x.CreatedBy)
            .HasMaxLength(50);
        modelBuilder.Entity<Bank>()
            .Property(x => x.CreatedDateTime);
        modelBuilder.Entity<Bank>()
            .Property(x => x.UpdatedBy)
            .HasMaxLength(50);
        modelBuilder.Entity<Bank>()
            .Property(x => x.UpdatedDateTime);
    }
}