using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Interfaces;

public interface IHistoricoRepository
{
    Task<int> CriarAsync(Historico historico);
    Task<List<Historico>> ObterPorEntidadeAsync(string entidade, int entidadeId, string idEmpresa, int limite = 50);
    Task<List<Historico>> ObterRecentesAsync(string idEmpresa, int limite = 20);
}

public class HistoricoListResult
{
    public List<Historico> Itens { get; set; } = new();
    public int Total { get; set; }
}
