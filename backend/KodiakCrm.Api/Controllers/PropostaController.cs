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
    private readonly HistoricoService _historico;

    public PropostaController(PropostaService service, HistoricoService historico)
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
    public async Task<ActionResult<PropostaListDTO>> ObterLista(
        [FromQuery] string? busca,
        [FromQuery] string? status,
        [FromQuery] int? idParceiro,
        [FromQuery] int? clienteId,
        [FromQuery] DateTime? dataInicio,
        [FromQuery] DateTime? dataFim,
        [FromQuery] int pagina = 1,
        [FromQuery] int itensPorPagina = 20)
    {
        var idEmpresa = ObterIdEmpresa();
        var resultado = await _service.ObterListaAsync(idEmpresa, busca, status, idParceiro, clienteId, dataInicio, dataFim, pagina, itensPorPagina);
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

            _ = Task.Run(async () =>
            {
                try
                {
                    await _historico.RegistrarAsync(
                        idEmpresa, idEstabelecimento, cnpjEmpresa,
                        "proposta", proposta.Id, "criado",
                        $"Proposta \"{proposta.Numero}\" criada",
                        dadosDepois: new { proposta.Numero, proposta.Status, proposta.ValorTotal },
                        usuarioId: ObterUsuarioId(),
                        usuarioNome: ObterUsuarioNome());
                }
                catch { }
            });

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

        _ = Task.Run(async () =>
        {
            try
            {
                await _historico.RegistrarAsync(
                    idEmpresa, ObterIdEstabelecimento(), ObterCnpjEmpresa(),
                    "proposta", id, "alterado",
                    $"Proposta \"{proposta.Numero}\" atualizada",
                    dadosDepois: new { proposta.Numero, proposta.Status, proposta.ValorTotal },
                    usuarioId: ObterUsuarioId(),
                    usuarioNome: ObterUsuarioNome());
            }
            catch { }
        });

        return Ok(proposta);
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<PropostaDTO>> AlterarStatus(int id, [FromBody] AlterarStatusPropostaDTO dto)
    {
        var idEmpresa = ObterIdEmpresa();
        var proposta = await _service.AlterarStatusAsync(id, dto.Status, dto.MotivoRejeicao, idEmpresa);

        if (proposta == null)
            return NotFound(new { mensagem = "Proposta não encontrada" });

        _ = Task.Run(async () =>
        {
            try
            {
                var descricao = $"Proposta \"{proposta.Numero}\" status alterado para \"{dto.Status}\"";
                if (!string.IsNullOrEmpty(dto.MotivoRejeicao))
                    descricao += $" - Motivo: {dto.MotivoRejeicao}";

                await _historico.RegistrarAsync(
                    idEmpresa, ObterIdEstabelecimento(), ObterCnpjEmpresa(),
                    "proposta", id, "status_alterada",
                    descricao,
                    dadosDepois: new { proposta.Status, dto.MotivoRejeicao },
                    usuarioId: ObterUsuarioId(),
                    usuarioNome: ObterUsuarioNome());
            }
            catch { }
        });

        return Ok(proposta);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Excluir(int id)
    {
        var idEmpresa = ObterIdEmpresa();
        var proposta = await _service.ObterPorIdAsync(id, idEmpresa);
        var excluido = await _service.ExcluirAsync(id, idEmpresa);

        if (!excluido)
            return NotFound(new { mensagem = "Proposta não encontrada" });

        if (proposta != null)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await _historico.RegistrarAsync(
                        idEmpresa, ObterIdEstabelecimento(), ObterCnpjEmpresa(),
                        "proposta", id, "excluido",
                        $"Proposta \"{proposta.Numero}\" excluída",
                        dadosAntes: new { proposta.Numero, proposta.Status, proposta.ValorTotal },
                        usuarioId: ObterUsuarioId(),
                        usuarioNome: ObterUsuarioNome());
                }
                catch { }
            });
        }

        return NoContent();
    }
}
