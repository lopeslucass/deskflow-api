using DeskFlow.API.Models.DTOs;
using DeskFlow.API.Models.Entidades;
using DeskFlow.API.Models.Enums;
using DeskFlow.API.Repositories;
using DeskFlow.API.Exceptions;

namespace DeskFlow.API.Services;

public class InteracaoService
{
    private readonly InteracaoRepository _interacaoRepository;
    private readonly ChamadoRepository _chamadoRepository;

    public InteracaoService(InteracaoRepository interacaoRepository, ChamadoRepository chamadoRepository)
    {
        _interacaoRepository = interacaoRepository;
        _chamadoRepository = chamadoRepository;
    }

    public async Task<Interacao> AdicionarAsync(int chamadoId, CriarInteracaoDto dto)
    {
        var chamado = await _chamadoRepository.BuscarPorIdAsync(chamadoId);

        if (chamado == null)
        {
            throw new KeyNotFoundException("Chamado não encontrado.");
        }

        if (chamado.Status == StatusChamado.Fechado)
        {
            throw new BusinessConflictException("Não é possível adicionar interações a um chamado fechado.");
        }

        if (string.IsNullOrWhiteSpace(dto.Autor))
        {
            throw new ArgumentException("O autor da interação é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(dto.Mensagem))
        {
            throw new ArgumentException("A mensagem da interação é obrigatória.");
        }

        var interacao = new Interacao
        {
            ChamadoId = chamadoId,
            Autor = dto.Autor,
            Mensagem = dto.Mensagem,
            DataRegistro = DateTime.Now
        };

        await _interacaoRepository.AdicionarAsync(interacao);
        return interacao;
    }
}