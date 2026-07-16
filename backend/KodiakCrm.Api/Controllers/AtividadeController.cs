using System.Security.Claims;
using KodiakCrm.Core.DTOs;
using KodiakCrm.UseCases.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KodiakCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AtividadeController : ControllerBase
{
    private readonly AtividadeService _service;

    public AtividadeController(AtividadeService service)
    {
        _service = service;
    }

    private string ObterIdEmpresa() => User.FindFirst("id_empresa")?.Value ?? string.Empty;
    private string ObterIdEstabelecimento() => User.FindFirst("id_estabelecimento")?.Value ?? string.Empty;
    private string ObterCnpjEmpresa() => User.FindFirst("cnpj_empresa")?.Value ?? string.Empty;
    private int? ObterResponsavelId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    [HttpGet]
    public async Task<ActionResult<AtividadeListDTO>> ObterLista(
        [FromQuery] string? busca,
        [FromQuery] string? tipo,
        [FromQuery] int? idParceiro,
        [FromQuery] int? idOportunidade,
        [FromQuery] int? responsavelId,
        [FromQuery] bool? concluida,
        [FromQuery] DateTime? dataInicio,
        [FromQuery] DateTime? dataFim,
        [FromQuery] int pagina = 1,
        [FromQuery] int itensPorPagina = 20)
    {
        var idEmpresa = ObterIdEmpresa();
        var resultado = await _service.ObterListaAsync(idEmpresa, busca, tipo, idParceiro, idOportunidade, responsavelId, concluida, dataInicio, dataFim, pagina, itensPorPagina);
        return Ok(resultado);
    }

    [HttpGet("parceiro/{idParceiro}")]
    public async Task<ActionResult<List<AtividadeDTO>>> ObterPorParceiro(int idParceiro)
    {
        var idEmpresa = ObterIdEmpresa();
        var resultado = await _service.ObterPorParceiroAsync(idParceiro, idEmpresa);
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AtividadeDTO>> ObterPorId(int id)
    {
        var idEmpresa = ObterIdEmpresa();
        var atividade = await _service.ObterPorIdAsync(id, idEmpresa);

        if (atividade == null)
            return NotFound(new { mensagem = "Atividade não encontrada" });

        return Ok(atividade);
    }

    [HttpPost]
    public async Task<ActionResult<AtividadeDTO>> Criar([FromBody] AtividadeCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var idEmpresa = ObterIdEmpresa();
        var idEstabelecimento = ObterIdEstabelecimento();
        var cnpjEmpresa = ObterCnpjEmpresa();
        var responsavelId = ObterResponsavelId();

        try
        {
            var atividade = await _service.CriarAsync(dto, idEmpresa, idEstabelecimento, cnpjEmpresa, responsavelId);
            return CreatedAtAction(nameof(ObterPorId), new { id = atividade.Id }, atividade);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao criar atividade", erro = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AtividadeDTO>> Atualizar(int id, [FromBody] AtividadeUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var idEmpresa = ObterIdEmpresa();
        var atividade = await _service.AtualizarAsync(id, dto, idEmpresa);

        if (atividade == null)
            return NotFound(new { mensagem = "Atividade não encontrada" });

        return Ok(atividade);
    }
}
