using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Entities;

public class Funil : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
}

public class FunilEstagio
{
    public int Id { get; set; }
    public int IdFunil { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public int Probabilidade { get; set; }
}

public class Oportunidade : BaseEntity
{
    public string Titulo { get; set; } = string.Empty;
    public int? IdParceiro { get; set; }
    public int? IdEstagio { get; set; }
    public decimal? Valor { get; set; }
    public DateTime? DataPrevisao { get; set; }
    public int? ResponsavelId { get; set; }
    public string? Observacao { get; set; }
}
