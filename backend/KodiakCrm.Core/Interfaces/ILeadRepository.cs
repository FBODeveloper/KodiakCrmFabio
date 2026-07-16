using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Interfaces;

public interface ILeadRepository
{
    Task<Lead?> ObterPorIdAsync(int id, string idEmpresa);
    Task<LeadListResult> ObterListaAsync(string idEmpresa, string? busca, string? status, string? temperatura, DateTime? dataInicio, DateTime? dataFim, int pagina, int itensPorPagina);
    Task<int> CriarAsync(Lead lead);
    Task AtualizarAsync(Lead lead);
    Task ExcluirAsync(int id, string idEmpresa);
    Task<bool> ExisteEmailAsync(string email, string idEmpresa, int? idExcluir = null);

    Task<List<LeadEstagio>> ObterEstagiosAsync(string idEmpresa);
    Task<int> CriarEstagioAsync(LeadEstagio estagio);
    Task AtualizarEstagioAsync(LeadEstagio estagio);
    Task ExcluirEstagioAsync(int id, string idEmpresa);
    Task<List<Lead>> ObterPorEstagioAsync(int idEstagio, string idEmpresa);
    Task<LeadStatsDTO> ObterStatsAsync(string idEmpresa);
}

public class LeadListResult
{
    public List<Lead> Itens { get; set; } = new();
    public int Total { get; set; }
}

public interface IOportunidadeRepository
{
    Task<Oportunidade?> ObterPorIdAsync(int id, string idEmpresa);
    Task<OportunidadeListResult> ObterListaAsync(string idEmpresa, string? busca, int? idEstagio, int? responsavelId, string? status, DateTime? dataInicio, DateTime? dataFim, int pagina, int itensPorPagina);
    Task<List<Oportunidade>> ObterPorEstagioAsync(int idEstagio, string idEmpresa);
    Task<int> CriarAsync(Oportunidade oportunidade);
    Task AtualizarAsync(Oportunidade oportunidade);
}
