using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Interfaces;

public interface IClienteRepository
{
    Task<Cliente?> ObterPorIdAsync(int id, string idEmpresa);
    Task<ClienteListResult> ObterListaAsync(string idEmpresa, string? busca, int pagina, int itensPorPagina);
    Task<int> CriarAsync(Cliente cliente);
    Task AtualizarAsync(Cliente cliente);
    Task ExcluirAsync(int id, string idEmpresa);
}

public class ClienteListResult
{
    public List<Cliente> Itens { get; set; } = new();
    public int Total { get; set; }
}
