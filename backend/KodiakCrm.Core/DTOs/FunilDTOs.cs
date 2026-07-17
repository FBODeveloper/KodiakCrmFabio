namespace KodiakCrm.Core.DTOs;

public class FunilDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public List<FunilEstagioDTO> Estagios { get; set; } = new();
}

public class FunilCreateDTO
{
    public string Nome { get; set; } = string.Empty;
    public List<FunilEstagioCreateDTO> Estagios { get; set; } = new();
}

public class FunilUpdateDTO
{
    public string Nome { get; set; } = string.Empty;
    public List<FunilEstagioCreateDTO> Estagios { get; set; } = new();
}

public class FunilEstagioDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public int Probabilidade { get; set; }
}

public class FunilEstagioCreateDTO
{
    public string Nome { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public int Probabilidade { get; set; }
}

public class OportunidadeDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int? IdParceiro { get; set; }
    public string? ParceiroNome { get; set; }
    public int? IdEstagio { get; set; }
    public string? EstagioNome { get; set; }
    public int? FunilId { get; set; }
    public string? FunilNome { get; set; }
    public decimal? Valor { get; set; }
    public DateOnly? DataPrevisao { get; set; }
    public int? ResponsavelId { get; set; }
    public string? ResponsavelNome { get; set; }
    public string? Observacao { get; set; }
    public string? MotivoPerda { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
    public string IdEmpresa { get; set; } = string.Empty;
}

public class OportunidadeCreateDTO
{
    public string Titulo { get; set; } = string.Empty;
    public int? IdParceiro { get; set; }
    public int? IdEstagio { get; set; }
    public decimal? Valor { get; set; }
    public DateOnly? DataPrevisao { get; set; }
    public string? Observacao { get; set; }
}

public class OportunidadeUpdateDTO
{
    public string Titulo { get; set; } = string.Empty;
    public int? IdParceiro { get; set; }
    public int? IdEstagio { get; set; }
    public decimal? Valor { get; set; }
    public DateOnly? DataPrevisao { get; set; }
    public int? ResponsavelId { get; set; }
    public string? Observacao { get; set; }
    public string? MotivoPerda { get; set; }
}

public class OportunidadeMoverDTO
{
    public int IdEstagio { get; set; }
}

public class OportunidadeListDTO
{
    public List<OportunidadeDTO> Itens { get; set; } = new();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int ItensPorPagina { get; set; }
}

public class KanbanDTO
{
    public int FunilId { get; set; }
    public string FunilNome { get; set; } = string.Empty;
    public List<KanbanColunaDTO> Colunas { get; set; } = new();
}

public class KanbanColunaDTO
{
    public int EstagioId { get; set; }
    public string EstagioNome { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public List<OportunidadeDTO> Oportunidades { get; set; } = new();
}
