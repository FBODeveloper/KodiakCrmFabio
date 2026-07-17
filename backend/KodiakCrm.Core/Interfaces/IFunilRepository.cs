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
    Task ExcluirEstagioAsync(int idEstagio);
    Task ExcluirAsync(int id, string idEmpresa);
}

public class OportunidadeListResult
{
    public List<Oportunidade> Itens { get; set; } = new();
    public int Total { get; set; }
}
