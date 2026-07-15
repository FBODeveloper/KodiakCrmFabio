using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.UseCases.Services;

public class FunilService
{
    private readonly IFunilRepository _repository;

    public FunilService(IFunilRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<FunilDTO>> ObterListaAsync(string idEmpresa)
    {
        var funis = await _repository.ObterListaAsync(idEmpresa);
        var resultado = new List<FunilDTO>();

        foreach (var funil in funis)
        {
            var estagios = await _repository.ObterEstagiosAsync(funil.Id);
            resultado.Add(new FunilDTO
            {
                Id = funil.Id,
                Nome = funil.Nome,
                Ativo = funil.Ativo,
                Estagios = estagios.Select(e => new FunilEstagioDTO
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    Ordem = e.Ordem,
                    Probabilidade = e.Probabilidade
                }).ToList()
            });
        }

        return resultado;
    }

    public async Task<FunilDTO?> ObterPorIdAsync(int id, string idEmpresa)
    {
        var funil = await _repository.ObterPorIdAsync(id, idEmpresa);
        if (funil == null) return null;

        var estagios = await _repository.ObterEstagiosAsync(funil.Id);
        return new FunilDTO
        {
            Id = funil.Id,
            Nome = funil.Nome,
            Ativo = funil.Ativo,
            Estagios = estagios.Select(e => new FunilEstagioDTO
            {
                Id = e.Id,
                Nome = e.Nome,
                Ordem = e.Ordem,
                Probabilidade = e.Probabilidade
            }).ToList()
        };
    }

    public async Task<FunilDTO> CriarAsync(FunilCreateDTO dto, string idEmpresa)
    {
        var funil = new Funil
        {
            IdEmpresa = idEmpresa,
            Nome = dto.Nome
        };

        var id = await _repository.CriarAsync(funil);
        funil.Id = id;

        var estagios = new List<FunilEstagioDTO>();
        foreach (var estagioDto in dto.Estagios)
        {
            var estagio = new FunilEstagio
            {
                IdFunil = id,
                Nome = estagioDto.Nome,
                Ordem = estagioDto.Ordem,
                Probabilidade = estagioDto.Probabilidade
            };

            var estagioId = await _repository.CriarEstagioAsync(estagio);
            estagios.Add(new FunilEstagioDTO
            {
                Id = estagioId,
                Nome = estagio.Nome,
                Ordem = estagio.Ordem,
                Probabilidade = estagio.Probabilidade
            });
        }

        return new FunilDTO
        {
            Id = funil.Id,
            Nome = funil.Nome,
            Ativo = true,
            Estagios = estagios
        };
    }

    public async Task ExcluirAsync(int id, string idEmpresa)
    {
        await _repository.ExcluirAsync(id, idEmpresa);
    }
}

public class OportunidadeService
{
    private readonly IOportunidadeRepository _repository;
    private readonly IFunilRepository _funilRepository;

    public OportunidadeService(IOportunidadeRepository repository, IFunilRepository funilRepository)
    {
        _repository = repository;
        _funilRepository = funilRepository;
    }

    public async Task<OportunidadeDTO?> ObterPorIdAsync(int id, string idEmpresa)
    {
        var oportunidade = await _repository.ObterPorIdAsync(id, idEmpresa);
        if (oportunidade == null) return null;
        return MapearParaDTO(oportunidade);
    }

    public async Task<OportunidadeListDTO> ObterListaAsync(string idEmpresa, string? busca, int? idEstagio, int? responsavelId, int pagina, int itensPorPagina)
    {
        var resultado = await _repository.ObterListaAsync(idEmpresa, busca, idEstagio, responsavelId, pagina, itensPorPagina);
        return new OportunidadeListDTO
        {
            Itens = resultado.Itens.Select(MapearParaDTO).ToList(),
            Total = resultado.Total,
            Pagina = pagina,
            ItensPorPagina = itensPorPagina
        };
    }

    public async Task<OportunidadeDTO> CriarAsync(OportunidadeCreateDTO dto, string idEmpresa, string idEstabelecimento, string cnpjEmpresa)
    {
        var oportunidade = new Oportunidade
        {
            IdEmpresa = idEmpresa,
            IdEstabelecimento = idEstabelecimento,
            CnpjEmpresa = cnpjEmpresa,
            Titulo = dto.Titulo,
            IdParceiro = dto.IdParceiro,
            IdEstagio = dto.IdEstagio,
            Valor = dto.Valor,
            DataPrevisao = dto.DataPrevisao,
            Observacao = dto.Observacao
        };

        var id = await _repository.CriarAsync(oportunidade);
        oportunidade.Id = id;

        return MapearParaDTO(oportunidade);
    }

    public async Task<OportunidadeDTO?> AtualizarAsync(int id, OportunidadeUpdateDTO dto, string idEmpresa)
    {
        var oportunidade = await _repository.ObterPorIdAsync(id, idEmpresa);
        if (oportunidade == null) return null;

        oportunidade.Titulo = dto.Titulo;
        oportunidade.IdParceiro = dto.IdParceiro;
        oportunidade.IdEstagio = dto.IdEstagio;
        oportunidade.Valor = dto.Valor;
        oportunidade.DataPrevisao = dto.DataPrevisao;
        oportunidade.ResponsavelId = dto.ResponsavelId;
        oportunidade.Observacao = dto.Observacao;

        await _repository.AtualizarAsync(oportunidade);

        return MapearParaDTO(oportunidade);
    }

    public async Task<OportunidadeDTO?> MoverAsync(int id, OportunidadeMoverDTO dto, string idEmpresa)
    {
        var oportunidade = await _repository.ObterPorIdAsync(id, idEmpresa);
        if (oportunidade == null) return null;

        oportunidade.IdEstagio = dto.IdEstagio;
        await _repository.AtualizarAsync(oportunidade);

        return MapearParaDTO(oportunidade);
    }

    public async Task<KanbanDTO> ObterKanbanAsync(int funilId, string idEmpresa)
    {
        var funil = await _funilRepository.ObterPorIdAsync(funilId, idEmpresa);
        if (funil == null) throw new InvalidOperationException("Funil não encontrado");

        var estagios = await _funilRepository.ObterEstagiosAsync(funilId);

        var kanban = new KanbanDTO
        {
            FunilId = funil.Id,
            FunilNome = funil.Nome,
            Colunas = new List<KanbanColunaDTO>()
        };

        foreach (var estagio in estagios)
        {
            var oportunidades = await _repository.ObterPorEstagioAsync(estagio.Id, idEmpresa);
            kanban.Colunas.Add(new KanbanColunaDTO
            {
                EstagioId = estagio.Id,
                EstagioNome = estagio.Nome,
                Ordem = estagio.Ordem,
                Oportunidades = oportunidades.Select(MapearParaDTO).ToList()
            });
        }

        return kanban;
    }

    private static OportunidadeDTO MapearParaDTO(Oportunidade oportunidade)
    {
        return new OportunidadeDTO
        {
            Id = oportunidade.Id,
            IdEmpresa = oportunidade.IdEmpresa,
            Titulo = oportunidade.Titulo,
            IdParceiro = oportunidade.IdParceiro,
            IdEstagio = oportunidade.IdEstagio,
            Valor = oportunidade.Valor,
            DataPrevisao = oportunidade.DataPrevisao,
            ResponsavelId = oportunidade.ResponsavelId,
            Observacao = oportunidade.Observacao,
            Ativo = oportunidade.Ativo,
            DataCadastro = oportunidade.DataCadastro
        };
    }
}
