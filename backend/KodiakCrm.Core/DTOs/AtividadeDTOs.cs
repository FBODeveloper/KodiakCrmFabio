namespace KodiakCrm.Core.DTOs;

public class AtividadeDTO
{
    public int Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public int? IdParceiro { get; set; }
    public string? ParceiroNome { get; set; }
    public int? IdOportunidade { get; set; }
    public string? OportunidadeTitulo { get; set; }
    public int? ResponsavelId { get; set; }
    public string? ResponsavelNome { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public bool Concluida { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
    public string IdEmpresa { get; set; } = string.Empty;
}

public class AtividadeCreateDTO
{
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public int? IdParceiro { get; set; }
    public int? IdOportunidade { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
}

public class AtividadeUpdateDTO
{
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public int? IdParceiro { get; set; }
    public int? IdOportunidade { get; set; }
    public int? ResponsavelId { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public bool? Concluida { get; set; }
}

public class AtividadeListDTO
{
    public List<AtividadeDTO> Itens { get; set; } = new();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int ItensPorPagina { get; set; }
}
