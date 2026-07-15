using KodiakCrm.Core.DTOs;
using KodiakCrm.UseCases.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KodiakCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmpresaController : ControllerBase
{
    private readonly EmpresaService _empresaService;

    public EmpresaController(EmpresaService empresaService)
    {
        _empresaService = empresaService;
    }

    [HttpGet]
    public async Task<ActionResult<EmpresaListDTO>> ObterLista([FromQuery] string? busca, [FromQuery] int pagina = 1, [FromQuery] int itensPorPagina = 20)
    {
        var resultado = await _empresaService.ObterListaAsync(busca, pagina, itensPorPagina);
        return Ok(resultado);
    }

    [HttpGet("{cnpj}")]
    public async Task<ActionResult<EmpresaDTO>> ObterPorCnpj(string cnpj)
    {
        var empresa = await _empresaService.ObterPorCnpjAsync(cnpj);
        if (empresa == null)
        {
            return NotFound(new { mensagem = "Empresa não encontrada" });
        }

        return Ok(empresa);
    }

    [HttpPost]
    public async Task<ActionResult> Criar([FromBody] EmpresaCreateDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { mensagem = "Dados inválidos" });
        }

        try
        {
            var (sucesso, mensagem, empresa) = await _empresaService.CriarAsync(dto);

            if (!sucesso)
            {
                return BadRequest(new { mensagem });
            }

            return CreatedAtAction(nameof(ObterPorCnpj), new { cnpj = empresa!.Cnpj }, empresa);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao criar empresa", erro = ex.Message });
        }
    }

    [HttpPut("{cnpj}")]
    public async Task<ActionResult> Atualizar(string cnpj, [FromBody] EmpresaUpdateDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { mensagem = "Dados inválidos" });
        }

        var (sucesso, mensagem) = await _empresaService.AtualizarAsync(cnpj, dto);

        if (!sucesso)
        {
            return NotFound(new { mensagem });
        }

        return Ok(new { mensagem });
    }
}
