using KodiakCrm.Core.DTOs;
using KodiakCrm.UseCases.Services;
using Microsoft.AspNetCore.Mvc;

namespace KodiakCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new LoginResponse
            {
                Sucesso = false,
                Mensagem = "Dados inválidos"
            });
        }

        var resultado = await _authService.LoginAsync(request);

        if (!resultado.Sucesso)
        {
            return Unauthorized(resultado);
        }

        return Ok(resultado);
    }
}
