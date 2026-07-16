namespace KodiakCrm.Core.DTOs;

public class ContatoDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Cargo { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public int? IdCliente { get; set; }
    public string? ClienteNome { get; set; }
    public int? IdParceiro { get; set; }
    public string? ParceiroNome { get; set; }
    public string? Observacao { get; set; }
    public int? ResponsavelId { get; set; }
    public string? ResponsavelNome { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
    public string IdEmpresa { get; set; } = string.Empty;
}

public class ContatoCreateDTO
{
    public string Nome { get; set; } = string.Empty;
    public string? Cargo { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public int? IdCliente { get; set; }
    public int? IdParceiro { get; set; }
    public string? Observacao { get; set; }
    public int? ResponsavelId { get; set; }
}

public class ContatoUpdateDTO
{
    public string Nome { get; set; } = string.Empty;
    public string? Cargo { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public int? IdCliente { get; set; }
    public int? IdParceiro { get; set; }
    public string? Observacao { get; set; }
    public int? ResponsavelId { get; set; }
}

public class ContatoListDTO
{
    public List<ContatoDTO> Itens { get; set; } = new();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int ItensPorPagina { get; set; }
}
