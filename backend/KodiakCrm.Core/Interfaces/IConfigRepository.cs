using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Interfaces;

public interface IConfigRepository
{
    Task<EmpresaConfig?> ObterPorCnpjAsync(string cnpjEmpresa);
    Task AtualizarAsync(EmpresaConfig config);
    Task CriarAsync(EmpresaConfig config);
}
