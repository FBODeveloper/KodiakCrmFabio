using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Interfaces;

public interface IRelatorioRepository
{
    Task<RelatorioGerado?> CriarAsync(RelatorioGerado relatorio);
    Task<List<RelatorioGerado>> ObterRecentesAsync(string idEmpresa, int limite = 20);
    Task<List<OportunidadeRelatorio>> ObterOportunidadesAsync(string idEmpresa, DateTime? dataInicio, DateTime? dataFim, string? status, int? responsavelId);
    Task<List<AtividadeRelatorio>> ObterAtividadesAsync(string idEmpresa, DateTime? dataInicio, DateTime? dataFim, string? tipo, int? responsavelId);
    Task<List<LeadRelatorio>> ObterLeadsAsync(string idEmpresa, DateTime? dataInicio, DateTime? dataFim, int? responsavelId);
}

public class OportunidadeRelatorio
{
    public string Titulo { get; set; } = string.Empty;
    public decimal? Valor { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ResponsavelNome { get; set; }
    public int? ResponsavelId { get; set; }
    public string? MotivoPerda { get; set; }
    public DateTime DataCadastro { get; set; }
}

public class AtividadeRelatorio
{
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public bool Concluida { get; set; }
    public string? ResponsavelNome { get; set; }
    public int? ResponsavelId { get; set; }
    public DateTime DataCadastro { get; set; }
}

public class LeadRelatorio
{
    public string Nome { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? ResponsavelNome { get; set; }
    public int? ResponsavelId { get; set; }
    public DateTime DataCadastro { get; set; }
}
