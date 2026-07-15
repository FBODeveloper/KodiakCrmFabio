using System.Text.Json;
using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.UseCases.Services;

public class HistoricoService
{
    private readonly IHistoricoRepository _repository;

    public HistoricoService(IHistoricoRepository repository)
    {
        _repository = repository;
    }

    public async Task RegistrarAsync(string idEmpresa, string? idEstabelecimento, string? cnpjEmpresa,
        string entidade, int entidadeId, string acao, string descricao,
        object? dadosAntes = null, object? dadosDepois = null,
        int? usuarioId = null, string? usuarioNome = null)
    {
        var historico = new Historico
        {
            IdEmpresa = idEmpresa,
            IdEstabelecimento = idEstabelecimento,
            CnpjEmpresa = cnpjEmpresa,
            Entidade = entidade,
            EntidadeId = entidadeId,
            Acao = acao,
            Descricao = descricao,
            DadosAntes = dadosAntes != null ? JsonSerializer.Serialize(dadosAntes) : null,
            DadosDepois = dadosDepois != null ? JsonSerializer.Serialize(dadosDepois) : null,
            UsuarioId = usuarioId,
            UsuarioNome = usuarioNome,
            DataAcao = DateTime.UtcNow
        };

        await _repository.CriarAsync(historico);
    }

    public async Task<List<HistoricoDTO>> ObterPorEntidadeAsync(string entidade, int entidadeId, string idEmpresa, int limite = 50)
    {
        var itens = await _repository.ObterPorEntidadeAsync(entidade, entidadeId, idEmpresa, limite);
        return itens.Select(MapearParaDTO).ToList();
    }

    public async Task<List<HistoricoDTO>> ObterRecentesAsync(string idEmpresa, int limite = 20)
    {
        var itens = await _repository.ObterRecentesAsync(idEmpresa, limite);
        return itens.Select(MapearParaDTO).ToList();
    }

    private static HistoricoDTO MapearParaDTO(Historico h)
    {
        return new HistoricoDTO
        {
            Id = h.Id,
            Entidade = h.Entidade,
            EntidadeId = h.EntidadeId,
            Acao = h.Acao,
            Descricao = h.Descricao,
            DadosAntes = h.DadosAntes,
            DadosDepois = h.DadosDepois,
            UsuarioId = h.UsuarioId,
            UsuarioNome = h.UsuarioNome,
            DataAcao = h.DataAcao
        };
    }
}
