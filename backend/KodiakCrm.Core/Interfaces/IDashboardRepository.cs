using KodiakCrm.Core.DTOs;

namespace KodiakCrm.Core.Interfaces;

public interface IDashboardRepository
{
    Task<DashboardResumoDTO> ObterResumoAsync(string idEmpresa);
    Task<List<DashboardFunilDTO>> ObterFunilAsync(string idEmpresa);
    Task<List<DashboardLeadsPorStatusDTO>> ObterLeadsPorStatusAsync(string idEmpresa);
    Task<List<DashboardAtividadesDTO>> ObterAtividadesPorTipoAsync(string idEmpresa);
    Task<List<DashboardLeadRecenteDTO>> ObterLeadsRecentesAsync(string idEmpresa, int quantidade = 5);
    Task<List<DashboardLeadsPorEstagioDTO>> ObterLeadsPorEstagioAsync(string idEmpresa);
    Task<DashboardMetricaTicketMedioDTO> ObterTicketMedioAsync(string idEmpresa);
    Task<DashboardMetricaConversaoDTO> ObterMetricasConversaoAsync(string idEmpresa);
    Task<List<DashboardProdutividadeVendedorDTO>> ObterProdutividadeVendedoresAsync(string idEmpresa);
}
