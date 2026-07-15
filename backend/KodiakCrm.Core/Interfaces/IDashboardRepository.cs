using KodiakCrm.Core.DTOs;

namespace KodiakCrm.Core.Interfaces;

public interface IDashboardRepository
{
    Task<DashboardResumoDTO> ObterResumoAsync(string idEmpresa);
    Task<List<DashboardFunilDTO>> ObterFunilAsync(string idEmpresa);
    Task<List<DashboardLeadsPorStatusDTO>> ObterLeadsPorStatusAsync(string idEmpresa);
    Task<List<DashboardAtividadesDTO>> ObterAtividadesPorTipoAsync(string idEmpresa);
}
