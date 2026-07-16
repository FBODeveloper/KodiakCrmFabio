using System.Security.Claims;
using KodiakCrm.Core.DTOs;
using KodiakCrm.UseCases.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KodiakCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContatoController : ControllerBase
{
    private readonly ContatoService _service;
    private readonly HistoricoService _historico;

    public ContatoController(ContatoService service, HistoricoService historico)
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
    public async Task<ActionResult<ContatoListDTO>> ObterLista(
        [FromQuery] string? busca,
        [FromQuery] int? idCliente,
        [FromQuery] int? idParceiro,
        [FromQuery] int pagina = 1,
        [FromQuery] int itensPorPagina = 20)
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var resultado = await _service.ObterListaAsync(idEmpresa, busca, idCliente, idParceiro, pagina, itensPorPagina);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao listar contatos", erro = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContatoDTO>> ObterPorId(int id)
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var contato = await _service.ObterPorIdAsync(id, idEmpresa);

            if (contato == null)
                return NotFound(new { mensagem = "Contato não encontrado" });

            return Ok(contato);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao obter contato", erro = ex.Message });
        }
    }

    [HttpGet("cliente/{idCliente}")]
    public async Task<ActionResult<List<ContatoDTO>>> ObterPorCliente(int idCliente)
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var contatos = await _service.ObterPorClienteAsync(idCliente, idEmpresa);
            return Ok(contatos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao obter contatos", erro = ex.Message });
        }
    }

    [HttpGet("parceiro/{idParceiro}")]
    public async Task<ActionResult<List<ContatoDTO>>> ObterPorParceiro(int idParceiro)
    {
        try
        {
            var idEmpresa = ObterIdEmpresa();
            var contatos = await _service.ObterPorParceiroAsync(idParceiro, idEmpresa);
            return Ok(contatos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao obter contatos", erro = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ContatoDTO>> Criar([FromBody] ContatoCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var idEmpresa = ObterIdEmpresa();
            var contato = await _service.CriarAsync(dto, idEmpresa, ObterIdEstabelecimento(), ObterCnpjEmpresa());

            _ = Task.Run(async () =>
            {
                try
                {
                    await _historico.RegistrarAsync(
                        idEmpresa, ObterIdEstabelecimento(), ObterCnpjEmpresa(),
                        "contato", contato.Id, "criado",
                        $"Contato \"{contato.Nome}\" criado",
                        dadosDepois: new { contato.Nome, contato.Cargo, contato.Email },
                        usuarioId: ObterUsuarioId(),
                        usuarioNome: ObterUsuarioNome());
                }
                catch { }
            });

            return CreatedAtAction(nameof(ObterPorId), new { id = contato.Id }, contato);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao criar contato", erro = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ContatoDTO>> Atualizar(int id, [FromBody] ContatoUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var idEmpresa = ObterIdEmpresa();
            var contato = await _service.AtualizarAsync(id, dto, idEmpresa);

            if (contato == null)
                return NotFound(new { mensagem = "Contato não encontrado" });

            return Ok(contato);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao atualizar contato", erro = ex.Message });
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
                return NotFound(new { mensagem = "Contato não encontrado" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao excluir contato", erro = ex.Message });
        }
    }
}
