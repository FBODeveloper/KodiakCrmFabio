using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.UseCases.Services;

public class DashboardService
{
    private readonly IDashboardRepository _repository;

    public DashboardService(IDashboardRepository repository)
    {
        _repository = repository;
    }

    public async Task<DashboardResumoDTO> ObterResumoAsync(string idEmpresa)
    {
        return await _repository.ObterResumoAsync(idEmpresa);
    }

    public async Task<List<DashboardFunilDTO>> ObterFunilAsync(string idEmpresa)
    {
        return await _repository.ObterFunilAsync(idEmpresa);
    }

    public async Task<List<DashboardLeadsPorStatusDTO>> ObterLeadsPorStatusAsync(string idEmpresa)
    {
        return await _repository.ObterLeadsPorStatusAsync(idEmpresa);
    }

    public async Task<List<DashboardAtividadesDTO>> ObterAtividadesPorTipoAsync(string idEmpresa)
    {
        return await _repository.ObterAtividadesPorTipoAsync(idEmpresa);
    }
}
