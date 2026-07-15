using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Entities;

public class Lead : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? Empresa { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Source { get; set; }
    public string Status { get; set; } = "novo";
    public string Temperatura { get; set; } = "frio";
    public int? IdEstagio { get; set; }
    public int? IdParceiro { get; set; }
    public string? Observacao { get; set; }
    public int? ResponsavelId { get; set; }
    public string? ResponsavelNome { get; set; }
    public string? ResponsavelAvatar { get; set; }
}
