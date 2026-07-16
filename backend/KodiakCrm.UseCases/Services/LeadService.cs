using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.UseCases.Services;

public class LeadService
{
    private readonly ILeadRepository _leadRepository;
    private readonly IOportunidadeRepository _oportunidadeRepository;
    private readonly IFunilRepository _funilRepository;

    public LeadService(ILeadRepository leadRepository, IOportunidadeRepository oportunidadeRepository, IFunilRepository funilRepository)
    {
        _leadRepository = leadRepository;
        _oportunidadeRepository = oportunidadeRepository;
        _funilRepository = funilRepository;
    }

    public async Task<LeadDTO?> ObterPorIdAsync(int id, string idEmpresa)
    {
        var lead = await _leadRepository.ObterPorIdAsync(id, idEmpresa);
        if (lead == null) return null;

        string? estagioNome = null;
        if (lead.IdEstagio.HasValue)
        {
            var estagios = await _leadRepository.ObterEstagiosAsync(idEmpresa);
            estagioNome = estagios.FirstOrDefault(e => e.Id == lead.IdEstagio.Value)?.Nome;
        }

        var dto = MapearParaDTO(lead);
        dto.EstagioNome = estagioNome;
        return dto;
    }

    public async Task<LeadListDTO> ObterListaAsync(string idEmpresa, string? busca, string? status, string? temperatura, DateTime? dataInicio, DateTime? dataFim, int pagina, int itensPorPagina)
    {
        var resultado = await _leadRepository.ObterListaAsync(idEmpresa, busca, status, temperatura, dataInicio, dataFim, pagina, itensPorPagina);
        return new LeadListDTO
        {
            Itens = resultado.Itens.Select(MapearParaDTO).ToList(),
            Total = resultado.Total,
            Pagina = pagina,
            ItensPorPagina = itensPorPagina
        };
    }

    public async Task<LeadDTO> CriarAsync(LeadCreateDTO dto, string idEmpresa, string idEstabelecimento, string cnpjEmpresa)
    {
        var lead = new Lead
        {
            IdEmpresa = idEmpresa,
            IdEstabelecimento = idEstabelecimento,
            CnpjEmpresa = cnpjEmpresa,
            Nome = dto.Nome,
            Empresa = dto.Empresa,
            Email = dto.Email,
            Telefone = dto.Telefone,
            Source = dto.Source,
            Temperatura = dto.Temperatura ?? "frio",
            IdEstagio = dto.IdEstagio,
            ResponsavelId = dto.ResponsavelId,
            Observacao = dto.Observacao
        };

        var id = await _leadRepository.CriarAsync(lead);
        lead.Id = id;

        return MapearParaDTO(lead);
    }

    public async Task<LeadDTO?> AtualizarAsync(int id, LeadUpdateDTO dto, string idEmpresa)
    {
        var lead = await _leadRepository.ObterPorIdAsync(id, idEmpresa);
        if (lead == null) return null;

        lead.Nome = dto.Nome;
        lead.Empresa = dto.Empresa;
        lead.Email = dto.Email;
        lead.Telefone = dto.Telefone;
        lead.Source = dto.Source;
        lead.Status = dto.Status ?? lead.Status;
        lead.Temperatura = dto.Temperatura ?? lead.Temperatura;
        lead.IdEstagio = dto.IdEstagio ?? lead.IdEstagio;
        lead.ResponsavelId = dto.ResponsavelId;
        lead.Observacao = dto.Observacao;

        await _leadRepository.AtualizarAsync(lead);

        return MapearParaDTO(lead);
    }

    public async Task<bool> ExcluirAsync(int id, string idEmpresa)
    {
        var lead = await _leadRepository.ObterPorIdAsync(id, idEmpresa);
        if (lead == null) return false;

        await _leadRepository.ExcluirAsync(id, idEmpresa);
        return true;
    }

    public async Task<LeadDTO?> MoverAsync(int id, LeadMoverDTO dto, string idEmpresa)
    {
        var lead = await _leadRepository.ObterPorIdAsync(id, idEmpresa);
        if (lead == null) return null;

        lead.IdEstagio = dto.IdEstagio;
        await _leadRepository.AtualizarAsync(lead);

        return MapearParaDTO(lead);
    }

    public async Task<LeadConverterResponseDTO?> ConverterAsync(int id, LeadConverterDTO dto, string idEmpresa, string idEstabelecimento, string cnpjEmpresa)
    {
        var lead = await _leadRepository.ObterPorIdAsync(id, idEmpresa);
        if (lead == null || lead.Status == "convertido") return null;

        var titulo = !string.IsNullOrWhiteSpace(lead.Empresa)
            ? $"{lead.Empresa} - {lead.Nome}"
            : lead.Nome;

        var oportunidade = new Oportunidade
        {
            IdEmpresa = idEmpresa,
            IdEstabelecimento = idEstabelecimento,
            CnpjEmpresa = cnpjEmpresa,
            Titulo = titulo,
            IdParceiro = lead.IdParceiro,
            IdEstagio = dto.IdEstagio,
            Valor = dto.Valor,
            DataPrevisao = dto.DataPrevisao,
            ResponsavelId = lead.ResponsavelId,
            Observacao = dto.Observacao ?? lead.Observacao
        };

        var oportunidadeId = await _oportunidadeRepository.CriarAsync(oportunidade);

        lead.Status = "convertido";
        await _leadRepository.AtualizarAsync(lead);

        return new LeadConverterResponseDTO
        {
            LeadId = lead.Id,
            LeadNome = lead.Nome,
            OportunidadeId = oportunidadeId,
            OportunidadeTitulo = titulo
        };
    }

    public async Task<LeadKanbanDTO> ObterKanbanAsync(string idEmpresa)
    {
        var estagios = await _leadRepository.ObterEstagiosAsync(idEmpresa);

        var kanban = new LeadKanbanDTO
        {
            Estagios = estagios.Select(e => new LeadEstagioDTO
            {
                Id = e.Id,
                Nome = e.Nome,
                Ordem = e.Ordem,
                Cor = e.Cor
            }).ToList(),
            Colunas = new List<LeadKanbanColunaDTO>()
        };

        foreach (var estagio in estagios)
        {
            var leads = await _leadRepository.ObterPorEstagioAsync(estagio.Id, idEmpresa);
            kanban.Colunas.Add(new LeadKanbanColunaDTO
            {
                EstagioId = estagio.Id,
                EstagioNome = estagio.Nome,
                Ordem = estagio.Ordem,
                Cor = estagio.Cor,
                Leads = leads.Select(MapearParaDTO).ToList()
            });
        }

        return kanban;
    }

    public async Task<List<LeadEstagioDTO>> ObterEstagiosAsync(string idEmpresa)
    {
        var estagios = await _leadRepository.ObterEstagiosAsync(idEmpresa);
        return estagios.Select(e => new LeadEstagioDTO
        {
            Id = e.Id,
            Nome = e.Nome,
            Ordem = e.Ordem,
            Cor = e.Cor
        }).ToList();
    }

    public async Task<LeadEstagioDTO> CriarEstagioAsync(LeadEstagioCreateDTO dto, string idEmpresa)
    {
        var estagio = new LeadEstagio
        {
            IdEmpresa = idEmpresa,
            Nome = dto.Nome,
            Ordem = dto.Ordem,
            Cor = dto.Cor
        };

        var id = await _leadRepository.CriarEstagioAsync(estagio);
        estagio.Id = id;

        return new LeadEstagioDTO
        {
            Id = estagio.Id,
            Nome = estagio.Nome,
            Ordem = estagio.Ordem,
            Cor = estagio.Cor
        };
    }

    public async Task<LeadEstagioDTO?> AtualizarEstagioAsync(int id, LeadEstagioUpdateDTO dto, string idEmpresa)
    {
        var estagios = await _leadRepository.ObterEstagiosAsync(idEmpresa);
        var estagio = estagios.FirstOrDefault(e => e.Id == id);
        if (estagio == null) return null;

        estagio.Nome = dto.Nome;
        estagio.Ordem = dto.Ordem;
        estagio.Cor = dto.Cor;

        await _leadRepository.AtualizarEstagioAsync(estagio);

        return new LeadEstagioDTO
        {
            Id = estagio.Id,
            Nome = estagio.Nome,
            Ordem = estagio.Ordem,
            Cor = estagio.Cor
        };
    }

    public async Task<bool> ExcluirEstagioAsync(int id, string idEmpresa)
    {
        var estagios = await _leadRepository.ObterEstagiosAsync(idEmpresa);
        var estagio = estagios.FirstOrDefault(e => e.Id == id);
        if (estagio == null) return false;

        await _leadRepository.ExcluirEstagioAsync(id, idEmpresa);
        return true;
    }

    public async Task<LeadStatsDTO> ObterStatsAsync(string idEmpresa)
    {
        return await _leadRepository.ObterStatsAsync(idEmpresa);
    }

    private static LeadDTO MapearParaDTO(Lead lead)
    {
        return new LeadDTO
        {
            Id = lead.Id,
            IdEmpresa = lead.IdEmpresa,
            Nome = lead.Nome,
            Empresa = lead.Empresa,
            Email = lead.Email,
            Telefone = lead.Telefone,
            Source = lead.Source,
            Status = lead.Status,
            Temperatura = lead.Temperatura,
            IdEstagio = lead.IdEstagio,
            IdParceiro = lead.IdParceiro,
            Observacao = lead.Observacao,
            ResponsavelId = lead.ResponsavelId,
            ResponsavelNome = lead.ResponsavelNome,
            ResponsavelAvatar = lead.ResponsavelAvatar,
            Ativo = lead.Ativo,
            DataCadastro = lead.DataCadastro
        };
    }
}
