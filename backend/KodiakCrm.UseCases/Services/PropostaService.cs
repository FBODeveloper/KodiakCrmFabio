using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.UseCases.Services;

public class PropostaService
{
    private readonly IPropostaRepository _repository;

    public PropostaService(IPropostaRepository repository)
    {
        _repository = repository;
    }

    public async Task<PropostaDTO?> ObterPorIdAsync(int id, string idEmpresa)
    {
        var proposta = await _repository.ObterPorIdAsync(id, idEmpresa);
        if (proposta == null) return null;

        var itens = await _repository.ObterItensAsync(proposta.Id);
        return MapearParaDTO(proposta, itens);
    }

    public async Task<PropostaListDTO> ObterListaAsync(string idEmpresa, string? busca, string? status, int? idParceiro, DateTime? dataInicio, DateTime? dataFim, int pagina, int itensPorPagina)
    {
        var resultado = await _repository.ObterListaAsync(idEmpresa, busca, status, idParceiro, dataInicio, dataFim, pagina, itensPorPagina);
        return new PropostaListDTO
        {
            Itens = resultado.Itens.Select(p => MapearParaDTO(p, new List<PropostaItem>())).ToList(),
            Total = resultado.Total,
            Pagina = pagina,
            ItensPorPagina = itensPorPagina
        };
    }

    public async Task<PropostaDTO> CriarAsync(PropostaCreateDTO dto, string idEmpresa, string idEstabelecimento, string cnpjEmpresa)
    {
        var proposta = new Proposta
        {
            IdEmpresa = idEmpresa,
            IdEstabelecimento = idEstabelecimento,
            CnpjEmpresa = cnpjEmpresa,
            Titulo = dto.Titulo,
            IdParceiro = dto.IdParceiro,
            IdOportunidade = dto.IdOportunidade,
            DataValidade = dto.DataValidade,
            Observacao = dto.Observacao,
            Status = "rascunho"
        };

        var id = await _repository.CriarAsync(proposta);
        proposta.Id = id;

        var itens = new List<PropostaItem>();
        decimal valorTotal = 0;

        if (dto.Itens != null && dto.Itens.Count > 0)
        {
            foreach (var itemDto in dto.Itens)
            {
                if (string.IsNullOrWhiteSpace(itemDto.Descricao))
                    continue;

                var valorItem = itemDto.Quantidade * itemDto.ValorUnitario;
                var item = new PropostaItem
                {
                    IdProposta = id,
                    Descricao = itemDto.Descricao,
                    Quantidade = itemDto.Quantidade,
                    ValorUnitario = itemDto.ValorUnitario,
                    ValorTotal = valorItem
                };

                var itemId = await _repository.CriarItemAsync(item);
                item.Id = itemId;
                itens.Add(item);
                valorTotal += valorItem;
            }
        }

        proposta.ValorTotal = valorTotal;
        await _repository.AtualizarAsync(proposta);

        return MapearParaDTO(proposta, itens);
    }

    public async Task<PropostaDTO?> AtualizarAsync(int id, PropostaUpdateDTO dto, string idEmpresa)
    {
        var proposta = await _repository.ObterPorIdAsync(id, idEmpresa);
        if (proposta == null) return null;

        proposta.Titulo = dto.Titulo;
        proposta.IdParceiro = dto.IdParceiro;
        proposta.IdOportunidade = dto.IdOportunidade;
        proposta.DataValidade = dto.DataValidade;
        proposta.Observacao = dto.Observacao;

        await _repository.ExcluirItensAsync(id);

        var itens = new List<PropostaItem>();
        decimal valorTotal = 0;

        if (dto.Itens != null && dto.Itens.Count > 0)
        {
            foreach (var itemDto in dto.Itens)
            {
                if (string.IsNullOrWhiteSpace(itemDto.Descricao))
                    continue;

                var valorItem = itemDto.Quantidade * itemDto.ValorUnitario;
                var item = new PropostaItem
                {
                    IdProposta = id,
                    Descricao = itemDto.Descricao,
                    Quantidade = itemDto.Quantidade,
                    ValorUnitario = itemDto.ValorUnitario,
                    ValorTotal = valorItem
                };

                var itemId = await _repository.CriarItemAsync(item);
                item.Id = itemId;
                itens.Add(item);
                valorTotal += valorItem;
            }
        }

        proposta.ValorTotal = valorTotal;
        await _repository.AtualizarAsync(proposta);

        return MapearParaDTO(proposta, itens);
    }

    public async Task<PropostaDTO?> AlterarStatusAsync(int id, string status, string idEmpresa)
    {
        var proposta = await _repository.ObterPorIdAsync(id, idEmpresa);
        if (proposta == null) return null;

        proposta.Status = status;
        await _repository.AtualizarAsync(proposta);

        var itens = await _repository.ObterItensAsync(proposta.Id);
        return MapearParaDTO(proposta, itens);
    }

    private static PropostaDTO MapearParaDTO(Proposta proposta, List<PropostaItem> itens)
    {
        return new PropostaDTO
        {
            Id = proposta.Id,
            IdEmpresa = proposta.IdEmpresa,
            Titulo = proposta.Titulo,
            IdParceiro = proposta.IdParceiro,
            IdOportunidade = proposta.IdOportunidade,
            ValorTotal = proposta.ValorTotal,
            DataValidade = proposta.DataValidade,
            Status = proposta.Status,
            Observacao = proposta.Observacao,
            Ativo = proposta.Ativo,
            DataCadastro = proposta.DataCadastro,
            Itens = itens.Select(i => new PropostaItemDTO
            {
                Id = i.Id,
                Descricao = i.Descricao,
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario,
                ValorTotal = i.ValorTotal
            }).ToList()
        };
    }
}
