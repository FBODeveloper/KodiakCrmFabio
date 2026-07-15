using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.UseCases.Services;

public class ParceiroService
{
    private readonly IParceiroRepository _repository;

    public ParceiroService(IParceiroRepository repository)
    {
        _repository = repository;
    }

    public async Task<ParceiroDTO?> ObterPorIdAsync(int id, string idEmpresa)
    {
        var parceiro = await _repository.ObterPorIdAsync(id, idEmpresa);
        if (parceiro == null) return null;
        return MapearParaDTO(parceiro);
    }

    public async Task<ParceiroListDTO> ObterListaAsync(string idEmpresa, string? busca, int pagina, int itensPorPagina)
    {
        var resultado = await _repository.ObterListaAsync(idEmpresa, busca, pagina, itensPorPagina);
        return new ParceiroListDTO
        {
            Itens = resultado.Itens.Select(MapearParaDTO).ToList(),
            Total = resultado.Total,
            Pagina = pagina,
            ItensPorPagina = itensPorPagina
        };
    }

    public async Task<ParceiroDTO> CriarAsync(ParceiroCreateDTO dto, string idEmpresa, string idEstabelecimento, string cnpjEmpresa)
    {
        if (!string.IsNullOrWhiteSpace(dto.CpfCnpj))
        {
            var existe = await _repository.ExisteCpfCnpjAsync(dto.CpfCnpj, idEmpresa);
            if (existe)
                throw new InvalidOperationException("CPF/CNPJ já cadastrado para esta empresa");
        }

        var parceiro = new Parceiro
        {
            IdEmpresa = idEmpresa,
            IdEstabelecimento = idEstabelecimento,
            CnpjEmpresa = cnpjEmpresa,
            RazaoSocial = dto.RazaoSocial,
            NomeFantasia = dto.NomeFantasia,
            CpfCnpj = dto.CpfCnpj,
            TipoPessoa = dto.TipoPessoa,
            Email = dto.Email,
            Telefone = dto.Telefone,
            Celular = dto.Celular,
            Observacao = dto.Observacao
        };

        var id = await _repository.CriarAsync(parceiro);
        parceiro.Id = id;

        return MapearParaDTO(parceiro);
    }

    public async Task<ParceiroDTO?> AtualizarAsync(int id, ParceiroUpdateDTO dto, string idEmpresa)
    {
        var parceiro = await _repository.ObterPorIdAsync(id, idEmpresa);
        if (parceiro == null) return null;

        if (!string.IsNullOrWhiteSpace(dto.CpfCnpj))
        {
            var existe = await _repository.ExisteCpfCnpjAsync(dto.CpfCnpj, idEmpresa, id);
            if (existe)
                throw new InvalidOperationException("CPF/CNPJ já cadastrado para esta empresa");
        }

        parceiro.RazaoSocial = dto.RazaoSocial;
        parceiro.NomeFantasia = dto.NomeFantasia;
        parceiro.CpfCnpj = dto.CpfCnpj;
        parceiro.TipoPessoa = dto.TipoPessoa;
        parceiro.Email = dto.Email;
        parceiro.Telefone = dto.Telefone;
        parceiro.Celular = dto.Celular;
        parceiro.Observacao = dto.Observacao;

        await _repository.AtualizarAsync(parceiro);

        return MapearParaDTO(parceiro);
    }

    public async Task<bool> SincronizarComErpAsync(ParceiroSyncDTO dto)
    {
        var existente = await _repository.ObterPorKodiakErpAsync(dto.IdParceiroKodiakErp, dto.IdEmpresa);

        if (existente != null)
        {
            existente.RazaoSocial = dto.RazaoSocial;
            existente.NomeFantasia = dto.NomeFantasia;
            existente.CpfCnpj = dto.CpfCnpj;
            existente.TipoPessoa = dto.TipoPessoa;
            existente.Email = dto.Email;
            existente.Telefone = dto.Telefone;
            existente.Celular = dto.Celular;

            await _repository.AtualizarAsync(existente);
            return true;
        }

        var parceiro = new Parceiro
        {
            IdEmpresa = dto.IdEmpresa,
            IdEstabelecimento = dto.IdEstabelecimento,
            CnpjEmpresa = dto.CnpjEmpresa,
            RazaoSocial = dto.RazaoSocial,
            NomeFantasia = dto.NomeFantasia,
            CpfCnpj = dto.CpfCnpj,
            TipoPessoa = dto.TipoPessoa,
            Email = dto.Email,
            Telefone = dto.Telefone,
            Celular = dto.Celular,
            IdParceiroKodiakErp = dto.IdParceiroKodiakErp
        };

        var id = await _repository.CriarAsync(parceiro);
        return id > 0;
    }

    private static ParceiroDTO MapearParaDTO(Parceiro parceiro)
    {
        return new ParceiroDTO
        {
            Id = parceiro.Id,
            IdEmpresa = parceiro.IdEmpresa,
            RazaoSocial = parceiro.RazaoSocial,
            NomeFantasia = parceiro.NomeFantasia,
            CpfCnpj = parceiro.CpfCnpj,
            TipoPessoa = parceiro.TipoPessoa,
            Email = parceiro.Email,
            Telefone = parceiro.Telefone,
            Celular = parceiro.Celular,
            IdParceiroKodiakErp = parceiro.IdParceiroKodiakErp,
            Observacao = parceiro.Observacao,
            Ativo = parceiro.Ativo,
            DataCadastro = parceiro.DataCadastro
        };
    }
}
