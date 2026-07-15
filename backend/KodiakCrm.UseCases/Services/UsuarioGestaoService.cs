using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.UseCases.Services;

public class UsuarioGestaoService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEmpresaRepository _empresaRepository;

    public UsuarioGestaoService(IUsuarioRepository usuarioRepository, IEmpresaRepository empresaRepository)
    {
        _usuarioRepository = usuarioRepository;
        _empresaRepository = empresaRepository;
    }

    public async Task<UsuarioGestaoDTO?> ObterPorIdAsync(int id, string idEmpresa)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(id, idEmpresa);
        if (usuario == null) return null;

        return MapearParaDTO(usuario);
    }

    public async Task<UsuarioGestaoListDTO> ObterListaAsync(string idEmpresa, string? busca, string? perfil, int pagina, int itensPorPagina)
    {
        var resultado = await _usuarioRepository.ObterListaAsync(idEmpresa, busca, perfil, pagina, itensPorPagina);

        return new UsuarioGestaoListDTO
        {
            Itens = resultado.Itens.Select(MapearParaDTO).ToList(),
            Total = resultado.Total,
            Pagina = pagina,
            ItensPorPagina = itensPorPagina
        };
    }

    public async Task<(bool Sucesso, string Mensagem, UsuarioGestaoDTO? Usuario)> CriarAsync(string idEmpresa, UsuarioCreateDTO dto)
    {
        var empresa = await _empresaRepository.ObterPorCnpjAsync(idEmpresa);
        if (empresa == null)
        {
            return (false, "Empresa não encontrada", null);
        }

        var totalUsuarios = await _usuarioRepository.ContarPorEmpresaAsync(idEmpresa);
        if (totalUsuarios >= empresa.QuantidadeUsuariosContratados)
        {
            return (false, $"Limite de usuários atingido. Contratados: {empresa.QuantidadeUsuariosContratados}, Ativos: {totalUsuarios}", null);
        }

        var existente = await _usuarioRepository.ObterPorEmailAsync(dto.Email, idEmpresa);
        if (existente != null)
        {
            return (false, "Já existe um usuário com este e-mail nesta empresa", null);
        }

        var usuario = new Usuario
        {
            IdEmpresa = idEmpresa,
            IdEstabelecimento = idEmpresa,
            CnpjEmpresa = idEmpresa,
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = AuthService.HashSenha(dto.Senha),
            Perfil = dto.Perfil,
            Avatar = dto.Avatar,
            DataNascimento = dto.DataNascimento,
            Ativo = true
        };

        var id = await _usuarioRepository.CriarAsync(usuario);
        var criado = await _usuarioRepository.ObterPorIdAsync(id, idEmpresa);

        return (true, "Usuário criado com sucesso", MapearParaDTO(criado!));
    }

    public async Task<(bool Sucesso, string Mensagem)> AtualizarAsync(int id, string idEmpresa, UsuarioUpdateDTO dto)
    {
        var existente = await _usuarioRepository.ObterPorIdAsync(id, idEmpresa);
        if (existente == null)
        {
            return (false, "Usuário não encontrado");
        }

        if (!string.IsNullOrEmpty(dto.Email) && dto.Email != existente.Email)
        {
            var emailExistente = await _usuarioRepository.ObterPorEmailAsync(dto.Email, idEmpresa);
            if (emailExistente != null)
            {
                return (false, "Já existe um usuário com este e-mail nesta empresa");
            }
        }

        existente.Nome = dto.Nome;
        if (!string.IsNullOrEmpty(dto.Email))
            existente.Email = dto.Email;
        existente.Perfil = dto.Perfil;
        existente.Avatar = dto.Avatar;
        existente.DataNascimento = dto.DataNascimento;
        if (dto.Ativo.HasValue)
            existente.Ativo = dto.Ativo.Value;

        if (!string.IsNullOrEmpty(dto.Senha))
        {
            existente.SenhaHash = AuthService.HashSenha(dto.Senha);
        }

        await _usuarioRepository.AtualizarAsync(existente);
        return (true, "Usuário atualizado com sucesso");
    }

    public async Task<(bool Sucesso, string Mensagem)> ExcluirAsync(int id, string idEmpresa)
    {
        var existente = await _usuarioRepository.ObterPorIdAsync(id, idEmpresa);
        if (existente == null)
        {
            return (false, "Usuário não encontrado");
        }

        await _usuarioRepository.ExcluirAsync(id, idEmpresa);
        return (true, "Usuário excluído com sucesso");
    }

    private static UsuarioGestaoDTO MapearParaDTO(Usuario usuario)
    {
        return new UsuarioGestaoDTO
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Perfil = usuario.Perfil,
            Avatar = usuario.Avatar,
            DataNascimento = usuario.DataNascimento,
            Ativo = usuario.Ativo,
            DataCadastro = usuario.DataCadastro
        };
    }
}
