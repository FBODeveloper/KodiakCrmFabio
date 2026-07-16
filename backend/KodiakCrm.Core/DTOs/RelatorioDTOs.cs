namespace KodiakCrm.Core.DTOs;

public class RelatorioFiltroDTO
{
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string? Status { get; set; }
    public int? ResponsavelId { get; set; }
    public string? TipoAtividade { get; set; }
}

public class RelatorioVendasDTO
{
    public int TotalOportunidades { get; set; }
    public int OportunidadesGanhas { get; set; }
    public int OportunidadesPerdidas { get; set; }
    public int OportunidadesAbertas { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal ValorGanho { get; set; }
    public decimal ValorPerdido { get; set; }
    public decimal TicketMedio { get; set; }
    public decimal TaxaConversao { get; set; }
    public List<RelatorioVendasPorPeriodoDTO> PorPeriodo { get; set; } = new();
    public List<RelatorioVendasPorResponsavelDTO> PorResponsavel { get; set; } = new();
}

public class RelatorioVendasPorPeriodoDTO
{
    public string Periodo { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal Valor { get; set; }
}

public class RelatorioVendasPorResponsavelDTO
{
    public string ResponsavelNome { get; set; } = string.Empty;
    public int Total { get; set; }
    public int Ganhas { get; set; }
    public decimal ValorTotal { get; set; }
}

public class RelatorioAtividadesDTO
{
    public int TotalAtividades { get; set; }
    public int Concluidas { get; set; }
    public int Pendentes { get; set; }
    public decimal TaxaConclusao { get; set; }
    public List<RelatorioAtividadesPorTipoDTO> PorTipo { get; set; } = new();
    public List<RelatorioAtividadesPorResponsavelDTO> PorResponsavel { get; set; } = new();
}

public class RelatorioAtividadesPorTipoDTO
{
    public string Tipo { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public int Concluidas { get; set; }
}

public class RelatorioAtividadesPorResponsavelDTO
{
    public string ResponsavelNome { get; set; } = string.Empty;
    public int Total { get; set; }
    public int Concluidas { get; set; }
}

public class RelatorioPerformanceDTO
{
    public List<RelatorioPerformanceVendedorDTO> Vendedores { get; set; } = new();
}

public class RelatorioPerformanceVendedorDTO
{
    public string VendedorNome { get; set; } = string.Empty;
    public int TotalLeads { get; set; }
    public int TotalOportunidades { get; set; }
    public int OportunidadesGanhas { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal TaxaConversao { get; set; }
    public int TotalAtividades { get; set; }
}

public class RelatorioGeradoDTO
{
    public int Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string? Parametros { get; set; }
    public string Resultado { get; set; } = string.Empty;
    public string? UsuarioNome { get; set; }
    public DateTime DataGeracao { get; set; }
}
