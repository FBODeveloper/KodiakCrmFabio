using Dapper;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;
using KodiakCrm.Infrastructure.Data;

namespace KodiakCrm.Infrastructure.Repositories;

public class RelatorioRepository : IRelatorioRepository
{
    private readonly IDatabaseConnection _database;

    public RelatorioRepository(IDatabaseConnection database)
    {
        _database = database;
    }

    public async Task<RelatorioGerado?> CriarAsync(RelatorioGerado relatorio)
    {
        using var conn = _database.GetConnection();
        const string sql = @"
            INSERT INTO relatorio_gerado (id_empresa, cnpj_empresa, tipo, titulo, parametros, resultado, usuario_id, usuario_nome)
            VALUES (@IdEmpresa, @CnpjEmpresa, @Tipo, @Titulo, @Parametros::jsonb, @Resultado::jsonb, @UsuarioId, @UsuarioNome)
            RETURNING id, data_geracao";

        var result = await conn.QueryFirstOrDefaultAsync<RelatorioGerado>(sql, relatorio);
        if (result != null)
        {
            relatorio.Id = result.Id;
            relatorio.DataGeracao = result.DataGeracao;
        }
        return relatorio;
    }

    public async Task<List<RelatorioGerado>> ObterRecentesAsync(string idEmpresa, int limite = 20)
    {
        using var conn = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, cnpj_empresa, tipo, titulo, parametros, resultado::text,
                   usuario_id, usuario_nome, data_geracao
            FROM relatorio_gerado
            WHERE id_empresa = @IdEmpresa
            ORDER BY data_geracao DESC
            LIMIT @Limite";

        return (await conn.QueryAsync<RelatorioGerado>(sql, new { IdEmpresa = idEmpresa, Limite = limite })).ToList();
    }

    public async Task<List<OportunidadeRelatorio>> ObterOportunidadesAsync(
        string idEmpresa, DateTime? dataInicio, DateTime? dataFim, string? status, int? responsavelId)
    {
        using var conn = _database.GetConnection();
        var where = "WHERE o.id_empresa = @IdEmpresa AND o.ativo = true";
        var parameters = new DynamicParameters();
        parameters.Add("IdEmpresa", idEmpresa);

        if (dataInicio.HasValue) { where += " AND o.data_cadastro >= @DataInicio"; parameters.Add("DataInicio", dataInicio.Value); }
        if (dataFim.HasValue) { where += " AND o.data_cadastro <= @DataFim"; parameters.Add("DataFim", dataFim.Value.AddDays(1)); }
        if (responsavelId.HasValue) { where += " AND o.responsavel_id = @ResponsavelId"; parameters.Add("ResponsavelId", responsavelId.Value); }

        var sql = $@"
            SELECT o.titulo, o.valor,
                   CASE WHEN o.motivo_perda IS NOT NULL THEN 'perdida' ELSE 'aberta' END as status,
                   u.nome as responsavel_nome, o.responsavel_id,
                   o.motivo_perda, o.data_cadastro
            FROM oportunidade o
            LEFT JOIN usuario u ON o.responsavel_id = u.id
            {where}";

        if (!string.IsNullOrEmpty(status))
        {
            if (status == "ganha") sql += " AND o.motivo_perda IS NULL AND EXISTS (SELECT 1 FROM cliente c WHERE c.id_oportunidade = o.id)";
            else if (status == "perdida") sql += " AND o.motivo_perda IS NOT NULL";
            else if (status == "aberta") sql += " AND o.motivo_perda IS NULL AND NOT EXISTS (SELECT 1 FROM cliente c WHERE c.id_oportunidade = o.id)";
        }

        sql += " ORDER BY o.data_cadastro DESC";

        return (await conn.QueryAsync<OportunidadeRelatorio>(sql, parameters)).ToList();
    }

    public async Task<List<AtividadeRelatorio>> ObterAtividadesAsync(
        string idEmpresa, DateTime? dataInicio, DateTime? dataFim, string? tipo, int? responsavelId)
    {
        using var conn = _database.GetConnection();
        var where = "WHERE a.id_empresa = @IdEmpresa AND a.ativo = true";
        var parameters = new DynamicParameters();
        parameters.Add("IdEmpresa", idEmpresa);

        if (dataInicio.HasValue) { where += " AND a.data_cadastro >= @DataInicio"; parameters.Add("DataInicio", dataInicio.Value); }
        if (dataFim.HasValue) { where += " AND a.data_cadastro <= @DataFim"; parameters.Add("DataFim", dataFim.Value.AddDays(1)); }
        if (!string.IsNullOrEmpty(tipo)) { where += " AND a.tipo = @Tipo"; parameters.Add("Tipo", tipo); }
        if (responsavelId.HasValue) { where += " AND a.responsavel_id = @ResponsavelId"; parameters.Add("ResponsavelId", responsavelId.Value); }

        var sql = $@"
            SELECT a.tipo, a.titulo, a.concluida,
                   u.nome as responsavel_nome, a.responsavel_id,
                   a.data_cadastro
            FROM atividade a
            LEFT JOIN usuario u ON a.responsavel_id = u.id
            {where}
            ORDER BY a.data_cadastro DESC";

        return (await conn.QueryAsync<AtividadeRelatorio>(sql, parameters)).ToList();
    }

    public async Task<List<LeadRelatorio>> ObterLeadsAsync(
        string idEmpresa, DateTime? dataInicio, DateTime? dataFim, int? responsavelId)
    {
        using var conn = _database.GetConnection();
        var where = "WHERE l.id_empresa = @IdEmpresa AND l.ativo = true";
        var parameters = new DynamicParameters();
        parameters.Add("IdEmpresa", idEmpresa);

        if (dataInicio.HasValue) { where += " AND l.data_cadastro >= @DataInicio"; parameters.Add("DataInicio", dataInicio.Value); }
        if (dataFim.HasValue) { where += " AND l.data_cadastro <= @DataFim"; parameters.Add("DataFim", dataFim.Value.AddDays(1)); }
        if (responsavelId.HasValue) { where += " AND l.responsavel_id = @ResponsavelId"; parameters.Add("ResponsavelId", responsavelId.Value); }

        var sql = $@"
            SELECT l.nome, l.status,
                   u.nome as responsavel_nome, l.responsavel_id,
                   l.data_cadastro
            FROM lead l
            LEFT JOIN usuario u ON l.responsavel_id = u.id
            {where}
            ORDER BY l.data_cadastro DESC";

        return (await conn.QueryAsync<LeadRelatorio>(sql, parameters)).ToList();
    }
}
