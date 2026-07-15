namespace KodiakCrm.Core.DTOs;

public class ParceiroDTO
{
    public int Id { get; set; }
    public string RazaoSocial { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string? CpfCnpj { get; set; }
    public string? TipoPessoa { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public int? IdParceiroKodiakErp { get; set; }
    public string? Observacao { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
    public string IdEmpresa { get; set; } = string.Empty;
}

public class ParceiroCreateDTO
{
    public string RazaoSocial { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string? CpfCnpj { get; set; }
    public string? TipoPessoa { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public string? Observacao { get; set; }
}

public class ParceiroUpdateDTO
{
    public string RazaoSocial { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string? CpfCnpj { get; set; }
    public string? TipoPessoa { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public string? Observacao { get; set; }
}

public class ParceiroSyncDTO
{
    public int IdParceiroKodiakErp { get; set; }
    public string RazaoSocial { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string? CpfCnpj { get; set; }
    public string? TipoPessoa { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public string IdEmpresa { get; set; } = string.Empty;
    public string IdEstabelecimento { get; set; } = string.Empty;
    public string CnpjEmpresa { get; set; } = string.Empty;
}

public class ParceiroListDTO
{
    public List<ParceiroDTO> Itens { get; set; } = new();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int ItensPorPagina { get; set; }
}
