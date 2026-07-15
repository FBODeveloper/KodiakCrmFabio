using System.Security.Claims;
using KodiakCrm.Core.DTOs;
using KodiakCrm.UseCases.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KodiakCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HistoricoController : ControllerBase
{
    private readonly HistoricoService _service;

    public HistoricoController(HistoricoService service)
    {
        _service = service;
    }

    private string ObterIdEmpresa() => User.FindFirst("id_empresa")?.Value ?? string.Empty;

    [HttpGet("recentes")]
    public async Task<ActionResult<List<HistoricoDTO>>> ObterRecentes([FromQuery] int limite = 20)
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var resultado = await _service.ObterRecentesAsync(idEmpresa, limite);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao obter histórico", erro = ex.Message });
        }
    }

    [HttpGet("{entidade}/{entidadeId}")]
    public async Task<ActionResult<List<HistoricoDTO>>> ObterPorEntidade(string entidade, int entidadeId)
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var resultado = await _service.ObterPorEntidadeAsync(entidade, entidadeId, idEmpresa);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao obter histórico", erro = ex.Message });
        }
    }
}
