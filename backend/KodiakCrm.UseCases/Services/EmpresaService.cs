using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.UseCases.Services;

public class EmpresaService
{
    private readonly IEmpresaRepository _repository;

    public EmpresaService(IEmpresaRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmpresaDTO?> ObterPorCnpjAsync(string cnpj)
    {
        var empresa = await _repository.ObterPorCnpjAsync(cnpj);
        if (empresa == null) return null;

        var totalUsuarios = await _repository.ContarUsuariosAsync(cnpj);

        return new EmpresaDTO
        {
            Cnpj = empresa.Cnpj,
            RazaoSocial = empresa.RazaoSocial,
            NomeFantasia = empresa.NomeFantasia,
            QuantidadeUsuariosContratados = empresa.QuantidadeUsuariosContratados,
            Ativo = empresa.Ativo,
            DataCadastro = empresa.DataCadastro,
            TotalUsuarios = totalUsuarios
        };
    }

    public async Task<EmpresaListDTO> ObterListaAsync(string? busca, int pagina, int itensPorPagina)
    {
        var resultado = await _repository.ObterListaAsync(busca, pagina, itensPorPagina);

        var itens = new List<EmpresaDTO>();
        foreach (var empresa in resultado.Itens)
        {
            var totalUsuarios = await _repository.ContarUsuariosAsync(empresa.Cnpj);
            itens.Add(new EmpresaDTO
            {
                Cnpj = empresa.Cnpj,
                RazaoSocial = empresa.RazaoSocial,
                NomeFantasia = empresa.NomeFantasia,
                QuantidadeUsuariosContratados = empresa.QuantidadeUsuariosContratados,
                Ativo = empresa.Ativo,
                DataCadastro = empresa.DataCadastro,
                TotalUsuarios = totalUsuarios
            });
        }

        return new EmpresaListDTO
        {
            Itens = itens,
            Total = resultado.Total,
            Pagina = pagina,
            ItensPorPagina = itensPorPagina
        };
    }

    public async Task<(bool Sucesso, string Mensagem, EmpresaDTO? Empresa)> CriarAsync(EmpresaCreateDTO dto)
    {
        var existente = await _repository.ObterPorCnpjAsync(dto.Cnpj);
        if (existente != null)
        {
            return (false, "Já existe uma empresa com este CNPJ", null);
        }

        var empresa = new Empresa
        {
            Cnpj = dto.Cnpj,
            RazaoSocial = dto.RazaoSocial,
            NomeFantasia = dto.NomeFantasia,
            QuantidadeUsuariosContratados = dto.QuantidadeUsuariosContratados
        };

        await _repository.CriarAsync(empresa);

        var criada = await _repository.ObterPorCnpjAsync(dto.Cnpj);
        return (true, "Empresa criada com sucesso", new EmpresaDTO
        {
            Cnpj = criada!.Cnpj,
            RazaoSocial = criada.RazaoSocial,
            NomeFantasia = criada.NomeFantasia,
            QuantidadeUsuariosContratados = criada.QuantidadeUsuariosContratados,
            Ativo = criada.Ativo,
            DataCadastro = criada.DataCadastro,
            TotalUsuarios = 0
        });
    }

    public async Task<(bool Sucesso, string Mensagem)> AtualizarAsync(string cnpj, EmpresaUpdateDTO dto)
    {
        var existente = await _repository.ObterPorCnpjAsync(cnpj);
        if (existente == null)
        {
            return (false, "Empresa não encontrada");
        }

        existente.RazaoSocial = dto.RazaoSocial;
        existente.NomeFantasia = dto.NomeFantasia;
        existente.QuantidadeUsuariosContratados = dto.QuantidadeUsuariosContratados;

        await _repository.AtualizarAsync(existente);
        return (true, "Empresa atualizada com sucesso");
    }
}
