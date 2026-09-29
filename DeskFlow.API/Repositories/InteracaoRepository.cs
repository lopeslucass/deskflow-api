using DeskFlow.API.Data;
using DeskFlow.API.Models.Entidades;

namespace DeskFlow.API.Repositories;

public class InteracaoRepository
{
    private readonly AppDbContext _context;
    
    public InteracaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Interacao interacao)
    {
        await _context.Interacoes.AddAsync(interacao);
        await _context.SaveChangesAsync();
    }
}