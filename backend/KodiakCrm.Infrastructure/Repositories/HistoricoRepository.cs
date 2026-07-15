using Dapper;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.Infrastructure.Repositories;

public class HistoricoRepository : IHistoricoRepository
{
    private readonly IDatabaseConnection _database;

    public HistoricoRepository(IDatabaseConnection database)
    {
        _database = database;
    }

    public async Task<int> CriarAsync(Historico historico)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            INSERT INTO historico (id_empresa, id_estabelecimento, cnpj_empresa,
                                   entidade, entidade_id, acao, descricao,
                                   dados_antes, dados_depois,
                                   usuario_id, usuario_nome, data_acao)
            VALUES (@IdEmpresa, @IdEstabelecimento, @CnpjEmpresa,
                    @Entidade, @EntidadeId, @Acao, @Descricao,
                    @DadosAntes, @DadosDepois,
                    @UsuarioId, @UsuarioNome, @DataAcao)
            RETURNING id";

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            historico.IdEmpresa,
            historico.IdEstabelecimento,
            historico.CnpjEmpresa,
            historico.Entidade,
            historico.EntidadeId,
            historico.Acao,
            historico.Descricao,
            historico.DadosAntes,
            historico.DadosDepois,
            historico.UsuarioId,
            historico.UsuarioNome,
            historico.DataAcao
        });
    }

    public async Task<List<Historico>> ObterPorEntidadeAsync(string entidade, int entidadeId, string idEmpresa, int limite = 50)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, id_estabelecimento, cnpj_empresa,
                   entidade, entidade_id, acao, descricao,
                   dados_antes, dados_depois,
                   usuario_id, usuario_nome, data_acao
            FROM historico
            WHERE entidade = @Entidade AND entidade_id = @EntidadeId AND id_empresa = @IdEmpresa
            ORDER BY data_acao DESC
            LIMIT @Limite";

        return (await connection.QueryAsync<Historico>(sql, new
        {
            Entidade = entidade,
            EntidadeId = entidadeId,
            IdEmpresa = idEmpresa,
            Limite = limite
        })).ToList();
    }

    public async Task<List<Historico>> ObterRecentesAsync(string idEmpresa, int limite = 20)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, id_estabelecimento, cnpj_empresa,
                   entidade, entidade_id, acao, descricao,
                   dados_antes, dados_depois,
                   usuario_id, usuario_nome, data_acao
            FROM historico
            WHERE id_empresa = @IdEmpresa
            ORDER BY data_acao DESC
            LIMIT @Limite";

        return (await connection.QueryAsync<Historico>(sql, new
        {
            IdEmpresa = idEmpresa,
            Limite = limite
        })).ToList();
    }
}
