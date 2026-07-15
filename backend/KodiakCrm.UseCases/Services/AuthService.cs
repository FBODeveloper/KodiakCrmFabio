using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace KodiakCrm.UseCases.Services;

public class AuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUsuarioRepository usuarioRepository, IEmpresaRepository empresaRepository, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _empresaRepository = empresaRepository;
        _configuration = configuration;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email, request.IdEmpresa);

        if (usuario == null)
        {
            return new LoginResponse
            {
                Sucesso = false,
                Mensagem = "Usuário não encontrado"
            };
        }

        if (!VerificarSenha(request.Senha, usuario.SenhaHash))
        {
            return new LoginResponse
            {
                Sucesso = false,
                Mensagem = "Senha inválida"
            };
        }

        var empresa = await _empresaRepository.ObterPorCnpjAsync(usuario.IdEmpresa);
        var totalUsuarios = await _empresaRepository.ContarUsuariosAsync(usuario.IdEmpresa);

        var token = GerarToken(usuario);

        return new LoginResponse
        {
            Sucesso = true,
            Token = token,
            Mensagem = "Login realizado com sucesso",
            Usuario = new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                IdEmpresa = usuario.IdEmpresa,
                Perfil = usuario.Perfil,
                Avatar = usuario.Avatar,
                DataNascimento = usuario.DataNascimento,
                Empresa = empresa != null ? new EmpresaDTO
                {
                    Cnpj = empresa.Cnpj,
                    RazaoSocial = empresa.RazaoSocial,
                    NomeFantasia = empresa.NomeFantasia,
                    QuantidadeUsuariosContratados = empresa.QuantidadeUsuariosContratados,
                    Ativo = empresa.Ativo,
                    DataCadastro = empresa.DataCadastro,
                    TotalUsuarios = totalUsuarios
                } : null
            }
        };
    }

    public string GerarToken(Usuario usuario)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? "KodiakCrm_SecretKey_Padrao_2024";
        var issuer = jwtSettings["Issuer"] ?? "KodiakCrm";
        var audience = jwtSettings["Audience"] ?? "KodiakCrm";
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim("id_empresa", usuario.IdEmpresa),
            new Claim("id_estabelecimento", usuario.IdEstabelecimento),
            new Claim("cnpj_empresa", usuario.CnpjEmpresa),
            new Claim(ClaimTypes.Role, usuario.Perfil)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string HashSenha(string senha)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
        return Convert.ToBase64String(bytes);
    }

    private static bool VerificarSenha(string senha, string hash)
    {
        var hashCalculado = HashSenha(senha);
        return hashCalculado == hash;
    }
}
