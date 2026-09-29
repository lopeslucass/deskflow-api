using DeskFlow.API.Data;
using DeskFlow.API.Models.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Repositories;

public class ChamadoRepository
{
    private readonly AppDbContext _context;
    
    public ChamadoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Chamado chamado)
    {
        await _context.Chamados.AddAsync(chamado);
        await _context.SaveChangesAsync();
    }

    public async Task<Chamado?> BuscarPorIdAsync(int id)
    {
        return await _context.Chamados
            .Include(chamado => chamado.Categoria)
            .Include(chamado => chamado.Interacoes)
            .FirstOrDefaultAsync(chamado => chamado.Id == id);
    }

    public async Task AtualizarAsync(Chamado chamado)
    {
        _context.Chamados.Update(chamado);
        await _context.SaveChangesAsync();
    }
}