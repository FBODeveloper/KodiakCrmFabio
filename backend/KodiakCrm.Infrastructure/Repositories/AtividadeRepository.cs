using Dapper;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.Infrastructure.Repositories;

public class AtividadeRepository : IAtividadeRepository
{
    private readonly IDatabaseConnection _database;

    public AtividadeRepository(IDatabaseConnection database)
    {
        _database = database;
    }

    public async Task<Atividade?> ObterPorIdAsync(int id, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, id_estabelecimento, cnpj_empresa,
                   tipo, titulo, descricao, id_parceiro, id_oportunidade,
                   responsavel_id, data_inicio, data_fim, concluida, ativo, data_cadastro
            FROM atividade 
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        return await connection.QueryFirstOrDefaultAsync<Atividade>(sql, new { Id = id, IdEmpresa = idEmpresa });
    }

    public async Task<AtividadeListResult> ObterListaAsync(string idEmpresa, string? busca, string? tipo, int? idParceiro, int? idOportunidade, int? responsavelId, bool? concluida, int pagina, int itensPorPagina)
    {
        using var connection = _database.GetConnection();

        var whereClause = "WHERE a.id_empresa = @IdEmpresa AND a.ativo = true";
        var parameters = new DynamicParameters();
        parameters.Add("IdEmpresa", idEmpresa);

        if (!string.IsNullOrWhiteSpace(busca))
        {
            whereClause += " AND (a.titulo ILIKE @Busca OR a.descricao ILIKE @Busca)";
            parameters.Add("Busca", $"%{busca}%");
        }

        if (!string.IsNullOrWhiteSpace(tipo))
        {
            whereClause += " AND a.tipo = @Tipo";
            parameters.Add("Tipo", tipo);
        }

        if (idParceiro.HasValue)
        {
            whereClause += " AND a.id_parceiro = @IdParceiro";
            parameters.Add("IdParceiro", idParceiro.Value);
        }

        if (idOportunidade.HasValue)
        {
            whereClause += " AND a.id_oportunidade = @IdOportunidade";
            parameters.Add("IdOportunidade", idOportunidade.Value);
        }

        if (responsavelId.HasValue)
        {
            whereClause += " AND a.responsavel_id = @ResponsavelId";
            parameters.Add("ResponsavelId", responsavelId.Value);
        }

        if (concluida.HasValue)
        {
            whereClause += " AND a.concluida = @Concluida";
            parameters.Add("Concluida", concluida.Value);
        }

        var countSql = $"SELECT COUNT(*) FROM atividade a {whereClause}";
        var total = await connection.ExecuteScalarAsync<int>(countSql, parameters);

        var offset = (pagina - 1) * itensPorPagina;
        parameters.Add("Offset", offset);
        parameters.Add("Limit", itensPorPagina);

        var sql = $@"
            SELECT a.id, a.id_empresa, a.id_estabelecimento, a.cnpj_empresa,
                   a.tipo, a.titulo, a.descricao, a.id_parceiro, a.id_oportunidade,
                   a.responsavel_id, a.data_inicio, a.data_fim, a.concluida, a.ativo, a.data_cadastro,
                   p.razao_social as parceiro_nome,
                   o.titulo as oportunidade_titulo,
                   u.nome as responsavel_nome
            FROM atividade a
            LEFT JOIN parceiro p ON a.id_parceiro = p.id
            LEFT JOIN oportunidade o ON a.id_oportunidade = o.id
            LEFT JOIN usuario u ON a.responsavel_id = u.id
            {whereClause}
            ORDER BY a.data_cadastro DESC
            LIMIT @Limit OFFSET @Offset";

        var itens = (await connection.QueryAsync<Atividade>(sql, parameters)).ToList();

        return new AtividadeListResult
        {
            Itens = itens,
            Total = total
        };
    }

    public async Task<List<Atividade>> ObterPorParceiroAsync(int idParceiro, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, id_estabelecimento, cnpj_empresa,
                   tipo, titulo, descricao, id_parceiro, id_oportunidade,
                   responsavel_id, data_inicio, data_fim, concluida, ativo, data_cadastro
            FROM atividade 
            WHERE id_parceiro = @IdParceiro AND id_empresa = @IdEmpresa AND ativo = true
            ORDER BY data_cadastro DESC";

        return (await connection.QueryAsync<Atividade>(sql, new { IdParceiro = idParceiro, IdEmpresa = idEmpresa })).ToList();
    }

    public async Task<int> CriarAsync(Atividade atividade)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            INSERT INTO atividade (id_empresa, id_estabelecimento, cnpj_empresa,
                                   tipo, titulo, descricao, id_parceiro, id_oportunidade,
                                   responsavel_id, data_inicio, data_fim, concluida)
            VALUES (@IdEmpresa, @IdEstabelecimento, @CnpjEmpresa,
                    @Tipo, @Titulo, @Descricao, @IdParceiro, @IdOportunidade,
                    @ResponsavelId, @DataInicio, @DataFim, @Concluida)
            RETURNING id";

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            atividade.IdEmpresa,
            atividade.IdEstabelecimento,
            atividade.CnpjEmpresa,
            atividade.Tipo,
            atividade.Titulo,
            atividade.Descricao,
            atividade.IdParceiro,
            atividade.IdOportunidade,
            atividade.ResponsavelId,
            atividade.DataInicio,
            atividade.DataFim,
            atividade.Concluida
        });
    }

    public async Task AtualizarAsync(Atividade atividade)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            UPDATE atividade 
            SET tipo = @Tipo,
                titulo = @Titulo,
                descricao = @Descricao,
                id_parceiro = @IdParceiro,
                id_oportunidade = @IdOportunidade,
                responsavel_id = @ResponsavelId,
                data_inicio = @DataInicio,
                data_fim = @DataFim,
                concluida = @Concluida
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        await connection.ExecuteAsync(sql, new
        {
            atividade.Id,
            atividade.IdEmpresa,
            atividade.Tipo,
            atividade.Titulo,
            atividade.Descricao,
            atividade.IdParceiro,
            atividade.IdOportunidade,
            atividade.ResponsavelId,
            atividade.DataInicio,
            atividade.DataFim,
            atividade.Concluida
        });
    }
}
