using System.Security.Claims;
using KodiakCrm.Core.DTOs;
using KodiakCrm.UseCases.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KodiakCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LeadController : ControllerBase
{
    private readonly LeadService _service;

    public LeadController(LeadService service)
    {
        _service = service;
    }

    private string ObterIdEmpresa() => User.FindFirst("id_empresa")?.Value ?? string.Empty;
    private string ObterIdEstabelecimento() => User.FindFirst("id_estabelecimento")?.Value ?? string.Empty;
    private string ObterCnpjEmpresa() => User.FindFirst("cnpj_empresa")?.Value ?? string.Empty;

    [HttpGet]
    public async Task<ActionResult<LeadListDTO>> ObterLista(
        [FromQuery] string? busca,
        [FromQuery] string? status,
        [FromQuery] int pagina = 1,
        [FromQuery] int itensPorPagina = 20)
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var resultado = await _service.ObterListaAsync(idEmpresa, busca, status, pagina, itensPorPagina);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao listar leads", erro = ex.Message });
        }
    }

    [HttpGet("stats")]
    public async Task<ActionResult<LeadStatsDTO>> ObterStats()
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var stats = await _service.ObterStatsAsync(idEmpresa);
            return Ok(stats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao obter stats", erro = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LeadDTO>> ObterPorId(int id)
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var lead = await _service.ObterPorIdAsync(id, idEmpresa);

            if (lead == null)
                return NotFound(new { mensagem = "Lead não encontrado" });

            return Ok(lead);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao obter lead", erro = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<LeadDTO>> Criar([FromBody] LeadCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var idEmpresa = ObterIdEmpresa();
            var idEstabelecimento = ObterIdEstabelecimento();
            var cnpjEmpresa = ObterCnpjEmpresa();

            var lead = await _service.CriarAsync(dto, idEmpresa, idEstabelecimento, cnpjEmpresa);
            return CreatedAtAction(nameof(ObterPorId), new { id = lead.Id }, lead);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao criar lead", erro = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LeadDTO>> Atualizar(int id, [FromBody] LeadUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var idEmpresa = ObterIdEmpresa();
            var lead = await _service.AtualizarAsync(id, dto, idEmpresa);

            if (lead == null)
                return NotFound(new { mensagem = "Lead não encontrado" });

            return Ok(lead);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao atualizar lead", erro = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Excluir(int id)
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var excluido = await _service.ExcluirAsync(id, idEmpresa);

            if (!excluido)
                return NotFound(new { mensagem = "Lead não encontrado" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao excluir lead", erro = ex.Message });
        }
    }

    [HttpPost("{id}/mover")]
    public async Task<ActionResult<LeadDTO>> Mover(int id, [FromBody] LeadMoverDTO dto)
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var lead = await _service.MoverAsync(id, dto, idEmpresa);

            if (lead == null)
                return NotFound(new { mensagem = "Lead não encontrado" });

            return Ok(lead);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao mover lead", erro = ex.Message });
        }
    }

    [HttpGet("kanban")]
    public async Task<ActionResult<LeadKanbanDTO>> ObterKanban()
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var kanban = await _service.ObterKanbanAsync(idEmpresa);
            return Ok(kanban);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao obter kanban", erro = ex.Message });
        }
    }

    [HttpGet("estagios")]
    public async Task<ActionResult<List<LeadEstagioDTO>>> ObterEstagios()
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var estagios = await _service.ObterEstagiosAsync(idEmpresa);
            return Ok(estagios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao obter estágios", erro = ex.Message });
        }
    }

    [HttpPost("estagios")]
    public async Task<ActionResult<LeadEstagioDTO>> CriarEstagio([FromBody] LeadEstagioCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var idEmpresa = ObterIdEmpresa();
            var estagio = await _service.CriarEstagioAsync(dto, idEmpresa);
            return CreatedAtAction(nameof(ObterEstagios), estagio);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao criar estágio", erro = ex.Message });
        }
    }

    [HttpPut("estagios/{id}")]
    public async Task<ActionResult<LeadEstagioDTO>> AtualizarEstagio(int id, [FromBody] LeadEstagioUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var idEmpresa = ObterIdEmpresa();
            var estagio = await _service.AtualizarEstagioAsync(id, dto, idEmpresa);

            if (estagio == null)
                return NotFound(new { mensagem = "Estágio não encontrado" });

            return Ok(estagio);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao atualizar estágio", erro = ex.Message });
        }
    }

    [HttpDelete("estagios/{id}")]
    public async Task<ActionResult> ExcluirEstagio(int id)
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var excluido = await _service.ExcluirEstagioAsync(id, idEmpresa);

            if (!excluido)
                return NotFound(new { mensagem = "Estágio não encontrado" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao excluir estágio", erro = ex.Message });
        }
    }
}
