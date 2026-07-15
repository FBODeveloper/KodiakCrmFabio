namespace KodiakCrm.Core.DTOs;

public class UsuarioGestaoDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Perfil { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public DateTime? DataNascimento { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
}

public class UsuarioCreateDTO
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Perfil { get; set; } = "usuario";
    public string? Avatar { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string? IdEmpresa { get; set; }
}

public class UsuarioUpdateDTO
{
    public string Nome { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Senha { get; set; }
    public string Perfil { get; set; } = "usuario";
    public string? Avatar { get; set; }
    public DateTime? DataNascimento { get; set; }
    public bool? Ativo { get; set; }
}

public class UsuarioGestaoListDTO
{
    public List<UsuarioGestaoDTO> Itens { get; set; } = new();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int ItensPorPagina { get; set; }
}
