using Dapper;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.Infrastructure.Repositories;

public class LeadRepository : ILeadRepository
{
    private readonly IDatabaseConnection _database;

    public LeadRepository(IDatabaseConnection database)
    {
        _database = database;
    }

    public async Task<Lead?> ObterPorIdAsync(int id, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, id_estabelecimento, cnpj_empresa,
                   nome, empresa, email, telefone, source, status,
                   temperatura, id_estagio, id_parceiro, observacao,
                   responsavel_id, responsavel_nome, responsavel_avatar,
                   ativo, data_cadastro
            FROM lead 
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        return await connection.QueryFirstOrDefaultAsync<Lead>(sql, new { Id = id, IdEmpresa = idEmpresa });
    }

    public async Task<LeadListResult> ObterListaAsync(string idEmpresa, string? busca, string? status, int pagina, int itensPorPagina)
    {
        using var connection = _database.GetConnection();

        var whereClause = "WHERE l.id_empresa = @IdEmpresa AND l.ativo = true";
        var parameters = new DynamicParameters();
        parameters.Add("IdEmpresa", idEmpresa);

        if (!string.IsNullOrWhiteSpace(busca))
        {
            whereClause += " AND (l.nome ILIKE @Busca OR l.empresa ILIKE @Busca OR l.email ILIKE @Busca)";
            parameters.Add("Busca", $"%{busca}%");
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            whereClause += " AND l.status = @Status";
            parameters.Add("Status", status);
        }

        var countSql = $"SELECT COUNT(*) FROM lead l {whereClause}";
        var total = await connection.ExecuteScalarAsync<int>(countSql, parameters);

        var offset = (pagina - 1) * itensPorPagina;
        parameters.Add("Offset", offset);
        parameters.Add("Limit", itensPorPagina);

        var sql = $@"
            SELECT l.id, l.id_empresa, l.id_estabelecimento, l.cnpj_empresa,
                   l.nome, l.empresa, l.email, l.telefone, l.source, l.status,
                   l.temperatura, l.id_estagio, l.id_parceiro, l.observacao,
                   l.responsavel_id, l.responsavel_nome, l.responsavel_avatar,
                   l.ativo, l.data_cadastro,
                   u.nome as responsavel_nome, u.avatar as responsavel_avatar
            FROM lead l
            LEFT JOIN usuario u ON l.responsavel_id = u.id
            {whereClause}
            ORDER BY l.data_cadastro DESC
            LIMIT @Limit OFFSET @Offset";

        var itens = (await connection.QueryAsync<Lead>(sql, parameters)).ToList();

        return new LeadListResult
        {
            Itens = itens,
            Total = total
        };
    }

    public async Task<int> CriarAsync(Lead lead)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            INSERT INTO lead (id_empresa, id_estabelecimento, cnpj_empresa,
                              nome, empresa, email, telefone, source, status,
                              temperatura, id_estagio, id_parceiro, observacao, responsavel_id)
            VALUES (@IdEmpresa, @IdEstabelecimento, @CnpjEmpresa,
                    @Nome, @Empresa, @Email, @Telefone, @Source, @Status,
                    @Temperatura, @IdEstagio, @IdParceiro, @Observacao, @ResponsavelId)
            RETURNING id";

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            lead.IdEmpresa,
            lead.IdEstabelecimento,
            lead.CnpjEmpresa,
            lead.Nome,
            lead.Empresa,
            lead.Email,
            lead.Telefone,
            lead.Source,
            lead.Status,
            lead.Temperatura,
            lead.IdEstagio,
            lead.IdParceiro,
            lead.Observacao,
            lead.ResponsavelId
        });
    }

    public async Task AtualizarAsync(Lead lead)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            UPDATE lead 
            SET nome = @Nome,
                empresa = @Empresa,
                email = @Email,
                telefone = @Telefone,
                source = @Source,
                status = @Status,
                temperatura = @Temperatura,
                id_estagio = @IdEstagio,
                id_parceiro = @IdParceiro,
                observacao = @Observacao,
                responsavel_id = @ResponsavelId
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        await connection.ExecuteAsync(sql, new
        {
            lead.Id,
            lead.IdEmpresa,
            lead.Nome,
            lead.Empresa,
            lead.Email,
            lead.Telefone,
            lead.Source,
            lead.Status,
            lead.Temperatura,
            lead.IdEstagio,
            lead.IdParceiro,
            lead.Observacao,
            lead.ResponsavelId
        });
    }

    public async Task ExcluirAsync(int id, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            UPDATE lead SET ativo = false
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        await connection.ExecuteAsync(sql, new { Id = id, IdEmpresa = idEmpresa });
    }

    public async Task<bool> ExisteEmailAsync(string email, string idEmpresa, int? idExcluir = null)
    {
        using var connection = _database.GetConnection();
        var sql = "SELECT COUNT(*) FROM lead WHERE email = @Email AND id_empresa = @IdEmpresa AND ativo = true";
        var parameters = new DynamicParameters();
        parameters.Add("Email", email);
        parameters.Add("IdEmpresa", idEmpresa);

        if (idExcluir.HasValue)
        {
            sql += " AND id != @IdExcluir";
            parameters.Add("IdExcluir", idExcluir.Value);
        }

        var count = await connection.ExecuteScalarAsync<int>(sql, parameters);
        return count > 0;
    }

    public async Task<List<LeadEstagio>> ObterEstagiosAsync(string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, nome, ordem, cor
            FROM lead_estagio
            WHERE id_empresa = @IdEmpresa
            ORDER BY ordem";

        return (await connection.QueryAsync<LeadEstagio>(sql, new { IdEmpresa = idEmpresa })).ToList();
    }

    public async Task<int> CriarEstagioAsync(LeadEstagio estagio)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            INSERT INTO lead_estagio (id_empresa, nome, ordem, cor)
            VALUES (@IdEmpresa, @Nome, @Ordem, @Cor)
            RETURNING id";

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            estagio.IdEmpresa,
            estagio.Nome,
            estagio.Ordem,
            estagio.Cor
        });
    }

    public async Task AtualizarEstagioAsync(LeadEstagio estagio)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            UPDATE lead_estagio
            SET nome = @Nome, ordem = @Ordem, cor = @Cor
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        await connection.ExecuteAsync(sql, new
        {
            estagio.Id,
            estagio.IdEmpresa,
            estagio.Nome,
            estagio.Ordem,
            estagio.Cor
        });
    }

    public async Task ExcluirEstagioAsync(int id, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            DELETE FROM lead_estagio
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        await connection.ExecuteAsync(sql, new { Id = id, IdEmpresa = idEmpresa });
    }

    public async Task<List<Lead>> ObterPorEstagioAsync(int idEstagio, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT l.id, l.id_empresa, l.id_estabelecimento, l.cnpj_empresa,
                   l.nome, l.empresa, l.email, l.telefone, l.source, l.status,
                   l.temperatura, l.id_estagio, l.id_parceiro, l.observacao,
                   l.responsavel_id, l.responsavel_nome, l.responsavel_avatar,
                   l.ativo, l.data_cadastro
            FROM lead l
            WHERE l.id_estagio = @IdEstagio AND l.id_empresa = @IdEmpresa AND l.ativo = true
            ORDER BY l.data_cadastro DESC";

        return (await connection.QueryAsync<Lead>(sql, new { IdEstagio = idEstagio, IdEmpresa = idEmpresa })).ToList();
    }
}
