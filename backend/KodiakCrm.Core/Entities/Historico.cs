namespace KodiakCrm.Core.Entities;

public class Historico
{
    public int Id { get; set; }
    public string IdEmpresa { get; set; } = string.Empty;
    public string? IdEstabelecimento { get; set; }
    public string? CnpjEmpresa { get; set; }
    public string Entidade { get; set; } = string.Empty;
    public int EntidadeId { get; set; }
    public string Acao { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string? DadosAntes { get; set; }
    public string? DadosDepois { get; set; }
    public int? UsuarioId { get; set; }
    public string? UsuarioNome { get; set; }
    public DateTime DataAcao { get; set; }
}
