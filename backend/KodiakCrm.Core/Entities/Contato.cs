using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Entities;

public class Contato : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? Cargo { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public int? IdCliente { get; set; }
    public int? IdParceiro { get; set; }
    public string? Observacao { get; set; }
    public int? ResponsavelId { get; set; }
    public string? ResponsavelNome { get; set; }
    public string? ClienteNome { get; set; }
    public string? ParceiroNome { get; set; }
}
