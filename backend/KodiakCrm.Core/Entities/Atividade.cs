using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Entities;

public class Atividade : BaseEntity
{
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public int? IdParceiro { get; set; }
    public int? IdOportunidade { get; set; }
    public int? ResponsavelId { get; set; }
    public int? ClienteId { get; set; }
    public string Status { get; set; } = "pendente";
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public bool Concluida { get; set; }
}
