using DeskFlow.API.Models.Entidades;
using DeskFlow.API.Models.Enums;
using DeskFlow.API.Repositories;
using DeskFlow.API.Models.DTOs;
using DeskFlow.API.Exceptions;

namespace DeskFlow.API.Services;

public class ChamadoService
{
    private readonly ChamadoRepository _chamadoRepository;
    private readonly CategoriaRepository _categoriaRepository;

    public ChamadoService (ChamadoRepository chamadoRepository, CategoriaRepository categoriaRepository)
    {
        _chamadoRepository = chamadoRepository;
        _categoriaRepository = categoriaRepository;
    }

    public async Task<Chamado> AdicionarAsync(CriarChamadoDto dto)
    {
        var categoria = await _categoriaRepository.BuscarPorIdAsync(dto.CategoriaId);

        if (categoria == null)
        {
            throw new ArgumentException("A categoria informada não existe.");
        }

        if (string.IsNullOrWhiteSpace(dto.Titulo))
        {
            throw new ArgumentException("O título do chamado é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(dto.Descricao))
        {
            throw new ArgumentException("A descrição do chamado é obrigatória.");
        }

        if (string.IsNullOrWhiteSpace(dto.SolicitanteNome))
        {
            throw new ArgumentException("O nome do solicitante é obrigatório.");
        }

        var chamado = new Chamado
        {
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            Prioridade = dto.Prioridade,
            SolicitanteNome = dto.SolicitanteNome,
            CategoriaId = dto.CategoriaId,
            Categoria = categoria,
            Status = StatusChamado.Aberto,
            DataAbertura = DateTime.Now
        };

        await _chamadoRepository.AdicionarAsync(chamado);
        return chamado;
    }

    public async Task IniciarAsync(int id)
    {
        var chamado = await _chamadoRepository.BuscarPorIdAsync(id);

        if (chamado == null)
        {
            throw new KeyNotFoundException("Chamado não encontrado.");
        }

        if (chamado.Status != StatusChamado.Aberto)
        {
            throw new BusinessConflictException("Somente chamados abertos podem ser iniciados.");
        }

        chamado.Status = StatusChamado.EmAndamento;
        await _chamadoRepository.AtualizarAsync(chamado);
    }

    public async Task FecharAsync(int id, string solucao)
    {
        var chamado = await _chamadoRepository.BuscarPorIdAsync(id);

        if (chamado == null)
        {
            throw new KeyNotFoundException("Chamado não encontrado.");
        }

        if (chamado.Status == StatusChamado.Fechado)
        {
            throw new BusinessConflictException("O chamado já está fechado.");
        }

        if (string.IsNullOrWhiteSpace(solucao))
        {
            throw new ArgumentException("A solução é obrigatória para fechar o chamado.");
        }

        chamado.Status = StatusChamado.Fechado;
        chamado.Solucao = solucao;
        chamado.DataFechamento = DateTime.Now;

        await _chamadoRepository.AtualizarAsync(chamado);
    }

    public async Task<Chamado?> BuscarPorIdAsync(int id)
    {
        return await _chamadoRepository.BuscarPorIdAsync(id);
    }

    public async Task<List<Chamado>> ListarAsync(StatusChamado? status, Prioridade? prioridade,int? categoriaId)
    {
        return await _chamadoRepository.ListarAsync(status, prioridade, categoriaId);
    }
}