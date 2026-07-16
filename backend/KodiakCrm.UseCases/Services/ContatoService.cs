using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.UseCases.Services;

public class ContatoService
{
    private readonly IContatoRepository _repository;

    public ContatoService(IContatoRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContatoDTO?> ObterPorIdAsync(int id, string idEmpresa)
    {
        var contato = await _repository.ObterPorIdAsync(id, idEmpresa);
        if (contato == null) return null;
        return MapearParaDTO(contato);
    }

    public async Task<ContatoListDTO> ObterListaAsync(string idEmpresa, string? busca, int? idCliente, int? idParceiro, int pagina, int itensPorPagina)
    {
        var resultado = await _repository.ObterListaAsync(idEmpresa, busca, idCliente, idParceiro, pagina, itensPorPagina);
        return new ContatoListDTO
        {
            Itens = resultado.Itens.Select(MapearParaDTO).ToList(),
            Total = resultado.Total,
            Pagina = pagina,
            ItensPorPagina = itensPorPagina
        };
    }

    public async Task<List<ContatoDTO>> ObterPorClienteAsync(int idCliente, string idEmpresa)
    {
        var contatos = await _repository.ObterPorClienteAsync(idCliente, idEmpresa);
        return contatos.Select(MapearParaDTO).ToList();
    }

    public async Task<List<ContatoDTO>> ObterPorParceiroAsync(int idParceiro, string idEmpresa)
    {
        var contatos = await _repository.ObterPorParceiroAsync(idParceiro, idEmpresa);
        return contatos.Select(MapearParaDTO).ToList();
    }

    public async Task<ContatoDTO> CriarAsync(ContatoCreateDTO dto, string idEmpresa, string idEstabelecimento, string cnpjEmpresa)
    {
        var contato = new Contato
        {
            IdEmpresa = idEmpresa,
            IdEstabelecimento = idEstabelecimento,
            CnpjEmpresa = cnpjEmpresa,
            Nome = dto.Nome,
            Cargo = dto.Cargo,
            Email = dto.Email,
            Telefone = dto.Telefone,
            Celular = dto.Celular,
            IdCliente = dto.IdCliente,
            IdParceiro = dto.IdParceiro,
            Observacao = dto.Observacao,
            ResponsavelId = dto.ResponsavelId
        };

        var id = await _repository.CriarAsync(contato);
        contato.Id = id;

        return MapearParaDTO(contato);
    }

    public async Task<ContatoDTO?> AtualizarAsync(int id, ContatoUpdateDTO dto, string idEmpresa)
    {
        var contato = await _repository.ObterPorIdAsync(id, idEmpresa);
        if (contato == null) return null;

        contato.Nome = dto.Nome;
        contato.Cargo = dto.Cargo;
        contato.Email = dto.Email;
        contato.Telefone = dto.Telefone;
        contato.Celular = dto.Celular;
        contato.IdCliente = dto.IdCliente;
        contato.IdParceiro = dto.IdParceiro;
        contato.Observacao = dto.Observacao;
        contato.ResponsavelId = dto.ResponsavelId;

        await _repository.AtualizarAsync(contato);

        return MapearParaDTO(contato);
    }

    public async Task<bool> ExcluirAsync(int id, string idEmpresa)
    {
        var contato = await _repository.ObterPorIdAsync(id, idEmpresa);
        if (contato == null) return false;

        await _repository.ExcluirAsync(id, idEmpresa);
        return true;
    }

    private static ContatoDTO MapearParaDTO(Contato contato)
    {
        return new ContatoDTO
        {
            Id = contato.Id,
            IdEmpresa = contato.IdEmpresa,
            Nome = contato.Nome,
            Cargo = contato.Cargo,
            Email = contato.Email,
            Telefone = contato.Telefone,
            Celular = contato.Celular,
            IdCliente = contato.IdCliente,
            ClienteNome = contato.ClienteNome,
            IdParceiro = contato.IdParceiro,
            ParceiroNome = contato.ParceiroNome,
            Observacao = contato.Observacao,
            ResponsavelId = contato.ResponsavelId,
            ResponsavelNome = contato.ResponsavelNome,
            Ativo = contato.Ativo,
            DataCadastro = contato.DataCadastro
        };
    }
}
