using DeskFlow.API.Models.Entidades;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services;

public class CategoriaService
{
    private readonly CategoriaRepository _repository;

    public CategoriaService(CategoriaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Categoria>> ListarAsync()
    {
        return await _repository.ListarAsync();
    }

    public async Task<Categoria?> BuscarPorIdAsync(int id)
    {
        return await _repository.BuscarPorIdAsync(id);
    }

        public async Task AdicionarAsync(Categoria categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria.Nome))
        {
            throw new ArgumentException("O nome da categoria é obrigatório.");
        }
        await _repository.AdicionarAsync(categoria);
    }

    public async Task AtualizarAsync(int id, Categoria categoria)
    {
        var categoriaExistente = await _repository.BuscarPorIdAsync(id);

        if (categoriaExistente == null)
        {
            throw new KeyNotFoundException("Categoria não encontrada.");
        }

        if (string.IsNullOrWhiteSpace(categoria.Nome))
        {
            throw new ArgumentException("O nome da categoria é obrigatório.");
        }

        categoriaExistente.Nome = categoria.Nome;
        await _repository.AtualizarAsync(categoriaExistente);
    }

    public async Task ExcluirAsync(int id)
    {
        var categoria = await _repository.BuscarPorIdAsync(id);
        if (categoria == null)
        {
            throw new KeyNotFoundException("Categoria não encontrada.");
        }

        var possuiChamados = await _repository.PossuiChamadosAsync(id);
        if (possuiChamados)
        {
            throw new InvalidOperationException("Não é possível excluir uma categoria que possui chamados vinculados.");
        }
        await _repository.ExcluirAsync(categoria);
    }
}