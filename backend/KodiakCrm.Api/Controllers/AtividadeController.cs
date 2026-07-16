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
    private readonly HistoricoService _historico;

    public AtividadeController(AtividadeService service, HistoricoService historico)
    {
        _service = service;
        _historico = historico;
    }

    private string ObterIdEmpresa() => User.FindFirst("id_empresa")?.Value ?? string.Empty;
    private string ObterIdEstabelecimento() => User.FindFirst("id_estabelecimento")?.Value ?? string.Empty;
    private string ObterCnpjEmpresa() => User.FindFirst("cnpj_empresa")?.Value ?? string.Empty;
    private int? ObterResponsavelId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }
    private string ObterUsuarioNome() => User.FindFirst("nome")?.Value ?? string.Empty;

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

            _ = Task.Run(async () =>
            {
                try
                {
                    await _historico.RegistrarAsync(
                        idEmpresa, idEstabelecimento, cnpjEmpresa,
                        "atividade", atividade.Id, "criado",
                        $"Atividade \"{atividade.Titulo}\" criada",
                        dadosDepois: new { atividade.Titulo, atividade.Tipo, atividade.DataInicio },
                        usuarioId: responsavelId,
                        usuarioNome: ObterUsuarioNome());
                }
                catch { }
            });

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

        _ = Task.Run(async () =>
        {
            try
            {
                await _historico.RegistrarAsync(
                    idEmpresa, ObterIdEstabelecimento(), ObterCnpjEmpresa(),
                    "atividade", id, "alterado",
                    $"Atividade \"{atividade.Titulo}\" atualizada",
                    dadosDepois: new { atividade.Titulo, atividade.Tipo, atividade.Concluida },
                    usuarioId: ObterResponsavelId(),
                    usuarioNome: ObterUsuarioNome());
            }
            catch { }
        });

        return Ok(atividade);
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<AtividadeDTO>> AlterarStatus(int id, [FromBody] AlterarStatusAtividadeDTO dto)
    {
        var statusPermitidos = new[] { "pendente", "concluido", "cancelado" };
        if (!statusPermitidos.Contains(dto.Status))
            return BadRequest(new { mensagem = "Status inválido. Valores permitidos: pendente, concluido, cancelado" });

        var idEmpresa = ObterIdEmpresa();
        var atividadeAtual = await _service.ObterPorIdAsync(id, idEmpresa);
        if (atividadeAtual == null)
            return NotFound(new { mensagem = "Atividade não encontrada" });

        if (atividadeAtual.Status != "pendente")
            return BadRequest(new { mensagem = "Só é possível alterar status de atividades com status 'pendente'" });

        if (dto.Status != "concluido" && dto.Status != "cancelado")
            return BadRequest(new { mensagem = "Só é possível alterar para 'concluido' ou 'cancelado'" });

        var atividade = await _service.AlterarStatusAsync(id, dto.Status, idEmpresa);

        _ = Task.Run(async () =>
        {
            try
            {
                await _historico.RegistrarAsync(
                    idEmpresa, ObterIdEstabelecimento(), ObterCnpjEmpresa(),
                    "atividade", id, "status_alterado",
                    $"Atividade \"{atividade!.Titulo}\" status alterado para \"{dto.Status}\"",
                    dadosAntes: new { Status = atividadeAtual.Status },
                    dadosDepois: new { Status = dto.Status },
                    usuarioId: ObterResponsavelId(),
                    usuarioNome: ObterUsuarioNome());
            }
            catch { }
        });

        return Ok(atividade);
    }
}
