using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Entities;

public class Proposta : BaseEntity
{
    public string Titulo { get; set; } = string.Empty;
    public string? Numero { get; set; }
    public DateOnly? DataProposta { get; set; }
    public string? FormaPagamento { get; set; }
    public string? PrazoEntrega { get; set; }
    public int? IdParceiro { get; set; }
    public string? ParceiroNome { get; set; }
    public int? IdOportunidade { get; set; }
    public string? OportunidadeTitulo { get; set; }
    public int? ClienteId { get; set; }
    public string? ClienteNome { get; set; }
    public int? ContatoId { get; set; }
    public string? ContatoNome { get; set; }
    public decimal? ValorTotal { get; set; }
    public DateOnly? DataValidade { get; set; }
    public string Status { get; set; } = "rascunho";
    public string? Observacao { get; set; }
}

public class PropostaItem
{
    public int Id { get; set; }
    public int IdProposta { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
}
