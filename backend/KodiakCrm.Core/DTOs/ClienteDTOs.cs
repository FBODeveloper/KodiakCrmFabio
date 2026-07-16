namespace KodiakCrm.Core.DTOs;

public class ClienteDTO
{
    public int Id { get; set; }
    public string RazaoSocial { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string? CnpjCpf { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public string? Endereco { get; set; }
    public string? Observacao { get; set; }
    public string? Origem { get; set; }
    public DateTime? DataConversao { get; set; }
    public int? IdOportunidade { get; set; }
    public int? ResponsavelId { get; set; }
    public string? ResponsavelNome { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
    public string IdEmpresa { get; set; } = string.Empty;
}

public class ClienteCreateDTO
{
    public string RazaoSocial { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string? CnpjCpf { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public string? Endereco { get; set; }
    public string? Observacao { get; set; }
    public int? ResponsavelId { get; set; }
}

public class ClienteUpdateDTO
{
    public string RazaoSocial { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string? CnpjCpf { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public string? Endereco { get; set; }
    public string? Observacao { get; set; }
    public int? ResponsavelId { get; set; }
}

public class ClienteListDTO
{
    public List<ClienteDTO> Itens { get; set; } = new();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int ItensPorPagina { get; set; }
}
