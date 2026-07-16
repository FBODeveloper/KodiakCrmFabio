using System.Security.Claims;
using KodiakCrm.Core.DTOs;
using KodiakCrm.UseCases.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KodiakCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FunilController : ControllerBase
{
    private readonly FunilService _service;

    public FunilController(FunilService service)
    {
        _service = service;
    }

    private string ObterIdEmpresa() => User.FindFirst("id_empresa")?.Value ?? string.Empty;

    [HttpGet]
    public async Task<ActionResult<List<FunilDTO>>> ObterLista()
    {
        var idEmpresa = ObterIdEmpresa();
        var resultado = await _service.ObterListaAsync(idEmpresa);
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FunilDTO>> ObterPorId(int id)
    {
        var idEmpresa = ObterIdEmpresa();
        var funil = await _service.ObterPorIdAsync(id, idEmpresa);

        if (funil == null)
            return NotFound(new { mensagem = "Funil não encontrado" });

        return Ok(funil);
    }

    [HttpPost]
    public async Task<ActionResult<FunilDTO>> Criar([FromBody] FunilCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var idEmpresa = ObterIdEmpresa();
            var funil = await _service.CriarAsync(dto, idEmpresa);
            return CreatedAtAction(nameof(ObterPorId), new { id = funil.Id }, funil);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao criar funil", erro = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Excluir(int id)
    {
        var idEmpresa = ObterIdEmpresa();
        await _service.ExcluirAsync(id, idEmpresa);
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OportunidadeController : ControllerBase
{
    private readonly OportunidadeService _service;
    private readonly HistoricoService _historico;

    public OportunidadeController(OportunidadeService service, HistoricoService historico)
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
    public async Task<ActionResult<OportunidadeListDTO>> ObterLista(
        [FromQuery] string? busca,
        [FromQuery] int? idEstagio,
        [FromQuery] int? responsavelId,
        [FromQuery] string? status,
        [FromQuery] DateTime? dataInicio,
        [FromQuery] DateTime? dataFim,
        [FromQuery] int pagina = 1,
        [FromQuery] int itensPorPagina = 20)
    {
        var idEmpresa = ObterIdEmpresa();
        var resultado = await _service.ObterListaAsync(idEmpresa, busca, idEstagio, responsavelId, status, dataInicio, dataFim, pagina, itensPorPagina);
        return Ok(resultado);
    }

    [HttpGet("kanban/{funilId}")]
    public async Task<ActionResult<KanbanDTO>> ObterKanban(int funilId)
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var kanban = await _service.ObterKanbanAsync(funilId, idEmpresa);
            return Ok(kanban);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OportunidadeDTO>> ObterPorId(int id)
    {
        var idEmpresa = ObterIdEmpresa();
        var oportunidade = await _service.ObterPorIdAsync(id, idEmpresa);

        if (oportunidade == null)
            return NotFound(new { mensagem = "Oportunidade não encontrada" });

        return Ok(oportunidade);
    }

    [HttpPost]
    public async Task<ActionResult<OportunidadeDTO>> Criar([FromBody] OportunidadeCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var idEmpresa = ObterIdEmpresa();
        var idEstabelecimento = ObterIdEstabelecimento();
        var cnpjEmpresa = ObterCnpjEmpresa();

        try
        {
            var oportunidade = await _service.CriarAsync(dto, idEmpresa, idEstabelecimento, cnpjEmpresa);

            _ = Task.Run(async () =>
            {
                try
                {
                    await _historico.RegistrarAsync(
                        idEmpresa, idEstabelecimento, cnpjEmpresa,
                        "oportunidade", oportunidade.Id, "criado",
                        $"Oportunidade \"{oportunidade.Titulo}\" criada",
                        dadosDepois: new { oportunidade.Titulo, oportunidade.Valor },
                        usuarioId: ObterUsuarioId(),
                        usuarioNome: ObterUsuarioNome());
                }
                catch { }
            });

            return CreatedAtAction(nameof(ObterPorId), new { id = oportunidade.Id }, oportunidade);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao criar oportunidade", erro = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OportunidadeDTO>> Atualizar(int id, [FromBody] OportunidadeUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var idEmpresa = ObterIdEmpresa();
        var oportunidade = await _service.AtualizarAsync(id, dto, idEmpresa);

        if (oportunidade == null)
            return NotFound(new { mensagem = "Oportunidade não encontrada" });

        _ = Task.Run(async () =>
        {
            try
            {
                await _historico.RegistrarAsync(
                    idEmpresa, ObterIdEstabelecimento(), ObterCnpjEmpresa(),
                    "oportunidade", id, "alterado",
                    $"Oportunidade \"{oportunidade.Titulo}\" atualizada",
                    dadosDepois: new { oportunidade.Titulo, oportunidade.Valor },
                    usuarioId: ObterUsuarioId(),
                    usuarioNome: ObterUsuarioNome());
            }
            catch { }
        });

        return Ok(oportunidade);
    }

    [HttpPut("{id}/mover")]
    public async Task<ActionResult<OportunidadeDTO>> Mover(int id, [FromBody] OportunidadeMoverDTO dto)
    {
        var idEmpresa = ObterIdEmpresa();
        var oportunidade = await _service.MoverAsync(id, dto, idEmpresa);

        if (oportunidade == null)
            return NotFound(new { mensagem = "Oportunidade não encontrada" });

        _ = Task.Run(async () =>
        {
            try
            {
                await _historico.RegistrarAsync(
                    idEmpresa, ObterIdEstabelecimento(), ObterCnpjEmpresa(),
                    "oportunidade", id, "etapa_alterada",
                    $"Oportunidade \"{oportunidade.Titulo}\" movida para etapa {oportunidade.EstagioNome}",
                    dadosDepois: new { oportunidade.IdEstagio, oportunidade.EstagioNome },
                    usuarioId: ObterUsuarioId(),
                    usuarioNome: ObterUsuarioNome());
            }
            catch { }
        });

        return Ok(oportunidade);
    }
}
