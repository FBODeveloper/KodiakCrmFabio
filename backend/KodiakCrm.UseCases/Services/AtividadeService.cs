using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.UseCases.Services;

public class AtividadeService
{
    private readonly IAtividadeRepository _repository;

    public AtividadeService(IAtividadeRepository repository)
    {
        _repository = repository;
    }

    public async Task<AtividadeDTO?> ObterPorIdAsync(int id, string idEmpresa)
    {
        var atividade = await _repository.ObterPorIdAsync(id, idEmpresa);
        if (atividade == null) return null;
        return MapearParaDTO(atividade);
    }

    public async Task<AtividadeListDTO> ObterListaAsync(string idEmpresa, string? busca, string? tipo, int? idParceiro, int? idOportunidade, int? responsavelId, bool? concluida, int pagina, int itensPorPagina)
    {
        var resultado = await _repository.ObterListaAsync(idEmpresa, busca, tipo, idParceiro, idOportunidade, responsavelId, concluida, pagina, itensPorPagina);
        return new AtividadeListDTO
        {
            Itens = resultado.Itens.Select(MapearParaDTO).ToList(),
            Total = resultado.Total,
            Pagina = pagina,
            ItensPorPagina = itensPorPagina
        };
    }

    public async Task<List<AtividadeDTO>> ObterPorParceiroAsync(int idParceiro, string idEmpresa)
    {
        var atividades = await _repository.ObterPorParceiroAsync(idParceiro, idEmpresa);
        return atividades.Select(MapearParaDTO).ToList();
    }

    public async Task<AtividadeDTO> CriarAsync(AtividadeCreateDTO dto, string idEmpresa, string idEstabelecimento, string cnpjEmpresa, int? responsavelId)
    {
        var atividade = new Atividade
        {
            IdEmpresa = idEmpresa,
            IdEstabelecimento = idEstabelecimento,
            CnpjEmpresa = cnpjEmpresa,
            Tipo = dto.Tipo,
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            IdParceiro = dto.IdParceiro,
            IdOportunidade = dto.IdOportunidade,
            ResponsavelId = responsavelId,
            DataInicio = dto.DataInicio,
            DataFim = dto.DataFim
        };

        var id = await _repository.CriarAsync(atividade);
        atividade.Id = id;

        return MapearParaDTO(atividade);
    }

    public async Task<AtividadeDTO?> AtualizarAsync(int id, AtividadeUpdateDTO dto, string idEmpresa)
    {
        var atividade = await _repository.ObterPorIdAsync(id, idEmpresa);
        if (atividade == null) return null;

        atividade.Tipo = dto.Tipo;
        atividade.Titulo = dto.Titulo;
        atividade.Descricao = dto.Descricao;
        atividade.IdParceiro = dto.IdParceiro;
        atividade.IdOportunidade = dto.IdOportunidade;
        atividade.ResponsavelId = dto.ResponsavelId;
        atividade.DataInicio = dto.DataInicio;
        atividade.DataFim = dto.DataFim;
        if (dto.Concluida.HasValue)
            atividade.Concluida = dto.Concluida.Value;

        await _repository.AtualizarAsync(atividade);

        return MapearParaDTO(atividade);
    }

    private static AtividadeDTO MapearParaDTO(Atividade atividade)
    {
        return new AtividadeDTO
        {
            Id = atividade.Id,
            IdEmpresa = atividade.IdEmpresa,
            Tipo = atividade.Tipo,
            Titulo = atividade.Titulo,
            Descricao = atividade.Descricao,
            IdParceiro = atividade.IdParceiro,
            IdOportunidade = atividade.IdOportunidade,
            ResponsavelId = atividade.ResponsavelId,
            DataInicio = atividade.DataInicio,
            DataFim = atividade.DataFim,
            Concluida = atividade.Concluida,
            Ativo = atividade.Ativo,
            DataCadastro = atividade.DataCadastro
        };
    }
}
