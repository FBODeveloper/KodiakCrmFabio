using System.Security.Claims;
using KodiakCrm.Core.DTOs;
using KodiakCrm.UseCases.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KodiakCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClienteController : ControllerBase
{
    private readonly ClienteService _service;
    private readonly HistoricoService _historico;

    public ClienteController(ClienteService service, HistoricoService historico)
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
    public async Task<ActionResult<ClienteListDTO>> ObterLista(
        [FromQuery] string? busca,
        [FromQuery] string? origem,
        [FromQuery] DateTime? dataInicio,
        [FromQuery] DateTime? dataFim,
        [FromQuery] int pagina = 1,
        [FromQuery] int itensPorPagina = 20)
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var resultado = await _service.ObterListaAsync(idEmpresa, busca, origem, dataInicio, dataFim, pagina, itensPorPagina);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao listar clientes", erro = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDTO>> ObterPorId(int id)
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var cliente = await _service.ObterPorIdAsync(id, idEmpresa);

            if (cliente == null)
                return NotFound(new { mensagem = "Cliente não encontrado" });

            return Ok(cliente);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao obter cliente", erro = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ClienteDTO>> Criar([FromBody] ClienteCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var idEmpresa = ObterIdEmpresa();
            var cliente = await _service.CriarAsync(dto, idEmpresa, ObterIdEstabelecimento(), ObterCnpjEmpresa());

            _ = Task.Run(async () =>
            {
                try
                {
                    await _historico.RegistrarAsync(
                        idEmpresa, ObterIdEstabelecimento(), ObterCnpjEmpresa(),
                        "cliente", cliente.Id, "criado",
                        $"Cliente \"{cliente.RazaoSocial}\" criado",
                        dadosDepois: new { cliente.RazaoSocial, cliente.CnpjCpf, cliente.Email },
                        usuarioId: ObterUsuarioId(),
                        usuarioNome: ObterUsuarioNome());
                }
                catch { }
            });

            return CreatedAtAction(nameof(ObterPorId), new { id = cliente.Id }, cliente);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao criar cliente", erro = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClienteDTO>> Atualizar(int id, [FromBody] ClienteUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var idEmpresa = ObterIdEmpresa();
            var cliente = await _service.AtualizarAsync(id, dto, idEmpresa);

            if (cliente == null)
                return NotFound(new { mensagem = "Cliente não encontrado" });

            return Ok(cliente);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao atualizar cliente", erro = ex.Message });
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
                return NotFound(new { mensagem = "Cliente não encontrado" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao excluir cliente", erro = ex.Message });
        }
    }
}
