using System.Security.Claims;
using KodiakCrm.Core.DTOs;
using KodiakCrm.UseCases.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KodiakCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ParceiroController : ControllerBase
{
    private readonly ParceiroService _service;

    public ParceiroController(ParceiroService service)
    {
        _service = service;
    }

    private string ObterIdEmpresa() => User.FindFirst("id_empresa")?.Value ?? string.Empty;
    private string ObterIdEstabelecimento() => User.FindFirst("id_estabelecimento")?.Value ?? string.Empty;
    private string ObterCnpjEmpresa() => User.FindFirst("cnpj_empresa")?.Value ?? string.Empty;

    [HttpGet]
    public async Task<ActionResult<ParceiroListDTO>> ObterLista([FromQuery] string? busca, [FromQuery] int pagina = 1, [FromQuery] int itensPorPagina = 20)
    {
        var idEmpresa = ObterIdEmpresa();
        var resultado = await _service.ObterListaAsync(idEmpresa, busca, pagina, itensPorPagina);
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParceiroDTO>> ObterPorId(int id)
    {
        var idEmpresa = ObterIdEmpresa();
        var parceiro = await _service.ObterPorIdAsync(id, idEmpresa);

        if (parceiro == null)
            return NotFound(new { mensagem = "Parceiro não encontrado" });

        return Ok(parceiro);
    }

    [HttpPost]
    public async Task<ActionResult<ParceiroDTO>> Criar([FromBody] ParceiroCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var idEmpresa = ObterIdEmpresa();
            var idEstabelecimento = ObterIdEstabelecimento();
            var cnpjEmpresa = ObterCnpjEmpresa();

            var parceiro = await _service.CriarAsync(dto, idEmpresa, idEstabelecimento, cnpjEmpresa);
            return CreatedAtAction(nameof(ObterPorId), new { id = parceiro.Id }, parceiro);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao criar parceiro", erro = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ParceiroDTO>> Atualizar(int id, [FromBody] ParceiroUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var idEmpresa = ObterIdEmpresa();
            var parceiro = await _service.AtualizarAsync(id, dto, idEmpresa);

            if (parceiro == null)
                return NotFound(new { mensagem = "Parceiro não encontrado" });

            return Ok(parceiro);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensagem = ex.Message });
        }
    }

    [HttpPost("sincronizar-erp")]
    [AllowAnonymous]
    public async Task<ActionResult> SincronizarComErp([FromBody] ParceiroSyncDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var resultado = await _service.SincronizarComErpAsync(dto);
        return Ok(new { sucesso = resultado });
    }
}
