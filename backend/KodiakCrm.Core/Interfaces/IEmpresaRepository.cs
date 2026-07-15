using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Interfaces;

public interface IEmpresaRepository
{
    Task<Empresa?> ObterPorCnpjAsync(string cnpj);
    Task<EmpresaListResult> ObterListaAsync(string? busca, int pagina, int itensPorPagina);
    Task CriarAsync(Empresa empresa);
    Task AtualizarAsync(Empresa empresa);
    Task<int> ContarUsuariosAsync(string cnpj);
}

public class EmpresaListResult
{
    public List<Empresa> Itens { get; set; } = new();
    public int Total { get; set; }
}
