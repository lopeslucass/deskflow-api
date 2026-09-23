using DeskFlow.API.Models.Enums;

namespace DeskFlow.API.Models.Entidades;

public class Chamado
{
    public int Id {get; set;}
    public int CategoriaId {get; set;}
    public Categoria Categoria {get; set;} = null!;

    public string Titulo {get; set;} = string.Empty;
    public string Descricao {get; set;} = string.Empty;
    public string SolicitanteNome {get; set;} = string.Empty;

    public Prioridade Prioridade {get; set;}
    public StatusChamado Status {get; set;}
    public DateTime DataAbertura {get; set;}
    public DateTime? DataFechamento {get; set;}

    public string? Solucao {get; set;}

    public ICollection<Interacao> Interacoes {get; set;} = new List<Interacao>();

}