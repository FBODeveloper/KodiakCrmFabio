using Dapper;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.Infrastructure.Repositories;

public class FunilRepository : IFunilRepository
{
    private readonly IDatabaseConnection _database;

    public FunilRepository(IDatabaseConnection database)
    {
        _database = database;
    }

    public async Task<Funil?> ObterPorIdAsync(int id, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, nome, ativo
            FROM funil 
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        return await connection.QueryFirstOrDefaultAsync<Funil>(sql, new { Id = id, IdEmpresa = idEmpresa });
    }

    public async Task<List<Funil>> ObterListaAsync(string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, nome, ativo
            FROM funil 
            WHERE id_empresa = @IdEmpresa AND ativo = true
            ORDER BY nome";

        return (await connection.QueryAsync<Funil>(sql, new { IdEmpresa = idEmpresa })).ToList();
    }

    public async Task<List<FunilEstagio>> ObterEstagiosAsync(int idFunil)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_funil, nome, ordem, probabilidade
            FROM funil_estagio 
            WHERE id_funil = @IdFunil
            ORDER BY ordem";

        return (await connection.QueryAsync<FunilEstagio>(sql, new { IdFunil = idFunil })).ToList();
    }

    public async Task<int> CriarAsync(Funil funil)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            INSERT INTO funil (id_empresa, nome)
            VALUES (@IdEmpresa, @Nome)
            RETURNING id";

        return await connection.ExecuteScalarAsync<int>(sql, new { funil.IdEmpresa, funil.Nome });
    }

    public async Task<int> CriarEstagioAsync(FunilEstagio estagio)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            INSERT INTO funil_estagio (id_funil, nome, ordem, probabilidade)
            VALUES (@IdFunil, @Nome, @Ordem, @Probabilidade)
            RETURNING id";

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            estagio.IdFunil,
            estagio.Nome,
            estagio.Ordem,
            estagio.Probabilidade
        });
    }

    public async Task AtualizarAsync(Funil funil)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            UPDATE funil 
            SET nome = @Nome
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        await connection.ExecuteAsync(sql, new { funil.Id, funil.IdEmpresa, funil.Nome });
    }

    public async Task ExcluirAsync(int id, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            UPDATE funil 
            SET ativo = false
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        await connection.ExecuteAsync(sql, new { Id = id, IdEmpresa = idEmpresa });
    }
}

public class OportunidadeRepository : IOportunidadeRepository
{
    private readonly IDatabaseConnection _database;

    public OportunidadeRepository(IDatabaseConnection database)
    {
        _database = database;
    }

    public async Task<Oportunidade?> ObterPorIdAsync(int id, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, id_estabelecimento, cnpj_empresa,
                   titulo, id_parceiro, id_estagio, valor,
                   data_previsao, responsavel_id, observacao, ativo, data_cadastro
            FROM oportunidade 
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        return await connection.QueryFirstOrDefaultAsync<Oportunidade>(sql, new { Id = id, IdEmpresa = idEmpresa });
    }

    public async Task<OportunidadeListResult> ObterListaAsync(string idEmpresa, string? busca, int? idEstagio, int? responsavelId, int pagina, int itensPorPagina)
    {
        using var connection = _database.GetConnection();

        var whereClause = "WHERE o.id_empresa = @IdEmpresa AND o.ativo = true";
        var parameters = new DynamicParameters();
        parameters.Add("IdEmpresa", idEmpresa);

        if (!string.IsNullOrWhiteSpace(busca))
        {
            whereClause += " AND o.titulo ILIKE @Busca";
            parameters.Add("Busca", $"%{busca}%");
        }

        if (idEstagio.HasValue)
        {
            whereClause += " AND o.id_estagio = @IdEstagio";
            parameters.Add("IdEstagio", idEstagio.Value);
        }

        if (responsavelId.HasValue)
        {
            whereClause += " AND o.responsavel_id = @ResponsavelId";
            parameters.Add("ResponsavelId", responsavelId.Value);
        }

        var countSql = $"SELECT COUNT(*) FROM oportunidade o {whereClause}";
        var total = await connection.ExecuteScalarAsync<int>(countSql, parameters);

        var offset = (pagina - 1) * itensPorPagina;
        parameters.Add("Offset", offset);
        parameters.Add("Limit", itensPorPagina);

        var sql = $@"
            SELECT o.id, o.id_empresa, o.id_estabelecimento, o.cnpj_empresa,
                   o.titulo, o.id_parceiro, o.id_estagio, o.valor,
                   o.data_previsao, o.responsavel_id, o.observacao, o.ativo, o.data_cadastro,
                   p.razao_social as parceiro_nome,
                   fe.nome as estagio_nome,
                   f.id as funil_id, f.nome as funil_nome,
                   u.nome as responsavel_nome
            FROM oportunidade o
            LEFT JOIN parceiro p ON o.id_parceiro = p.id
            LEFT JOIN funil_estagio fe ON o.id_estagio = fe.id
            LEFT JOIN funil f ON fe.id_funil = f.id
            LEFT JOIN usuario u ON o.responsavel_id = u.id
            {whereClause}
            ORDER BY o.data_cadastro DESC
            LIMIT @Limit OFFSET @Offset";

        var itens = (await connection.QueryAsync<Oportunidade>(sql, parameters)).ToList();

        return new OportunidadeListResult
        {
            Itens = itens,
            Total = total
        };
    }

    public async Task<List<Oportunidade>> ObterPorEstagioAsync(int idEstagio, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, id_estabelecimento, cnpj_empresa,
                   titulo, id_parceiro, id_estagio, valor,
                   data_previsao, responsavel_id, observacao, ativo, data_cadastro
            FROM oportunidade 
            WHERE id_estagio = @IdEstagio AND id_empresa = @IdEmpresa AND ativo = true
            ORDER BY data_cadastro DESC";

        return (await connection.QueryAsync<Oportunidade>(sql, new { IdEstagio = idEstagio, IdEmpresa = idEmpresa })).ToList();
    }

    public async Task<int> CriarAsync(Oportunidade oportunidade)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            INSERT INTO oportunidade (id_empresa, id_estabelecimento, cnpj_empresa,
                                      titulo, id_parceiro, id_estagio, valor,
                                      data_previsao, responsavel_id, observacao)
            VALUES (@IdEmpresa, @IdEstabelecimento, @CnpjEmpresa,
                    @Titulo, @IdParceiro, @IdEstagio, @Valor,
                    @DataPrevisao, @ResponsavelId, @Observacao)
            RETURNING id";

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            oportunidade.IdEmpresa,
            oportunidade.IdEstabelecimento,
            oportunidade.CnpjEmpresa,
            oportunidade.Titulo,
            oportunidade.IdParceiro,
            oportunidade.IdEstagio,
            oportunidade.Valor,
            oportunidade.DataPrevisao,
            oportunidade.ResponsavelId,
            oportunidade.Observacao
        });
    }

    public async Task AtualizarAsync(Oportunidade oportunidade)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            UPDATE oportunidade 
            SET titulo = @Titulo,
                id_parceiro = @IdParceiro,
                id_estagio = @IdEstagio,
                valor = @Valor,
                data_previsao = @DataPrevisao,
                responsavel_id = @ResponsavelId,
                observacao = @Observacao
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        await connection.ExecuteAsync(sql, new
        {
            oportunidade.Id,
            oportunidade.IdEmpresa,
            oportunidade.Titulo,
            oportunidade.IdParceiro,
            oportunidade.IdEstagio,
            oportunidade.Valor,
            oportunidade.DataPrevisao,
            oportunidade.ResponsavelId,
            oportunidade.Observacao
        });
    }
}
