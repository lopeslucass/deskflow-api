using DeskFlow.API.Services;
using Microsoft.AspNetCore.Mvc;
using DeskFlow.API.Models.Entidades;

namespace DeskFlow.API.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly CategoriaService _service;
    public CategoriasController(CategoriaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var categorias = await _service.ListarAsync();
        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var categoria = await _service.BuscarPorIdAsync(id);
        if (categoria == null)
        {
            return NotFound();
        }
        return Ok(categoria);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(Categoria categoria)
    {
        await _service.AdicionarAsync(categoria);
        return CreatedAtAction(nameof(BuscarPorId), new {id = categoria.Id}, categoria);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, Categoria categoria)
    {
        await _service.AtualizarAsync(id, categoria);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(int id)
    {
        await _service.ExcluirAsync(id);
        return NoContent();
    }
}