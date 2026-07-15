using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Interfaces;

public interface IFunilRepository
{
    Task<Funil?> ObterPorIdAsync(int id, string idEmpresa);
    Task<List<Funil>> ObterListaAsync(string idEmpresa);
    Task<List<FunilEstagio>> ObterEstagiosAsync(int idFunil);
    Task<int> CriarAsync(Funil funil);
    Task<int> CriarEstagioAsync(FunilEstagio estagio);
    Task AtualizarAsync(Funil funil);
    Task ExcluirAsync(int id, string idEmpresa);
}

public interface IOportunidadeRepository
{
    Task<Oportunidade?> ObterPorIdAsync(int id, string idEmpresa);
    Task<OportunidadeListResult> ObterListaAsync(string idEmpresa, string? busca, int? idEstagio, int? responsavelId, int pagina, int itensPorPagina);
    Task<List<Oportunidade>> ObterPorEstagioAsync(int idEstagio, string idEmpresa);
    Task<int> CriarAsync(Oportunidade oportunidade);
    Task AtualizarAsync(Oportunidade oportunidade);
}

public class OportunidadeListResult
{
    public List<Oportunidade> Itens { get; set; } = new();
    public int Total { get; set; }
}
