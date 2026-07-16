using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Interfaces;

public interface INotificacaoRepository
{
    Task<List<Notificacao>> ObterPorUsuarioAsync(string idEmpresa, int usuarioId, int limite = 50);
    Task<int> ContarNaoLidasAsync(string idEmpresa, int usuarioId);
    Task CriarAsync(Notificacao notificacao);
    Task MarcarLidaAsync(int id, string idEmpresa);
    Task MarcarTodasLidasAsync(string idEmpresa, int usuarioId);
    Task ExcluirAsync(int id, string idEmpresa);
}
