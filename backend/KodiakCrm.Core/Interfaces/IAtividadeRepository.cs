using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Interfaces;

public interface IAtividadeRepository
{
    Task<Atividade?> ObterPorIdAsync(int id, string idEmpresa);
    Task<AtividadeListResult> ObterListaAsync(string idEmpresa, string? busca, string? tipo, int? idParceiro, int? idOportunidade, int? responsavelId, bool? concluida, DateTime? dataInicio, DateTime? dataFim, int pagina, int itensPorPagina);
    Task<List<Atividade>> ObterPorParceiroAsync(int idParceiro, string idEmpresa);
    Task<int> CriarAsync(Atividade atividade);
    Task AtualizarAsync(Atividade atividade);
    Task<List<Atividade>> ObterAtividadesAtrasadasAsync(DateTime dataReferencia);
    Task<bool> AlterarStatusAsync(int id, string status, string idEmpresa);
}

public class AtividadeListResult
{
    public List<Atividade> Itens { get; set; } = new();
    public int Total { get; set; }
}
