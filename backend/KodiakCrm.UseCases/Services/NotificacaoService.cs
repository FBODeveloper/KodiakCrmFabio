using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.UseCases.Services;

public class NotificacaoService
{
    private readonly INotificacaoRepository _repository;

    public NotificacaoService(INotificacaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<NotificacaoResumoDTO> ObterResumoAsync(string idEmpresa, int usuarioId)
    {
        var naoLidas = await _repository.ContarNaoLidasAsync(idEmpresa, usuarioId);
        var recentes = await _repository.ObterPorUsuarioAsync(idEmpresa, usuarioId, 10);

        return new NotificacaoResumoDTO
        {
            TotalNaoLidas = naoLidas,
            Recentes = recentes.Select(MapearParaDTO).ToList()
        };
    }

    public async Task<List<NotificacaoDTO>> ObterPorUsuarioAsync(string idEmpresa, int usuarioId, int limite = 50)
    {
        var notificacoes = await _repository.ObterPorUsuarioAsync(idEmpresa, usuarioId, limite);
        return notificacoes.Select(MapearParaDTO).ToList();
    }

    public async Task CriarAsync(string idEmpresa, NotificacaoCriarDTO dto)
    {
        var notificacao = new Notificacao
        {
            IdEmpresa = idEmpresa,
            UsuarioId = dto.UsuarioId,
            Titulo = dto.Titulo,
            Mensagem = dto.Mensagem,
            Tipo = dto.Tipo,
            Entidade = dto.Entidade,
            EntidadeId = dto.EntidadeId
        };

        await _repository.CriarAsync(notificacao);
    }

    public async Task CriarParaTodosAsync(string idEmpresa, List<int> usuarioIds, string titulo, string mensagem, string tipo, string? entidade = null, int? entidadeId = null)
    {
        foreach (var usuarioId in usuarioIds)
        {
            await CriarAsync(idEmpresa, new NotificacaoCriarDTO
            {
                UsuarioId = usuarioId,
                Titulo = titulo,
                Mensagem = mensagem,
                Tipo = tipo,
                Entidade = entidade,
                EntidadeId = entidadeId
            });
        }
    }

    public async Task MarcarLidaAsync(int id, string idEmpresa)
    {
        await _repository.MarcarLidaAsync(id, idEmpresa);
    }

    public async Task MarcarTodasLidasAsync(string idEmpresa, int usuarioId)
    {
        await _repository.MarcarTodasLidasAsync(idEmpresa, usuarioId);
    }

    public async Task ExcluirAsync(int id, string idEmpresa)
    {
        await _repository.ExcluirAsync(id, idEmpresa);
    }

    private static NotificacaoDTO MapearParaDTO(Notificacao n)
    {
        return new NotificacaoDTO
        {
            Id = n.Id,
            Titulo = n.Titulo,
            Mensagem = n.Mensagem,
            Tipo = n.Tipo,
            Entidade = n.Entidade,
            EntidadeId = n.EntidadeId,
            Lida = n.Lida,
            DataCriacao = n.DataCriacao
        };
    }
}
