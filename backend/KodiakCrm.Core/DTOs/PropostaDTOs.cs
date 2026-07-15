namespace KodiakCrm.Core.DTOs;

public class PropostaDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int? IdParceiro { get; set; }
    public string? ParceiroNome { get; set; }
    public int? IdOportunidade { get; set; }
    public string? OportunidadeTitulo { get; set; }
    public decimal? ValorTotal { get; set; }
    public DateTime? DataValidade { get; set; }
    public string Status { get; set; } = "rascunho";
    public string? Observacao { get; set; }
    public List<PropostaItemDTO> Itens { get; set; } = new();
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
    public string IdEmpresa { get; set; } = string.Empty;
}

public class PropostaCreateDTO
{
    public string Titulo { get; set; } = string.Empty;
    public int? IdParceiro { get; set; }
    public int? IdOportunidade { get; set; }
    public DateTime? DataValidade { get; set; }
    public string? Observacao { get; set; }
    public List<PropostaItemCreateDTO> Itens { get; set; } = new();
}

public class PropostaUpdateDTO
{
    public string Titulo { get; set; } = string.Empty;
    public int? IdParceiro { get; set; }
    public int? IdOportunidade { get; set; }
    public DateTime? DataValidade { get; set; }
    public string? Observacao { get; set; }
    public List<PropostaItemCreateDTO> Itens { get; set; } = new();
}

public class PropostaItemDTO
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
}

public class PropostaItemCreateDTO
{
    public string Descricao { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
}

public class PropostaListDTO
{
    public List<PropostaDTO> Itens { get; set; } = new();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int ItensPorPagina { get; set; }
}
