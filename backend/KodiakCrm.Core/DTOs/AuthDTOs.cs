namespace KodiakCrm.Core.DTOs;

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string IdEmpresa { get; set; } = string.Empty;
}

public class LoginResponse
{
    public bool Sucesso { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public UsuarioDTO? Usuario { get; set; }
}

public class UsuarioDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string IdEmpresa { get; set; } = string.Empty;
    public string Perfil { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public DateTime? DataNascimento { get; set; }
    public EmpresaDTO? Empresa { get; set; }
}
