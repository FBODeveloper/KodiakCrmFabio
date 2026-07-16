using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Interfaces;

public interface IPropostaRepository
{
    Task<Proposta?> ObterPorIdAsync(int id, string idEmpresa);
    Task<PropostaListResult> ObterListaAsync(string idEmpresa, string? busca, string? status, int? idParceiro, int? clienteId, DateTime? dataInicio, DateTime? dataFim, int pagina, int itensPorPagina);
    Task<List<PropostaItem>> ObterItensAsync(int idProposta);
    Task<int> CriarAsync(Proposta proposta);
    Task AtualizarAsync(Proposta proposta);
    Task ExcluirAsync(int id, string idEmpresa);
    Task ExcluirItensAsync(int idProposta);
    Task<int> CriarItemAsync(PropostaItem item);
    Task<string> GerarProximoNumeroAsync(string idEmpresa);
}

public class PropostaListResult
{
    public List<Proposta> Itens { get; set; } = new();
    public int Total { get; set; }
}
