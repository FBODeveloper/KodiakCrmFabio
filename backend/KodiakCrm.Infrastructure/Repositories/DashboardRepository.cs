using Dapper;
using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.Infrastructure.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly IDatabaseConnection _database;

    public DashboardRepository(IDatabaseConnection database)
    {
        _database = database;
    }

    public async Task<DashboardResumoDTO> ObterResumoAsync(string idEmpresa)
    {
        using var connection = _database.GetConnection();

        var sql = @"
            SELECT 
                (SELECT COUNT(*) FROM parceiro WHERE id_empresa = @IdEmpresa AND ativo = true) as total_parceiros,
                (SELECT COUNT(*) FROM lead WHERE id_empresa = @IdEmpresa AND ativo = true) as total_leads,
                (SELECT COUNT(*) FROM lead WHERE id_empresa = @IdEmpresa AND ativo = true AND status = 'novo') as leads_novos,
                (SELECT COUNT(*) FROM oportunidade WHERE id_empresa = @IdEmpresa AND ativo = true) as total_oportunidades,
                (SELECT COALESCE(SUM(valor), 0) FROM oportunidade WHERE id_empresa = @IdEmpresa AND ativo = true) as valor_funil,
                (SELECT COUNT(*) FROM atividade WHERE id_empresa = @IdEmpresa AND ativo = true AND concluida = false) as atividades_pendentes,
                (SELECT COUNT(*) FROM proposta WHERE id_empresa = @IdEmpresa AND ativo = true AND status = 'enviada') as propostas_enviadas";

        return await connection.QueryFirstAsync<DashboardResumoDTO>(sql, new { IdEmpresa = idEmpresa });
    }

    public async Task<List<DashboardFunilDTO>> ObterFunilAsync(string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT fe.nome as estagio_nome, 
                   COUNT(o.id) as quantidade,
                   COALESCE(SUM(o.valor), 0) as valor
            FROM funil_estagio fe
            LEFT JOIN oportunidade o ON fe.id = o.id_estagio AND o.ativo = true
            LEFT JOIN funil f ON fe.id_funil = f.id
            WHERE f.id_empresa = @IdEmpresa AND f.ativo = true
            GROUP BY fe.nome, fe.ordem
            ORDER BY fe.ordem";

        return (await connection.QueryAsync<DashboardFunilDTO>(sql, new { IdEmpresa = idEmpresa })).ToList();
    }

    public async Task<List<DashboardLeadsPorStatusDTO>> ObterLeadsPorStatusAsync(string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT status, COUNT(*) as quantidade
            FROM lead 
            WHERE id_empresa = @IdEmpresa AND ativo = true
            GROUP BY status
            ORDER BY status";

        return (await connection.QueryAsync<DashboardLeadsPorStatusDTO>(sql, new { IdEmpresa = idEmpresa })).ToList();
    }

    public async Task<List<DashboardAtividadesDTO>> ObterAtividadesPorTipoAsync(string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT tipo, COUNT(*) as quantidade
            FROM atividade 
            WHERE id_empresa = @IdEmpresa AND ativo = true
            GROUP BY tipo
            ORDER BY tipo";

        return (await connection.QueryAsync<DashboardAtividadesDTO>(sql, new { IdEmpresa = idEmpresa })).ToList();
    }

    public async Task<List<DashboardLeadRecenteDTO>> ObterLeadsRecentesAsync(string idEmpresa, int quantidade = 5)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, nome, empresa, telefone, status, data_cadastro
            FROM lead 
            WHERE id_empresa = @IdEmpresa AND ativo = true
            ORDER BY data_cadastro DESC
            LIMIT @Quantidade";

        return (await connection.QueryAsync<DashboardLeadRecenteDTO>(sql, new { IdEmpresa = idEmpresa, Quantidade = quantidade })).ToList();
    }

    public async Task<List<DashboardLeadsPorEstagioDTO>> ObterLeadsPorEstagioAsync(string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT le.nome as estagio_nome, 
                   COUNT(l.id) as quantidade,
                   le.cor
            FROM lead_estagio le
            LEFT JOIN lead l ON le.id = l.id_estagio AND l.ativo = true
            WHERE le.id_empresa = @IdEmpresa
            GROUP BY le.nome, le.cor, le.ordem
            ORDER BY le.ordem";

        return (await connection.QueryAsync<DashboardLeadsPorEstagioDTO>(sql, new { IdEmpresa = idEmpresa })).ToList();
    }
}
