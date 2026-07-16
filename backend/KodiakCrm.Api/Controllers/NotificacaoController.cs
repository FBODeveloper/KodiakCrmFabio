using System.Security.Claims;
using KodiakCrm.Core.DTOs;
using KodiakCrm.UseCases.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KodiakCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificacaoController : ControllerBase
{
    private readonly NotificacaoService _service;

    public NotificacaoController(NotificacaoService service)
    {
        _service = service;
    }

    private string ObterIdEmpresa() => User.FindFirst("id_empresa")?.Value ?? string.Empty;
    private int ObterUsuarioId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value;
        return int.TryParse(claim, out var id) ? id : 0;
    }

    [HttpGet("resumo")]
    public async Task<ActionResult<NotificacaoResumoDTO>> ObterResumo()
    {
        var resultado = await _service.ObterResumoAsync(ObterIdEmpresa(), ObterUsuarioId());
        return Ok(resultado);
    }

    [HttpGet]
    public async Task<ActionResult<List<NotificacaoDTO>>> ObterNotificacoes([FromQuery] int limite = 50)
    {
        var resultado = await _service.ObterPorUsuarioAsync(ObterIdEmpresa(), ObterUsuarioId(), limite);
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<ActionResult> Criar([FromBody] NotificacaoCriarDTO dto)
    {
        await _service.CriarAsync(ObterIdEmpresa(), dto);
        return Created("", new { mensagem = "Notificação criada" });
    }

    [HttpPut("{id}/lida")]
    public async Task<ActionResult> MarcarLida(int id)
    {
        await _service.MarcarLidaAsync(id, ObterIdEmpresa());
        return Ok(new { mensagem = "Notificação marcada como lida" });
    }

    [HttpPut("lidas")]
    public async Task<ActionResult> MarcarTodasLidas()
    {
        await _service.MarcarTodasLidasAsync(ObterIdEmpresa(), ObterUsuarioId());
        return Ok(new { mensagem = "Todas notificações marcadas como lidas" });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Excluir(int id)
    {
        await _service.ExcluirAsync(id, ObterIdEmpresa());
        return Ok(new { mensagem = "Notificação excluída" });
    }
}
