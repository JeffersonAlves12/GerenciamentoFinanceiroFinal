using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinanceiroRazorTDS.Models;

namespace FinanceiroRazorTDS.Data
{
   using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Defina seus DbSets para as tabelas do banco de dados aqui. Por exemplo:
     public DbSet<UsuarioModel> Usuarios { get; set; }
     public DbSet<CategoriaModel> Categorias { get; set; }
     public DbSet<TransacaoModel> Transacoes { get; set; }
     public DbSet<InvestimentoModel> Investimentos { get; set; }
     public DbSet<EventoModel> Eventos { get; set; }






    protected override void OnModelCreating(ModelBuilder modelBuilder){
        
     

         modelBuilder.Entity<TransacaoModel>()
        .HasMany(s => s.Categorias)
        .WithOne(t => t.Transacoes)
        .HasForeignKey(t => t.TransacaoId)
        .IsRequired(false);

        modelBuilder.Entity<UsuarioModel>()
        .HasMany(s => s.Transacoes)
        .WithOne(t => t.Usuario)
        .HasForeignKey(t => t.UsuarioId)
        .IsRequired(false);

        modelBuilder.Entity<UsuarioModel>()
        .HasMany(u => u.Investimentos) 
        .WithOne(i => i.Usuario)
        .HasForeignKey(i => i.UsuarioId);
    
         modelBuilder.Entity<UsuarioModel>()
            .HasMany(u => u.Eventos)
            .WithOne(e => e.Usuario)
            .HasForeignKey(e => e.UsuarioId);


    }

}
}
