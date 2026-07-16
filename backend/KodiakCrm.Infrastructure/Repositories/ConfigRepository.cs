using Dapper;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;
using KodiakCrm.Infrastructure.Data;

namespace KodiakCrm.Infrastructure.Repositories;

public class ConfigRepository : IConfigRepository
{
    private readonly IDatabaseConnection _connection;

    public ConfigRepository(IDatabaseConnection connection)
    {
        _connection = connection;
    }

    public async Task<EmpresaConfig?> ObterPorCnpjAsync(string cnpjEmpresa)
    {
        using var conn = _connection.GetConnection();
        const string sql = @"
            SELECT id, cnpj_empresa, tema, fuso_horario, moeda, idioma,
                   notificacoes_email, notificacoes_sistema, data_atualizacao
            FROM empresa_config
            WHERE cnpj_empresa = @CnpjEmpresa";

        return await conn.QueryFirstOrDefaultAsync<EmpresaConfig>(sql, new { CnpjEmpresa = cnpjEmpresa });
    }

    public async Task AtualizarAsync(EmpresaConfig config)
    {
        using var conn = _connection.GetConnection();
        const string sql = @"
            UPDATE empresa_config
            SET tema = @Tema,
                fuso_horario = @FusoHorario,
                moeda = @Moeda,
                idioma = @Idioma,
                notificacoes_email = @NotificacoesEmail,
                notificacoes_sistema = @NotificacoesSistema,
                data_atualizacao = NOW()
            WHERE cnpj_empresa = @CnpjEmpresa";

        await conn.ExecuteAsync(sql, config);
    }

    public async Task CriarAsync(EmpresaConfig config)
    {
        using var conn = _connection.GetConnection();
        const string sql = @"
            INSERT INTO empresa_config (cnpj_empresa, tema, fuso_horario, moeda, idioma, notificacoes_email, notificacoes_sistema)
            VALUES (@CnpjEmpresa, @Tema, @FusoHorario, @Moeda, @Idioma, @NotificacoesEmail, @NotificacoesSistema)
            ON CONFLICT (cnpj_empresa) DO NOTHING";

        await conn.ExecuteAsync(sql, config);
    }
}
