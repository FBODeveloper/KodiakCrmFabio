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
                   titulo, numero, data_proposta, forma_pagamento, prazo_entrega,
                   id_parceiro, id_oportunidade, cliente_id, contato_id,
                   valor_total, data_validade, status, observacao, ativo, data_cadastro
            FROM proposta 
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        return await connection.QueryFirstOrDefaultAsync<Proposta>(sql, new { Id = id, IdEmpresa = idEmpresa });
    }

    public async Task<PropostaListResult> ObterListaAsync(string idEmpresa, string? busca, string? status, int? idParceiro, int? clienteId, DateTime? dataInicio, DateTime? dataFim, int pagina, int itensPorPagina)
    {
        using var connection = _database.GetConnection();

        var whereClause = "WHERE pr.id_empresa = @IdEmpresa AND pr.ativo = true";
        var parameters = new DynamicParameters();
        parameters.Add("IdEmpresa", idEmpresa);

        if (!string.IsNullOrWhiteSpace(busca))
        {
            whereClause += " AND (pr.titulo ILIKE @Busca OR pr.numero ILIKE @Busca)";
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

        if (clienteId.HasValue)
        {
            whereClause += " AND pr.cliente_id = @ClienteId";
            parameters.Add("ClienteId", clienteId.Value);
        }

        if (dataInicio.HasValue)
        {
            whereClause += " AND pr.data_cadastro >= @DataInicio";
            parameters.Add("DataInicio", dataInicio.Value);
        }

        if (dataFim.HasValue)
        {
            whereClause += " AND pr.data_cadastro <= @DataFim";
            parameters.Add("DataFim", dataFim.Value.Date.AddDays(1).AddTicks(-1));
        }

        var countSql = $"SELECT COUNT(*) FROM proposta pr {whereClause}";
        var total = await connection.ExecuteScalarAsync<int>(countSql, parameters);

        var offset = (pagina - 1) * itensPorPagina;
        parameters.Add("Offset", offset);
        parameters.Add("Limit", itensPorPagina);

        var sql = $@"
            SELECT pr.id, pr.id_empresa, pr.id_estabelecimento, pr.cnpj_empresa,
                   pr.titulo, pr.numero, pr.data_proposta, pr.forma_pagamento, pr.prazo_entrega,
                   pr.id_parceiro, pr.id_oportunidade, pr.cliente_id, pr.contato_id,
                   pr.valor_total, pr.data_validade, pr.status, pr.observacao,
                   pr.ativo, pr.data_cadastro,
                   p.razao_social as parceiro_nome,
                   o.titulo as oportunidade_titulo,
                   c.razao_social as cliente_nome,
                   ct.nome as contato_nome
            FROM proposta pr
            LEFT JOIN parceiro p ON pr.id_parceiro = p.id
            LEFT JOIN oportunidade o ON pr.id_oportunidade = o.id
            LEFT JOIN cliente c ON pr.cliente_id = c.id
            LEFT JOIN contato ct ON pr.contato_id = ct.id
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

    public async Task<string> GerarProximoNumeroAsync(string idEmpresa)
    {
        using var connection = _database.GetConnection();
        var ano = DateTime.UtcNow.Year;

        const string sql = @"
            SELECT numero FROM proposta 
            WHERE id_empresa = @IdEmpresa AND numero LIKE @Padrao
            ORDER BY 
                CAST(SUBSTRING(numero FROM 1 FOR POSITION('/' IN numero) - 1) AS INTEGER) DESC
            LIMIT 1";

        var padrao = $"%/{ano}";
        var ultimoNumero = await connection.QueryFirstOrDefaultAsync<string>(sql, new { IdEmpresa = idEmpresa, Padrao = padrao });

        int proximo = 1;
        if (!string.IsNullOrEmpty(ultimoNumero))
        {
            var parteNumerica = ultimoNumero.Split('/').FirstOrDefault();
            if (int.TryParse(parteNumerica, out var ultimo))
            {
                proximo = ultimo + 1;
            }
        }

        return $"{proximo}/{ano}";
    }

    public async Task<int> CriarAsync(Proposta proposta)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            INSERT INTO proposta (id_empresa, id_estabelecimento, cnpj_empresa,
                                  titulo, numero, data_proposta, forma_pagamento, prazo_entrega,
                                  id_parceiro, id_oportunidade, cliente_id, contato_id,
                                  valor_total, data_validade, status, observacao)
            VALUES (@IdEmpresa, @IdEstabelecimento, @CnpjEmpresa,
                    @Titulo, @Numero, @DataProposta, @FormaPagamento, @PrazoEntrega,
                    @IdParceiro, @IdOportunidade, @ClienteId, @ContatoId,
                    @ValorTotal, @DataValidade, @Status, @Observacao)
            RETURNING id";

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            proposta.IdEmpresa,
            proposta.IdEstabelecimento,
            proposta.CnpjEmpresa,
            proposta.Titulo,
            proposta.Numero,
            DataProposta = proposta.DataProposta?.ToDateTime(TimeOnly.MinValue),
            proposta.FormaPagamento,
            proposta.PrazoEntrega,
            proposta.IdParceiro,
            proposta.IdOportunidade,
            proposta.ClienteId,
            proposta.ContatoId,
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
                data_proposta = @DataProposta,
                forma_pagamento = @FormaPagamento,
                prazo_entrega = @PrazoEntrega,
                id_parceiro = @IdParceiro,
                id_oportunidade = @IdOportunidade,
                cliente_id = @ClienteId,
                contato_id = @ContatoId,
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
            DataProposta = proposta.DataProposta?.ToDateTime(TimeOnly.MinValue),
            proposta.FormaPagamento,
            proposta.PrazoEntrega,
            proposta.IdParceiro,
            proposta.IdOportunidade,
            proposta.ClienteId,
            proposta.ContatoId,
            proposta.ValorTotal,
            proposta.DataValidade,
            proposta.Status,
            proposta.Observacao
        });
    }

    public async Task ExcluirAsync(int id, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = "UPDATE proposta SET ativo = false WHERE id = @Id AND id_empresa = @IdEmpresa";
        await connection.ExecuteAsync(sql, new { Id = id, IdEmpresa = idEmpresa });
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
