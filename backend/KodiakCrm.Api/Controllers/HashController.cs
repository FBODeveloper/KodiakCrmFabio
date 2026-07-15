using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace KodiakCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HashController : ControllerBase
{
    [HttpPost("senha")]
    public ActionResult<string> GerarHashSenha([FromBody] HashRequest request)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(request.Senha));
        var hash = Convert.ToBase64String(bytes);
        return Ok(hash);
    }
}

public class HashRequest
{
    public string Senha { get; set; } = string.Empty;
}
