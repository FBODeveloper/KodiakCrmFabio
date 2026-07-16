using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Interfaces;

public interface IContatoRepository
{
    Task<Contato?> ObterPorIdAsync(int id, string idEmpresa);
    Task<ContatoListResult> ObterListaAsync(string idEmpresa, string? busca, int? idCliente, int? idParceiro, int pagina, int itensPorPagina);
    Task<List<Contato>> ObterPorClienteAsync(int idCliente, string idEmpresa);
    Task<List<Contato>> ObterPorParceiroAsync(int idParceiro, string idEmpresa);
    Task<int> CriarAsync(Contato contato);
    Task AtualizarAsync(Contato contato);
    Task ExcluirAsync(int id, string idEmpresa);
}

public class ContatoListResult
{
    public List<Contato> Itens { get; set; } = new();
    public int Total { get; set; }
}
