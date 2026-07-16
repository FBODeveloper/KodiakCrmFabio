using System.Security.Claims;
using KodiakCrm.Core.DTOs;
using KodiakCrm.UseCases.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KodiakCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _service;

    public DashboardController(DashboardService service)
    {
        _service = service;
    }

    private string ObterIdEmpresa() => User.FindFirst("id_empresa")?.Value ?? string.Empty;

    [HttpGet("resumo")]
    public async Task<ActionResult<DashboardResumoDTO>> ObterResumo()
    {
        var idEmpresa = ObterIdEmpresa();
        var resultado = await _service.ObterResumoAsync(idEmpresa);
        return Ok(resultado);
    }

    [HttpGet("funil")]
    public async Task<ActionResult<List<DashboardFunilDTO>>> ObterFunil()
    {
        var idEmpresa = ObterIdEmpresa();
        var resultado = await _service.ObterFunilAsync(idEmpresa);
        return Ok(resultado);
    }

    [HttpGet("leads-status")]
    public async Task<ActionResult<List<DashboardLeadsPorStatusDTO>>> ObterLeadsPorStatus()
    {
        var idEmpresa = ObterIdEmpresa();
        var resultado = await _service.ObterLeadsPorStatusAsync(idEmpresa);
        return Ok(resultado);
    }

    [HttpGet("atividades-tipo")]
    public async Task<ActionResult<List<DashboardAtividadesDTO>>> ObterAtividadesPorTipo()
    {
        var idEmpresa = ObterIdEmpresa();
        var resultado = await _service.ObterAtividadesPorTipoAsync(idEmpresa);
        return Ok(resultado);
    }

    [HttpGet("leads-recentes")]
    public async Task<ActionResult<List<DashboardLeadRecenteDTO>>> ObterLeadsRecentes([FromQuery] int quantidade = 5)
    {
        var idEmpresa = ObterIdEmpresa();
        var resultado = await _service.ObterLeadsRecentesAsync(idEmpresa, quantidade);
        return Ok(resultado);
    }

    [HttpGet("leads-por-estagio")]
    public async Task<ActionResult<List<DashboardLeadsPorEstagioDTO>>> ObterLeadsPorEstagio()
    {
        var idEmpresa = ObterIdEmpresa();
        var resultado = await _service.ObterLeadsPorEstagioAsync(idEmpresa);
        return Ok(resultado);
    }

    [HttpGet("ticket-medio")]
    public async Task<ActionResult<DashboardMetricaTicketMedioDTO>> ObterTicketMedio()
    {
        var idEmpresa = ObterIdEmpresa();
        var resultado = await _service.ObterTicketMedioAsync(idEmpresa);
        return Ok(resultado);
    }

    [HttpGet("conversao")]
    public async Task<ActionResult<DashboardMetricaConversaoDTO>> ObterMetricasConversao()
    {
        var idEmpresa = ObterIdEmpresa();
        var resultado = await _service.ObterMetricasConversaoAsync(idEmpresa);
        return Ok(resultado);
    }

    [HttpGet("produtividade")]
    public async Task<ActionResult<List<DashboardProdutividadeVendedorDTO>>> ObterProdutividade()
    {
        var idEmpresa = ObterIdEmpresa();
        var resultado = await _service.ObterProdutividadeVendedoresAsync(idEmpresa);
        return Ok(resultado);
    }
}
