namespace KodiakCrm.Core.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public string IdEmpresa { get; set; } = string.Empty;
    public string IdEstabelecimento { get; set; } = string.Empty;
    public string CnpjEmpresa { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    public bool Ativo { get; set; } = true;
}
