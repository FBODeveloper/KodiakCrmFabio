using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorEmailAsync(string email, string idEmpresa);
    Task<Usuario?> ObterPorIdAsync(int id, string idEmpresa);
    Task<int> CriarAsync(Usuario usuario);
    Task<UsuarioListResult> ObterListaAsync(string idEmpresa, string? busca, string? perfil, int pagina, int itensPorPagina);
    Task AtualizarAsync(Usuario usuario);
    Task ExcluirAsync(int id, string idEmpresa);
    Task<int> ContarPorEmpresaAsync(string idEmpresa);
}

public class UsuarioListResult
{
    public List<Usuario> Itens { get; set; } = new();
    public int Total { get; set; }
}
