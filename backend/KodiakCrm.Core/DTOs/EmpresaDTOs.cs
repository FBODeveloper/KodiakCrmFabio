namespace KodiakCrm.Core.DTOs;

public class EmpresaDTO
{
    public string Cnpj { get; set; } = string.Empty;
    public string RazaoSocial { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public string? Endereco { get; set; }
    public int QuantidadeUsuariosContratados { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
    public int TotalUsuarios { get; set; }
}

public class EmpresaCreateDTO
{
    public string Cnpj { get; set; } = string.Empty;
    public string RazaoSocial { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public string? Endereco { get; set; }
    public int QuantidadeUsuariosContratados { get; set; } = 1;
}

public class EmpresaUpdateDTO
{
    public string RazaoSocial { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public string? Endereco { get; set; }
    public int QuantidadeUsuariosContratados { get; set; }
}

public class EmpresaListDTO
{
    public List<EmpresaDTO> Itens { get; set; } = new();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int ItensPorPagina { get; set; }
}
