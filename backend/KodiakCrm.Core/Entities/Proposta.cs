using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Entities;

public class Proposta : BaseEntity
{
    public string Titulo { get; set; } = string.Empty;
    public int? IdParceiro { get; set; }
    public int? IdOportunidade { get; set; }
    public decimal? ValorTotal { get; set; }
    public DateTime? DataValidade { get; set; }
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
