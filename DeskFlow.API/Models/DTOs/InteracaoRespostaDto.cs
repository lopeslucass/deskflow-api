namespace DeskFlow.API.Models.DTOs;

public class InteracaoRespostaDto
{
    public int Id { get; set; }
    public string Autor { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public DateTime DataRegistro { get; set; }
}