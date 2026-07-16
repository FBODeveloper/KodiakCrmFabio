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
                (SELECT COUNT(*) FROM parceiro WHERE id_empresa = @IdEmpresa AND ativo = true) as TotalParceiros,
                (SELECT COUNT(*) FROM lead WHERE id_empresa = @IdEmpresa AND ativo = true) as TotalLeads,
                (SELECT COUNT(*) FROM lead WHERE id_empresa = @IdEmpresa AND ativo = true AND status = 'novo') as LeadsNovos,
                (SELECT COUNT(*) FROM oportunidade WHERE id_empresa = @IdEmpresa AND ativo = true) as TotalOportunidades,
                (SELECT COALESCE(SUM(valor), 0) FROM oportunidade WHERE id_empresa = @IdEmpresa AND ativo = true) as ValorFunil,
                (SELECT COUNT(*) FROM atividade WHERE id_empresa = @IdEmpresa AND ativo = true AND concluida = false) as AtividadesPendentes,
                (SELECT COUNT(*) FROM proposta WHERE id_empresa = @IdEmpresa AND ativo = true AND status = 'enviada') as PropostasEnviadas";

        return await connection.QueryFirstAsync<DashboardResumoDTO>(sql, new { IdEmpresa = idEmpresa });
    }

    public async Task<List<DashboardFunilDTO>> ObterFunilAsync(string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT fe.nome as EstagioNome, 
                   COUNT(o.id) as Quantidade,
                   COALESCE(SUM(o.valor), 0) as Valor
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
            SELECT status, COUNT(*) as Quantidade
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
            SELECT tipo, COUNT(*) as Quantidade
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
            SELECT id, nome, empresa, telefone, status, data_cadastro as DataCadastro
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
            SELECT le.nome as EstagioNome, 
                   COUNT(l.id) as Quantidade,
                   le.cor as Cor
            FROM lead_estagio le
            LEFT JOIN lead l ON le.id = l.id_estagio AND l.ativo = true
            WHERE le.id_empresa = @IdEmpresa
            GROUP BY le.nome, le.cor, le.ordem
            ORDER BY le.ordem";

        return (await connection.QueryAsync<DashboardLeadsPorEstagioDTO>(sql, new { IdEmpresa = idEmpresa })).ToList();
    }

    public async Task<DashboardMetricaTicketMedioDTO> ObterTicketMedioAsync(string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT 
                COALESCE(AVG(valor), 0) as TicketMedio,
                COUNT(CASE WHEN valor IS NOT NULL AND valor > 0 THEN 1 END) as TotalComValor
            FROM oportunidade 
            WHERE id_empresa = @IdEmpresa AND ativo = true";

        return await connection.QueryFirstAsync<DashboardMetricaTicketMedioDTO>(sql, new { IdEmpresa = idEmpresa });
    }

    public async Task<DashboardMetricaConversaoDTO> ObterMetricasConversaoAsync(string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT 
                (SELECT COUNT(*) FROM lead WHERE id_empresa = @IdEmpresa AND ativo = true) as TotalLeads,
                (SELECT COUNT(*) FROM lead WHERE id_empresa = @IdEmpresa AND ativo = true AND status = 'convertido') as LeadsConvertidos,
                CASE WHEN (SELECT COUNT(*) FROM lead WHERE id_empresa = @IdEmpresa AND ativo = true) > 0
                    THEN ROUND((SELECT COUNT(*) FROM lead WHERE id_empresa = @IdEmpresa AND ativo = true AND status = 'convertido')::numeric / 
                         (SELECT COUNT(*) FROM lead WHERE id_empresa = @IdEmpresa AND ativo = true)::numeric * 100, 1)
                    ELSE 0 END as TaxaConversao,
                (SELECT COUNT(*) FROM oportunidade o 
                 JOIN funil_estagio fe ON o.id_estagio = fe.id
                 JOIN funil f ON fe.id_funil = f.id
                 WHERE o.id_empresa = @IdEmpresa AND o.ativo = true 
                 AND fe.nome ILIKE '%ganho%' OR fe.nome ILIKE '%ganha%') as OportunidadesGanhas,
                (SELECT COUNT(*) FROM oportunidade WHERE id_empresa = @IdEmpresa AND ativo = true AND motivo_perda IS NOT NULL) as OportunidadesPerdidas,
                0 as TaxaSucesso";

        return await connection.QueryFirstAsync<DashboardMetricaConversaoDTO>(sql, new { IdEmpresa = idEmpresa });
    }

    public async Task<List<DashboardProdutividadeVendedorDTO>> ObterProdutividadeVendedoresAsync(string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT 
                o.responsavel_id as UsuarioId,
                COALESCE(u.nome, 'Sem responsável') as UsuarioNome,
                COUNT(o.id) as TotalOportunidades,
                COUNT(CASE WHEN o.motivo_perda IS NULL AND o.ativo = true THEN 1 END) as OportunidadesGanhas,
                COALESCE(SUM(o.valor), 0) as ValorTotal
            FROM oportunidade o
            LEFT JOIN usuario u ON o.responsavel_id = u.id
            WHERE o.id_empresa = @IdEmpresa AND o.ativo = true
            GROUP BY o.responsavel_id, u.nome
            ORDER BY ValorTotal DESC";

        return (await connection.QueryAsync<DashboardProdutividadeVendedorDTO>(sql, new { IdEmpresa = idEmpresa })).ToList();
    }
}
