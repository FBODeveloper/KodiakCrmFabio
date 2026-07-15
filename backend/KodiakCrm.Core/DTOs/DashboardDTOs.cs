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
