using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Entities;

public class Parceiro : BaseEntity
{
    public string RazaoSocial { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string? CpfCnpj { get; set; }
    public string? TipoPessoa { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public int? IdParceiroKodiakErp { get; set; }
    public string? Observacao { get; set; }
}

public class ParceiroEndereco : BaseEntity
{
    public int IdParceiro { get; set; }
    public string? TipoEndereco { get; set; }
    public string? Logradouro { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public int? IdMunicipio { get; set; }
    public int? IdEstado { get; set; }
    public int? IdPais { get; set; }
    public string? Cep { get; set; }
    public bool Principal { get; set; }
}
