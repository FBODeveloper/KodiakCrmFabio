using Dapper;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;
using KodiakCrm.Infrastructure.Data;

namespace KodiakCrm.Infrastructure.Repositories;

public class NotificacaoRepository : INotificacaoRepository
{
    private readonly IDatabaseConnection _database;

    public NotificacaoRepository(IDatabaseConnection database)
    {
        _database = database;
    }

    public async Task<List<Notificacao>> ObterPorUsuarioAsync(string idEmpresa, int usuarioId, int limite = 50)
    {
        using var conn = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, usuario_id, titulo, mensagem, tipo,
                   entidade, entidade_id, lida, data_criacao, data_leitura
            FROM notificacao
            WHERE id_empresa = @IdEmpresa AND usuario_id = @UsuarioId
            ORDER BY data_criacao DESC
            LIMIT @Limite";

        return (await conn.QueryAsync<Notificacao>(sql, new { IdEmpresa = idEmpresa, UsuarioId = usuarioId, Limite = limite })).ToList();
    }

    public async Task<int> ContarNaoLidasAsync(string idEmpresa, int usuarioId)
    {
        using var conn = _database.GetConnection();
        const string sql = @"
            SELECT COUNT(*) FROM notificacao
            WHERE id_empresa = @IdEmpresa AND usuario_id = @UsuarioId AND lida = false";

        return await conn.ExecuteScalarAsync<int>(sql, new { IdEmpresa = idEmpresa, UsuarioId = usuarioId });
    }

    public async Task CriarAsync(Notificacao notificacao)
    {
        using var conn = _database.GetConnection();
        const string sql = @"
            INSERT INTO notificacao (id_empresa, usuario_id, titulo, mensagem, tipo, entidade, entidade_id)
            VALUES (@IdEmpresa, @UsuarioId, @Titulo, @Mensagem, @Tipo, @Entidade, @EntidadeId)";

        await conn.ExecuteAsync(sql, notificacao);
    }

    public async Task MarcarLidaAsync(int id, string idEmpresa)
    {
        using var conn = _database.GetConnection();
        const string sql = @"
            UPDATE notificacao
            SET lida = true, data_leitura = NOW()
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        await conn.ExecuteAsync(sql, new { Id = id, IdEmpresa = idEmpresa });
    }

    public async Task MarcarTodasLidasAsync(string idEmpresa, int usuarioId)
    {
        using var conn = _database.GetConnection();
        const string sql = @"
            UPDATE notificacao
            SET lida = true, data_leitura = NOW()
            WHERE id_empresa = @IdEmpresa AND usuario_id = @UsuarioId AND lida = false";

        await conn.ExecuteAsync(sql, new { IdEmpresa = idEmpresa, UsuarioId = usuarioId });
    }

    public async Task ExcluirAsync(int id, string idEmpresa)
    {
        using var conn = _database.GetConnection();
        const string sql = "DELETE FROM notificacao WHERE id = @Id AND id_empresa = @IdEmpresa";
        await conn.ExecuteAsync(sql, new { Id = id, IdEmpresa = idEmpresa });
    }
}
