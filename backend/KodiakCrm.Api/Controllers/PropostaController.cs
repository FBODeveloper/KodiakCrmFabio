using System.Security.Claims;
using KodiakCrm.Core.DTOs;
using KodiakCrm.UseCases.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KodiakCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PropostaController : ControllerBase
{
    private readonly PropostaService _service;

    public PropostaController(PropostaService service)
    {
        _service = service;
    }

    private string ObterIdEmpresa() => User.FindFirst("id_empresa")?.Value ?? string.Empty;
    private string ObterIdEstabelecimento() => User.FindFirst("id_estabelecimento")?.Value ?? string.Empty;
    private string ObterCnpjEmpresa() => User.FindFirst("cnpj_empresa")?.Value ?? string.Empty;

    [HttpGet]
    public async Task<ActionResult<PropostaListDTO>> ObterLista(
        [FromQuery] string? busca,
        [FromQuery] string? status,
        [FromQuery] int? idParceiro,
        [FromQuery] int pagina = 1,
        [FromQuery] int itensPorPagina = 20)
    {
        var idEmpresa = ObterIdEmpresa();
        var resultado = await _service.ObterListaAsync(idEmpresa, busca, status, idParceiro, pagina, itensPorPagina);
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PropostaDTO>> ObterPorId(int id)
    {
        var idEmpresa = ObterIdEmpresa();
        var proposta = await _service.ObterPorIdAsync(id, idEmpresa);

        if (proposta == null)
            return NotFound(new { mensagem = "Proposta não encontrada" });

        return Ok(proposta);
    }

    [HttpPost]
    public async Task<ActionResult<PropostaDTO>> Criar([FromBody] PropostaCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var idEmpresa = ObterIdEmpresa();
        var idEstabelecimento = ObterIdEstabelecimento();
        var cnpjEmpresa = ObterCnpjEmpresa();

        try
        {
            var proposta = await _service.CriarAsync(dto, idEmpresa, idEstabelecimento, cnpjEmpresa);
            return CreatedAtAction(nameof(ObterPorId), new { id = proposta.Id }, proposta);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao criar proposta", erro = ex.Message, detalhes = ex.InnerException?.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PropostaDTO>> Atualizar(int id, [FromBody] PropostaUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var idEmpresa = ObterIdEmpresa();
        var proposta = await _service.AtualizarAsync(id, dto, idEmpresa);

        if (proposta == null)
            return NotFound(new { mensagem = "Proposta não encontrada" });

        return Ok(proposta);
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<PropostaDTO>> AlterarStatus(int id, [FromBody] AlterarStatusDTO dto)
    {
        var idEmpresa = ObterIdEmpresa();
        var proposta = await _service.AlterarStatusAsync(id, dto.Status, idEmpresa);

        if (proposta == null)
            return NotFound(new { mensagem = "Proposta não encontrada" });

        return Ok(proposta);
    }
}

public class AlterarStatusDTO
{
    public string Status { get; set; } = string.Empty;
}
