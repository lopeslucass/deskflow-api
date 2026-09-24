using DeskFlow.API.Data;
using DeskFlow.API.Models.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Repositories;

public class CategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Categoria>> ListarAsync()
    {
        return await _context.Categorias.ToListAsync();
    }

    public async Task<Categoria?> BuscarPorIdAsync(int id)
    {
        return await _context.Categorias.FindAsync(id);
    }

    public async Task AdicionarAsync(Categoria categoria)
    {
        await _context.Categorias.AddAsync(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task ExcluirAsync(Categoria categoria)
    {
        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> PossuiChamadosAsync(int categoriaId)
    {
        return await _context.Chamados.AnyAsync(chamado => chamado.CategoriaId == categoriaId);
    }
}