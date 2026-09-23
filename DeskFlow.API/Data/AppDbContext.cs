using DeskFlow.API.Models.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Categoria> Categorias {get; set;}
    public DbSet<Chamado> Chamados {get; set;}
    public DbSet<Interacao> Interacoes {get; set;}
    
}