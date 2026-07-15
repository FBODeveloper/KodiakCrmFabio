using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Interfaces;

public interface IParceiroRepository
{
    Task<Parceiro?> ObterPorIdAsync(int id, string idEmpresa);
    Task<Parceiro?> ObterPorCpfCnpjAsync(string cpfCnpj, string idEmpresa);
    Task<Parceiro?> ObterPorKodiakErpAsync(int idParceiroKodiakErp, string idEmpresa);
    Task<ParceiroListResult> ObterListaAsync(string idEmpresa, string? busca, int pagina, int itensPorPagina);
    Task<int> CriarAsync(Parceiro parceiro);
    Task AtualizarAsync(Parceiro parceiro);
    Task AtualizarKodiakErpAsync(int id, int idParceiroKodiakErp);
    Task<bool> ExisteCpfCnpjAsync(string cpfCnpj, string idEmpresa, int? idExcluir = null);
}

public class ParceiroListResult
{
    public List<Parceiro> Itens { get; set; } = new();
    public int Total { get; set; }
}
