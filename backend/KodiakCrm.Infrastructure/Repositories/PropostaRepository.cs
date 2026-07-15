using Dapper;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.Infrastructure.Repositories;

public class PropostaRepository : IPropostaRepository
{
    private readonly IDatabaseConnection _database;

    public PropostaRepository(IDatabaseConnection database)
    {
        _database = database;
    }

    public async Task<Proposta?> ObterPorIdAsync(int id, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, id_estabelecimento, cnpj_empresa,
                   titulo, id_parceiro, id_oportunidade, valor_total,
                   data_validade, status, observacao, ativo, data_cadastro
            FROM proposta 
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        return await connection.QueryFirstOrDefaultAsync<Proposta>(sql, new { Id = id, IdEmpresa = idEmpresa });
    }

    public async Task<PropostaListResult> ObterListaAsync(string idEmpresa, string? busca, string? status, int? idParceiro, int pagina, int itensPorPagina)
    {
        using var connection = _database.GetConnection();

        var whereClause = "WHERE pr.id_empresa = @IdEmpresa AND pr.ativo = true";
        var parameters = new DynamicParameters();
        parameters.Add("IdEmpresa", idEmpresa);

        if (!string.IsNullOrWhiteSpace(busca))
        {
            whereClause += " AND pr.titulo ILIKE @Busca";
            parameters.Add("Busca", $"%{busca}%");
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            whereClause += " AND pr.status = @Status";
            parameters.Add("Status", status);
        }

        if (idParceiro.HasValue)
        {
            whereClause += " AND pr.id_parceiro = @IdParceiro";
            parameters.Add("IdParceiro", idParceiro.Value);
        }

        var countSql = $"SELECT COUNT(*) FROM proposta pr {whereClause}";
        var total = await connection.ExecuteScalarAsync<int>(countSql, parameters);

        var offset = (pagina - 1) * itensPorPagina;
        parameters.Add("Offset", offset);
        parameters.Add("Limit", itensPorPagina);

        var sql = $@"
            SELECT pr.id, pr.id_empresa, pr.id_estabelecimento, pr.cnpj_empresa,
                   pr.titulo, pr.id_parceiro, pr.id_oportunidade, pr.valor_total,
                   pr.data_validade, pr.status, pr.observacao, pr.ativo, pr.data_cadastro,
                   p.razao_social as parceiro_nome,
                   o.titulo as oportunidade_titulo
            FROM proposta pr
            LEFT JOIN parceiro p ON pr.id_parceiro = p.id
            LEFT JOIN oportunidade o ON pr.id_oportunidade = o.id
            {whereClause}
            ORDER BY pr.data_cadastro DESC
            LIMIT @Limit OFFSET @Offset";

        var itens = (await connection.QueryAsync<Proposta>(sql, parameters)).ToList();

        return new PropostaListResult
        {
            Itens = itens,
            Total = total
        };
    }

    public async Task<List<PropostaItem>> ObterItensAsync(int idProposta)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_proposta, descricao, quantidade, valor_unitario, valor_total
            FROM proposta_item 
            WHERE id_proposta = @IdProposta
            ORDER BY id";

        return (await connection.QueryAsync<PropostaItem>(sql, new { IdProposta = idProposta })).ToList();
    }

    public async Task<int> CriarAsync(Proposta proposta)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            INSERT INTO proposta (id_empresa, id_estabelecimento, cnpj_empresa,
                                  titulo, id_parceiro, id_oportunidade, valor_total,
                                  data_validade, status, observacao)
            VALUES (@IdEmpresa, @IdEstabelecimento, @CnpjEmpresa,
                    @Titulo, @IdParceiro, @IdOportunidade, @ValorTotal,
                    @DataValidade, @Status, @Observacao)
            RETURNING id";

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            proposta.IdEmpresa,
            proposta.IdEstabelecimento,
            proposta.CnpjEmpresa,
            proposta.Titulo,
            proposta.IdParceiro,
            proposta.IdOportunidade,
            proposta.ValorTotal,
            proposta.DataValidade,
            proposta.Status,
            proposta.Observacao
        });
    }

    public async Task AtualizarAsync(Proposta proposta)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            UPDATE proposta 
            SET titulo = @Titulo,
                id_parceiro = @IdParceiro,
                id_oportunidade = @IdOportunidade,
                valor_total = @ValorTotal,
                data_validade = @DataValidade,
                status = @Status,
                observacao = @Observacao
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        await connection.ExecuteAsync(sql, new
        {
            proposta.Id,
            proposta.IdEmpresa,
            proposta.Titulo,
            proposta.IdParceiro,
            proposta.IdOportunidade,
            proposta.ValorTotal,
            proposta.DataValidade,
            proposta.Status,
            proposta.Observacao
        });
    }

    public async Task ExcluirItensAsync(int idProposta)
    {
        using var connection = _database.GetConnection();
        const string sql = "DELETE FROM proposta_item WHERE id_proposta = @IdProposta";
        await connection.ExecuteAsync(sql, new { IdProposta = idProposta });
    }

    public async Task<int> CriarItemAsync(PropostaItem item)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            INSERT INTO proposta_item (id_proposta, descricao, quantidade, valor_unitario, valor_total)
            VALUES (@IdProposta, @Descricao, @Quantidade, @ValorUnitario, @ValorTotal)
            RETURNING id";

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            item.IdProposta,
            item.Descricao,
            item.Quantidade,
            item.ValorUnitario,
            item.ValorTotal
        });
    }
}
