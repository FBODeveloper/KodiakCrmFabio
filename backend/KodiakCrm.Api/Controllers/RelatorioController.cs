using System.Security.Claims;
using KodiakCrm.Core.DTOs;
using KodiakCrm.UseCases.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KodiakCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RelatorioController : ControllerBase
{
    private readonly RelatorioService _service;

    public RelatorioController(RelatorioService service)
    {
        _service = service;
    }

    private string ObterIdEmpresa() => User.FindFirst("id_empresa")?.Value ?? string.Empty;
    private string ObterCnpjEmpresa() => User.FindFirst("cnpj_empresa")?.Value ?? string.Empty;
    private int ObterUsuarioId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value;
        return int.TryParse(claim, out var id) ? id : 0;
    }
    private string ObterNomeUsuario() => User.FindFirst("nome")?.Value ?? string.Empty;

    [HttpGet("vendas")]
    public async Task<ActionResult<RelatorioVendasDTO>> RelatorioVendas(
        [FromQuery] DateTime? dataInicio, [FromQuery] DateTime? dataFim,
        [FromQuery] string? status, [FromQuery] int? responsavelId)
    {
        var resultado = await _service.GerarRelatorioVendasAsync(
            ObterIdEmpresa(), ObterCnpjEmpresa(),
            new RelatorioFiltroDTO { DataInicio = dataInicio, DataFim = dataFim, Status = status, ResponsavelId = responsavelId },
            ObterUsuarioId(), ObterNomeUsuario());
        return Ok(resultado);
    }

    [HttpGet("atividades")]
    public async Task<ActionResult<RelatorioAtividadesDTO>> RelatorioAtividades(
        [FromQuery] DateTime? dataInicio, [FromQuery] DateTime? dataFim,
        [FromQuery] string? tipo, [FromQuery] int? responsavelId)
    {
        var resultado = await _service.GerarRelatorioAtividadesAsync(
            ObterIdEmpresa(), ObterCnpjEmpresa(),
            new RelatorioFiltroDTO { DataInicio = dataInicio, DataFim = dataFim, TipoAtividade = tipo, ResponsavelId = responsavelId },
            ObterUsuarioId(), ObterNomeUsuario());
        return Ok(resultado);
    }

    [HttpGet("performance")]
    public async Task<ActionResult<RelatorioPerformanceDTO>> RelatorioPerformance(
        [FromQuery] DateTime? dataInicio, [FromQuery] DateTime? dataFim,
        [FromQuery] int? responsavelId)
    {
        var resultado = await _service.GerarRelatorioPerformanceAsync(
            ObterIdEmpresa(), ObterCnpjEmpresa(),
            new RelatorioFiltroDTO { DataInicio = dataInicio, DataFim = dataFim, ResponsavelId = responsavelId },
            ObterUsuarioId(), ObterNomeUsuario());
        return Ok(resultado);
    }

    [HttpGet("recentes")]
    public async Task<ActionResult<List<RelatorioGeradoDTO>>> RelatoriosRecentes([FromQuery] int limite = 20)
    {
        var resultado = await _service.ObterRecentesAsync(ObterIdEmpresa(), limite);
        return Ok(resultado);
    }
}
