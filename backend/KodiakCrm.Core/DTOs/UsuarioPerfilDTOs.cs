namespace KodiakCrm.Core.DTOs;

public class UsuarioPerfilDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string Perfil { get; set; } = string.Empty;
    public string? EmpresaNome { get; set; }
    public string? EmpresaCnpj { get; set; }
}

public class UsuarioPerfilUpdateDTO
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Avatar { get; set; }
    public DateTime? DataNascimento { get; set; }
}

public class UsuarioAlterarSenhaDTO
{
    public string SenhaAtual { get; set; } = string.Empty;
    public string NovaSenha { get; set; } = string.Empty;
    public string ConfirmarSenha { get; set; } = string.Empty;
}
