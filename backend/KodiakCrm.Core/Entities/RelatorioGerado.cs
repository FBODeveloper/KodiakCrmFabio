namespace KodiakCrm.Core.Entities;

public class RelatorioGerado
{
    public int Id { get; set; }
    public string IdEmpresa { get; set; } = string.Empty;
    public string CnpjEmpresa { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string? Parametros { get; set; }
    public string Resultado { get; set; } = string.Empty;
    public int? UsuarioId { get; set; }
    public string? UsuarioNome { get; set; }
    public DateTime DataGeracao { get; set; } = DateTime.UtcNow;
}
