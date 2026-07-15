namespace KodiakCrm.Core.DTOs;

public class LeadDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Empresa { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Source { get; set; }
    public string Status { get; set; } = "novo";
    public string Temperatura { get; set; } = "frio";
    public int? IdEstagio { get; set; }
    public string? EstagioNome { get; set; }
    public int? IdParceiro { get; set; }
    public string? Observacao { get; set; }
    public int? ResponsavelId { get; set; }
    public string? ResponsavelNome { get; set; }
    public string? ResponsavelAvatar { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
    public string IdEmpresa { get; set; } = string.Empty;
}

public class LeadCreateDTO
{
    public string Nome { get; set; } = string.Empty;
    public string? Empresa { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Source { get; set; }
    public string? Temperatura { get; set; }
    public int? IdEstagio { get; set; }
    public int? ResponsavelId { get; set; }
    public string? Observacao { get; set; }
}

public class LeadUpdateDTO
{
    public string Nome { get; set; } = string.Empty;
    public string? Empresa { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Source { get; set; }
    public string? Status { get; set; }
    public string? Temperatura { get; set; }
    public int? IdEstagio { get; set; }
    public int? ResponsavelId { get; set; }
    public string? Observacao { get; set; }
}

public class LeadListDTO
{
    public List<LeadDTO> Itens { get; set; } = new();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int ItensPorPagina { get; set; }
}

public class LeadEstagioDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public string Cor { get; set; } = "#3b82f6";
}

public class LeadEstagioCreateDTO
{
    public string Nome { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public string Cor { get; set; } = "#3b82f6";
}

public class LeadEstagioUpdateDTO
{
    public string Nome { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public string Cor { get; set; } = "#3b82f6";
}

public class LeadKanbanDTO
{
    public List<LeadEstagioDTO> Estagios { get; set; } = new();
    public List<LeadKanbanColunaDTO> Colunas { get; set; } = new();
}

public class LeadKanbanColunaDTO
{
    public int EstagioId { get; set; }
    public string EstagioNome { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public string Cor { get; set; } = "#3b82f6";
    public List<LeadDTO> Leads { get; set; } = new();
}

public class LeadMoverDTO
{
    public int IdEstagio { get; set; }
}

public class LeadStatsDTO
{
    public int Total { get; set; }
    public int Novos { get; set; }
    public double TaxaConversao { get; set; }
    public int FollowupPendente { get; set; }
}
