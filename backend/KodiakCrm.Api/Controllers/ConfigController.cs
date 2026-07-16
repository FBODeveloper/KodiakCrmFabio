using System.Security.Claims;
using KodiakCrm.Core.DTOs;
using KodiakCrm.UseCases.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KodiakCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConfigController : ControllerBase
{
    private readonly ConfigService _configService;
    private readonly EmpresaService _empresaService;
    private readonly HistoricoService _historicoService;

    public ConfigController(ConfigService configService, EmpresaService empresaService, HistoricoService historicoService)
    {
        _configService = configService;
        _empresaService = empresaService;
        _historicoService = historicoService;
    }

    private string ObterIdEmpresa() => User.FindFirst("id_empresa")?.Value ?? string.Empty;
    private string ObterCnpjEmpresa() => User.FindFirst("cnpj_empresa")?.Value ?? string.Empty;
    private int ObterUsuarioId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value;
        return int.TryParse(claim, out var id) ? id : 0;
    }
    private string ObterNomeUsuario() => User.FindFirst("nome")?.Value ?? string.Empty;

    [HttpGet]
    public async Task<ActionResult<EmpresaConfigDTO>> ObterConfiguracoes()
    {
        var cnpj = ObterCnpjEmpresa();
        if (string.IsNullOrEmpty(cnpj))
            return BadRequest(new { mensagem = "Empresa não identificada" });

        var config = await _configService.ObterAsync(cnpj);
        if (config == null)
            return NotFound(new { mensagem = "Configurações não encontradas" });

        return Ok(config);
    }

    [HttpPut]
    public async Task<ActionResult> AtualizarConfiguracoes([FromBody] EmpresaConfigUpdateDTO dto)
    {
        var cnpj = ObterCnpjEmpresa();
        if (string.IsNullOrEmpty(cnpj))
            return BadRequest(new { mensagem = "Empresa não identificada" });

        var (sucesso, mensagem) = await _configService.AtualizarAsync(cnpj, dto);

        if (!sucesso)
            return BadRequest(new { mensagem });

        await _historicoService.RegistrarAsync(
            ObterIdEmpresa(),
            null,
            cnpj,
            "empresa_config",
            0,
            "alterado",
            "Configurações da empresa atualizadas",
            null,
            dto,
            ObterUsuarioId(),
            ObterNomeUsuario()
        );

        return Ok(new { mensagem });
    }

    [HttpGet("empresa")]
    public async Task<ActionResult<EmpresaDTO>> ObterEmpresa()
    {
        var cnpj = ObterCnpjEmpresa();
        if (string.IsNullOrEmpty(cnpj))
            return BadRequest(new { mensagem = "Empresa não identificada" });

        var empresa = await _empresaService.ObterPorCnpjAsync(cnpj);
        if (empresa == null)
            return NotFound(new { mensagem = "Empresa não encontrada" });

        return Ok(empresa);
    }

    [HttpPut("empresa")]
    public async Task<ActionResult> AtualizarEmpresa([FromBody] EmpresaUpdateDTO dto)
    {
        var cnpj = ObterCnpjEmpresa();
        if (string.IsNullOrEmpty(cnpj))
            return BadRequest(new { mensagem = "Empresa não identificada" });

        var (sucesso, mensagem) = await _empresaService.AtualizarAsync(cnpj, dto);

        if (!sucesso)
            return NotFound(new { mensagem });

        await _historicoService.RegistrarAsync(
            ObterIdEmpresa(),
            null,
            cnpj,
            "empresa",
            0,
            "alterado",
            "Dados da empresa atualizados",
            null,
            dto,
            ObterUsuarioId(),
            ObterNomeUsuario()
        );

        return Ok(new { mensagem });
    }
}
