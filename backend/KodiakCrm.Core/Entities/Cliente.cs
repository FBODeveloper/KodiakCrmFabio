using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Entities;

public class Cliente : BaseEntity
{
    public string RazaoSocial { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string? CnpjCpf { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public string? Endereco { get; set; }
    public string? Observacao { get; set; }
    public string? Origem { get; set; }
    public DateTime? DataConversao { get; set; }
    public int? IdOportunidade { get; set; }
    public int? ResponsavelId { get; set; }
    public string? ResponsavelNome { get; set; }
}
