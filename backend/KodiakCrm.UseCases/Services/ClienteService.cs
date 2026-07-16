using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.UseCases.Services;

public class ClienteService
{
    private readonly IClienteRepository _repository;

    public ClienteService(IClienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<ClienteDTO?> ObterPorIdAsync(int id, string idEmpresa)
    {
        var cliente = await _repository.ObterPorIdAsync(id, idEmpresa);
        if (cliente == null) return null;
        return MapearParaDTO(cliente);
    }

    public async Task<ClienteListDTO> ObterListaAsync(string idEmpresa, string? busca, int pagina, int itensPorPagina)
    {
        var resultado = await _repository.ObterListaAsync(idEmpresa, busca, pagina, itensPorPagina);
        return new ClienteListDTO
        {
            Itens = resultado.Itens.Select(MapearParaDTO).ToList(),
            Total = resultado.Total,
            Pagina = pagina,
            ItensPorPagina = itensPorPagina
        };
    }

    public async Task<ClienteDTO> CriarAsync(ClienteCreateDTO dto, string idEmpresa, string idEstabelecimento, string cnpjEmpresa)
    {
        var cliente = new Cliente
        {
            IdEmpresa = idEmpresa,
            IdEstabelecimento = idEstabelecimento,
            CnpjEmpresa = cnpjEmpresa,
            RazaoSocial = dto.RazaoSocial,
            NomeFantasia = dto.NomeFantasia,
            CnpjCpf = dto.CnpjCpf,
            Email = dto.Email,
            Telefone = dto.Telefone,
            Celular = dto.Celular,
            Endereco = dto.Endereco,
            Observacao = dto.Observacao,
            ResponsavelId = dto.ResponsavelId
        };

        var id = await _repository.CriarAsync(cliente);
        cliente.Id = id;

        return MapearParaDTO(cliente);
    }

    public async Task<ClienteDTO?> ConverterDeOportunidadeAsync(int oportunidadeId, string idEmpresa, string idEstabelecimento, string cnpjEmpresa)
    {
        var clienteExistente = await _repository.ObterListaAsync(idEmpresa, null, 1, 1);
        // Check if a client was already created from this opportunity
        var existing = clienteExistente.Itens.FirstOrDefault(c => c.IdOportunidade == oportunidadeId);
        if (existing != null) return MapearParaDTO(existing);

        var cliente = new Cliente
        {
            IdEmpresa = idEmpresa,
            IdEstabelecimento = idEstabelecimento,
            CnpjEmpresa = cnpjEmpresa,
            RazaoSocial = "Cliente - " + DateTime.UtcNow.ToString("dd/MM/yyyy"),
            Origem = "oportunidade",
            DataConversao = DateTime.UtcNow,
            IdOportunidade = oportunidadeId
        };

        var id = await _repository.CriarAsync(cliente);
        cliente.Id = id;

        return MapearParaDTO(cliente);
    }

    public async Task<ClienteDTO?> AtualizarAsync(int id, ClienteUpdateDTO dto, string idEmpresa)
    {
        var cliente = await _repository.ObterPorIdAsync(id, idEmpresa);
        if (cliente == null) return null;

        cliente.RazaoSocial = dto.RazaoSocial;
        cliente.NomeFantasia = dto.NomeFantasia;
        cliente.CnpjCpf = dto.CnpjCpf;
        cliente.Email = dto.Email;
        cliente.Telefone = dto.Telefone;
        cliente.Celular = dto.Celular;
        cliente.Endereco = dto.Endereco;
        cliente.Observacao = dto.Observacao;
        cliente.ResponsavelId = dto.ResponsavelId;

        await _repository.AtualizarAsync(cliente);

        return MapearParaDTO(cliente);
    }

    public async Task<bool> ExcluirAsync(int id, string idEmpresa)
    {
        var cliente = await _repository.ObterPorIdAsync(id, idEmpresa);
        if (cliente == null) return false;

        await _repository.ExcluirAsync(id, idEmpresa);
        return true;
    }

    private static ClienteDTO MapearParaDTO(Cliente cliente)
    {
        return new ClienteDTO
        {
            Id = cliente.Id,
            IdEmpresa = cliente.IdEmpresa,
            RazaoSocial = cliente.RazaoSocial,
            NomeFantasia = cliente.NomeFantasia,
            CnpjCpf = cliente.CnpjCpf,
            Email = cliente.Email,
            Telefone = cliente.Telefone,
            Celular = cliente.Celular,
            Endereco = cliente.Endereco,
            Observacao = cliente.Observacao,
            Origem = cliente.Origem,
            DataConversao = cliente.DataConversao,
            IdOportunidade = cliente.IdOportunidade,
            ResponsavelId = cliente.ResponsavelId,
            ResponsavelNome = cliente.ResponsavelNome,
            Ativo = cliente.Ativo,
            DataCadastro = cliente.DataCadastro
        };
    }
}
