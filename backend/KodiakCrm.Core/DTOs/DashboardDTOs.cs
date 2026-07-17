namespace KodiakCrm.Core.DTOs;

public class DashboardResumoDTO
{
    public int TotalParceiros { get; set; }
    public int TotalLeads { get; set; }
    public int LeadsNovos { get; set; }
    public int TotalOportunidades { get; set; }
    public decimal ValorFunil { get; set; }
    public int AtividadesPendentes { get; set; }
    public int PropostasEnviadas { get; set; }
}

public class DashboardFunilDTO
{
    public string EstagioNome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal Valor { get; set; }
}

public class DashboardLeadsPorStatusDTO
{
    public string Status { get; set; } = string.Empty;
    public int Quantidade { get; set; }
}

public class DashboardAtividadesDTO
{
    public string Tipo { get; set; } = string.Empty;
    public int Quantidade { get; set; }
}

public class DashboardLeadRecenteDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Empresa { get; set; }
    public string? Telefone { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }
}

public class DashboardLeadsPorEstagioDTO
{
    public string EstagioNome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public string Cor { get; set; } = "#3b82f6";
}

public class DashboardMetricaTicketMedioDTO
{
    public decimal TicketMedio { get; set; }
    public int TotalComValor { get; set; }
}

public class DashboardMetricaConversaoDTO
{
    public int TotalLeads { get; set; }
    public int LeadsConvertidos { get; set; }
    public double TaxaConversao { get; set; }
    public int OportunidadesGanhas { get; set; }
    public int OportunidadesPerdidas { get; set; }
    public double TaxaSucesso { get; set; }
}

public class DashboardProdutividadeVendedorDTO
{
    public int UsuarioId { get; set; }
    public string UsuarioNome { get; set; } = string.Empty;
    public int TotalOportunidades { get; set; }
    public int OportunidadesGanhas { get; set; }
    public decimal ValorTotal { get; set; }
}

public class TimelineItemDTO
{
    public string Mes { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal Valor { get; set; }
}
