using DeskFlow.API.Models.DTOs;
using DeskFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.API.Controllers;

[ApiController]
[Route("api/chamados")]
public class ChamadosController : ControllerBase
{
    private readonly ChamadoService _service;

    public ChamadosController(ChamadoService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Criar(CriarChamadoDto dto)
    {   
        var chamado = await _service.AdicionarAsync(dto);

        var resposta = new ChamadoRespostaDto
        {
            Id = chamado.Id,
            Titulo = chamado.Titulo,
            Descricao = chamado.Descricao,
            Prioridade = chamado.Prioridade,
            Status = chamado.Status,
            SolicitanteNome = chamado.SolicitanteNome,
            DataAbertura = chamado.DataAbertura,
            DataFechamento = chamado.DataFechamento,
            Solucao = chamado.Solucao,
            CategoriaId = chamado.CategoriaId,
            CategoriaNome = chamado.Categoria.Nome
        };

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = chamado.Id },
            resposta);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var chamado = await _service.BuscarPorIdAsync(id);

        if (chamado == null)
        {
            return NotFound();
        }

        var resposta = new ChamadoRespostaDto
        {
            Id = chamado.Id,
            Titulo = chamado.Titulo,
            Descricao = chamado.Descricao,
            Prioridade = chamado.Prioridade,
            Status = chamado.Status,
            SolicitanteNome = chamado.SolicitanteNome,
            DataAbertura = chamado.DataAbertura,
            DataFechamento = chamado.DataFechamento,
            Solucao = chamado.Solucao,
            CategoriaId = chamado.CategoriaId,
            CategoriaNome = chamado.Categoria.Nome,

            Interacoes = chamado.Interacoes
                .Select(interacao => new InteracaoRespostaDto
                {
                    Id = interacao.Id,
                    Autor = interacao.Autor,
                    Mensagem = interacao.Mensagem,
                    DataRegistro = interacao.DataRegistro
                })
                .ToList()
        };

        return Ok(resposta);
    }

    [HttpPatch("{id}/iniciar")]
    public async Task<IActionResult> Iniciar(int id)
    {
        await _service.IniciarAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/fechar")]
    public async Task<IActionResult> Fechar(int id, FecharChamadoDto dto)
    {
        await _service.FecharAsync(id, dto.Solucao);
        return NoContent();
    }
}