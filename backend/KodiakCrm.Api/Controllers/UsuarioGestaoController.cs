using System.Security.Claims;
using KodiakCrm.Core.DTOs;
using KodiakCrm.UseCases.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KodiakCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuarioGestaoController : ControllerBase
{
    private readonly UsuarioGestaoService _usuarioGestaoService;

    public UsuarioGestaoController(UsuarioGestaoService usuarioGestaoService)
    {
        _usuarioGestaoService = usuarioGestaoService;
    }

    private string ObterIdEmpresa()
    {
        return User.FindFirst("id_empresa")?.Value ?? string.Empty;
    }

    private bool IsAdmin()
    {
        return User.IsInRole("admin");
    }

    [HttpGet]
    public async Task<ActionResult<UsuarioGestaoListDTO>> ObterLista([FromQuery] string? busca, [FromQuery] string? perfil, [FromQuery] int pagina = 1, [FromQuery] int itensPorPagina = 20)
    {
        var idEmpresa = ObterIdEmpresa();
        var resultado = await _usuarioGestaoService.ObterListaAsync(idEmpresa, busca, perfil, pagina, itensPorPagina);
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioGestaoDTO>> ObterPorId(int id)
    {
        var idEmpresa = ObterIdEmpresa();
        var usuario = await _usuarioGestaoService.ObterPorIdAsync(id, idEmpresa);

        if (usuario == null)
        {
            return NotFound(new { mensagem = "Usuário não encontrado" });
        }

        return Ok(usuario);
    }

    private bool IsGerente()
    {
        return User.IsInRole("gerente");
    }

    [HttpPost]
    public async Task<ActionResult> Criar([FromBody] UsuarioCreateDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { mensagem = "Dados inválidos" });
        }

        string idEmpresa;
        if (IsAdmin())
        {
            idEmpresa = !string.IsNullOrEmpty(dto.IdEmpresa) ? dto.IdEmpresa : ObterIdEmpresa();
        }
        else
        {
            idEmpresa = ObterIdEmpresa();

            if (!string.IsNullOrEmpty(dto.IdEmpresa) && dto.IdEmpresa != idEmpresa)
            {
                return Forbid();
            }

            if (dto.Perfil == "admin")
            {
                return Forbid();
            }
        }

        var (sucesso, mensagem, usuario) = await _usuarioGestaoService.CriarAsync(idEmpresa, dto);

        if (!sucesso)
        {
            return BadRequest(new { mensagem });
        }

        return CreatedAtAction(nameof(ObterPorId), new { id = usuario!.Id }, usuario);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Atualizar(int id, [FromBody] UsuarioUpdateDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { mensagem = "Dados inválidos" });
        }

        var idEmpresa = ObterIdEmpresa();
        var (sucesso, mensagem) = await _usuarioGestaoService.AtualizarAsync(id, idEmpresa, dto);

        if (!sucesso)
        {
            return NotFound(new { mensagem });
        }

        return Ok(new { mensagem });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Excluir(int id)
    {
        var idEmpresa = ObterIdEmpresa();
        var (sucesso, mensagem) = await _usuarioGestaoService.ExcluirAsync(id, idEmpresa);

        if (!sucesso)
        {
            return NotFound(new { mensagem });
        }

        return Ok(new { mensagem });
    }
}
