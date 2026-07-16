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
    private readonly HistoricoService _historico;

    public LeadController(LeadService service, HistoricoService historico)
    {
        _service = service;
        _historico = historico;
    }

    private string ObterIdEmpresa() => User.FindFirst("id_empresa")?.Value ?? string.Empty;
    private string ObterIdEstabelecimento() => User.FindFirst("id_estabelecimento")?.Value ?? string.Empty;
    private string ObterCnpjEmpresa() => User.FindFirst("cnpj_empresa")?.Value ?? string.Empty;
    private int? ObterUsuarioId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }
    private string ObterUsuarioNome() => User.FindFirst("nome")?.Value ?? string.Empty;

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

            _ = Task.Run(async () =>
            {
                try
                {
                    await _historico.RegistrarAsync(
                        idEmpresa, idEstabelecimento, cnpjEmpresa,
                        "lead", lead.Id, "criado",
                        $"Lead \"{lead.Nome}\" criado",
                        dadosDepois: new { lead.Nome, lead.Email, lead.Telefone, lead.Empresa, lead.Temperatura },
                        usuarioId: ObterUsuarioId(),
                        usuarioNome: ObterUsuarioNome());
                }
                catch { }
            });

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
            var leadAntes = await _service.ObterPorIdAsync(id, idEmpresa);

            var lead = await _service.AtualizarAsync(id, dto, idEmpresa);

            if (lead == null)
                return NotFound(new { mensagem = "Lead não encontrado" });

            _ = Task.Run(async () =>
            {
                try
                {
                    await _historico.RegistrarAsync(
                        idEmpresa, ObterIdEstabelecimento(), ObterCnpjEmpresa(),
                        "lead", id, "alterado",
                        $"Lead \"{lead.Nome}\" atualizado",
                        dadosAntes: leadAntes != null ? new { leadAntes.Nome, leadAntes.Status, leadAntes.Temperatura } : null,
                        dadosDepois: new { lead.Nome, lead.Status, lead.Temperatura },
                        usuarioId: ObterUsuarioId(),
                        usuarioNome: ObterUsuarioNome());
                }
                catch { }
            });

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
            var lead = await _service.ObterPorIdAsync(id, idEmpresa);

            var excluido = await _service.ExcluirAsync(id, idEmpresa);

            if (!excluido)
                return NotFound(new { mensagem = "Lead não encontrado" });

            if (lead != null)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _historico.RegistrarAsync(
                            idEmpresa, ObterIdEstabelecimento(), ObterCnpjEmpresa(),
                            "lead", id, "excluido",
                            $"Lead \"{lead.Nome}\" excluído",
                            dadosAntes: new { lead.Nome, lead.Email, lead.Empresa, lead.Status },
                            usuarioId: ObterUsuarioId(),
                            usuarioNome: ObterUsuarioNome());
                    }
                    catch { }
                });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao excluir lead", erro = ex.Message });
        }
    }

    [HttpPost("{id}/converter")]
    public async Task<ActionResult<LeadConverterResponseDTO>> Converter(int id, [FromBody] LeadConverterDTO dto)
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var resultado = await _service.ConverterAsync(id, dto, idEmpresa, ObterIdEstabelecimento(), ObterCnpjEmpresa());

            if (resultado == null)
                return NotFound(new { mensagem = "Lead não encontrado ou já convertido" });

            _ = Task.Run(async () =>
            {
                try
                {
                    await _historico.RegistrarAsync(
                        idEmpresa, ObterIdEstabelecimento(), ObterCnpjEmpresa(),
                        "lead", id, "convertido",
                        $"Lead \"{resultado.LeadNome}\" convertido em oportunidade \"{resultado.OportunidadeTitulo}\"",
                        dadosDepois: new { resultado.LeadId, resultado.OportunidadeId, resultado.OportunidadeTitulo },
                        usuarioId: ObterUsuarioId(),
                        usuarioNome: ObterUsuarioNome());

                    await _historico.RegistrarAsync(
                        idEmpresa, ObterIdEstabelecimento(), ObterCnpjEmpresa(),
                        "oportunidade", resultado.OportunidadeId, "criado",
                        $"Oportunidade \"{resultado.OportunidadeTitulo}\" criada a partir do lead \"{resultado.LeadNome}\"",
                        dadosDepois: new { resultado.OportunidadeTitulo, resultado.OportunidadeId },
                        usuarioId: ObterUsuarioId(),
                        usuarioNome: ObterUsuarioNome());
                }
                catch { }
            });

            return Ok(resultado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao converter lead", erro = ex.Message });
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
