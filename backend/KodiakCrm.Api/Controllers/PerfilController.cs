using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KodiakCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PerfilController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEmpresaRepository _empresaRepository;

    public PerfilController(IUsuarioRepository usuarioRepository, IEmpresaRepository empresaRepository)
    {
        _usuarioRepository = usuarioRepository;
        _empresaRepository = empresaRepository;
    }

    private int ObterUsuarioId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value;
        return int.TryParse(claim, out var id) ? id : 0;
    }

    private string ObterIdEmpresa() => User.FindFirst("id_empresa")?.Value ?? string.Empty;

    [HttpGet]
    public async Task<ActionResult<UsuarioPerfilDTO>> ObterPerfil()
    {
        var usuarioId = ObterUsuarioId();
        var idEmpresa = ObterIdEmpresa();
        var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId, idEmpresa);
        if (usuario == null)
            return NotFound(new { mensagem = "Usuário não encontrado" });

        var empresa = await _empresaRepository.ObterPorCnpjAsync(usuario.IdEmpresa);

        return Ok(new UsuarioPerfilDTO
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Avatar = usuario.Avatar,
            DataNascimento = usuario.DataNascimento,
            Perfil = usuario.Perfil,
            EmpresaNome = empresa?.NomeFantasia ?? empresa?.RazaoSocial,
            EmpresaCnpj = empresa?.Cnpj
        });
    }

    [HttpPut]
    public async Task<ActionResult> AtualizarPerfil([FromBody] UsuarioPerfilUpdateDTO dto)
    {
        var usuarioId = ObterUsuarioId();
        var idEmpresa = ObterIdEmpresa();
        var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId, idEmpresa);
        if (usuario == null)
            return NotFound(new { mensagem = "Usuário não encontrado" });

        if (dto.Nome != null) usuario.Nome = dto.Nome;
        if (dto.Email != null) usuario.Email = dto.Email;
        if (dto.Avatar != null) usuario.Avatar = dto.Avatar;
        if (dto.DataNascimento.HasValue) usuario.DataNascimento = dto.DataNascimento.Value;

        await _usuarioRepository.AtualizarAsync(usuario);
        return Ok(new { mensagem = "Perfil atualizado com sucesso" });
    }

    [HttpPut("senha")]
    public async Task<ActionResult> AlterarSenha([FromBody] UsuarioAlterarSenhaDTO dto)
    {
        if (dto.NovaSenha != dto.ConfirmarSenha)
            return BadRequest(new { mensagem = "As senhas não conferem" });

        if (dto.NovaSenha.Length < 6)
            return BadRequest(new { mensagem = "A nova senha deve ter pelo menos 6 caracteres" });

        var usuarioId = ObterUsuarioId();
        var idEmpresa = ObterIdEmpresa();
        var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId, idEmpresa);
        if (usuario == null)
            return NotFound(new { mensagem = "Usuário não encontrado" });

        var senhaHash = HashSenha(dto.SenhaAtual);
        if (usuario.SenhaHash != senhaHash)
            return BadRequest(new { mensagem = "Senha atual incorreta" });

        usuario.SenhaHash = HashSenha(dto.NovaSenha);
        await _usuarioRepository.AtualizarAsync(usuario);

        return Ok(new { mensagem = "Senha alterada com sucesso" });
    }

    private static string HashSenha(string senha)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
        return Convert.ToBase64String(bytes);
    }
}
