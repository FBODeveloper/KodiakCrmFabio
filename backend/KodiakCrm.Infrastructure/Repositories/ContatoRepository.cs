using Dapper;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.Infrastructure.Repositories;

public class ContatoRepository : IContatoRepository
{
    private readonly IDatabaseConnection _database;

    public ContatoRepository(IDatabaseConnection database)
    {
        _database = database;
    }

    public async Task<Contato?> ObterPorIdAsync(int id, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT ct.id, ct.id_empresa, ct.id_estabelecimento, ct.cnpj_empresa,
                   ct.nome, ct.cargo, ct.email, ct.telefone, ct.celular,
                   ct.id_cliente, ct.id_parceiro, ct.observacao,
                   ct.responsavel_id, ct.ativo, ct.data_cadastro,
                   u.nome as responsavel_nome,
                   cl.razao_social as cliente_nome,
                   p.razao_social as parceiro_nome
            FROM contato ct
            LEFT JOIN usuario u ON ct.responsavel_id = u.id
            LEFT JOIN cliente cl ON ct.id_cliente = cl.id
            LEFT JOIN parceiro p ON ct.id_parceiro = p.id
            WHERE ct.id = @Id AND ct.id_empresa = @IdEmpresa";

        return await connection.QueryFirstOrDefaultAsync<Contato>(sql, new { Id = id, IdEmpresa = idEmpresa });
    }

    public async Task<ContatoListResult> ObterListaAsync(string idEmpresa, string? busca, int? idCliente, int? idParceiro, int pagina, int itensPorPagina)
    {
        using var connection = _database.GetConnection();

        var whereClause = "WHERE ct.id_empresa = @IdEmpresa AND ct.ativo = true";
        var parameters = new DynamicParameters();
        parameters.Add("IdEmpresa", idEmpresa);

        if (!string.IsNullOrWhiteSpace(busca))
        {
            whereClause += " AND (ct.nome ILIKE @Busca OR ct.email ILIKE @Busca OR ct.cargo ILIKE @Busca)";
            parameters.Add("Busca", $"%{busca}%");
        }

        if (idCliente.HasValue)
        {
            whereClause += " AND ct.id_cliente = @IdCliente";
            parameters.Add("IdCliente", idCliente.Value);
        }

        if (idParceiro.HasValue)
        {
            whereClause += " AND ct.id_parceiro = @IdParceiro";
            parameters.Add("IdParceiro", idParceiro.Value);
        }

        var countSql = $"SELECT COUNT(*) FROM contato ct {whereClause}";
        var total = await connection.ExecuteScalarAsync<int>(countSql, parameters);

        var offset = (pagina - 1) * itensPorPagina;
        parameters.Add("Offset", offset);
        parameters.Add("Limit", itensPorPagina);

        var sql = $@"
            SELECT ct.id, ct.id_empresa, ct.id_estabelecimento, ct.cnpj_empresa,
                   ct.nome, ct.cargo, ct.email, ct.telefone, ct.celular,
                   ct.id_cliente, ct.id_parceiro, ct.observacao,
                   ct.responsavel_id, ct.ativo, ct.data_cadastro,
                   u.nome as responsavel_nome,
                   cl.razao_social as cliente_nome,
                   p.razao_social as parceiro_nome
            FROM contato ct
            LEFT JOIN usuario u ON ct.responsavel_id = u.id
            LEFT JOIN cliente cl ON ct.id_cliente = cl.id
            LEFT JOIN parceiro p ON ct.id_parceiro = p.id
            {whereClause}
            ORDER BY ct.nome
            LIMIT @Limit OFFSET @Offset";

        var itens = (await connection.QueryAsync<Contato>(sql, parameters)).ToList();

        return new ContatoListResult
        {
            Itens = itens,
            Total = total
        };
    }

    public async Task<List<Contato>> ObterPorClienteAsync(int idCliente, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT ct.id, ct.id_empresa, ct.id_estabelecimento, ct.cnpj_empresa,
                   ct.nome, ct.cargo, ct.email, ct.telefone, ct.celular,
                   ct.id_cliente, ct.id_parceiro, ct.observacao,
                   ct.responsavel_id, ct.ativo, ct.data_cadastro,
                   u.nome as responsavel_nome,
                   cl.razao_social as cliente_nome,
                   p.razao_social as parceiro_nome
            FROM contato ct
            LEFT JOIN usuario u ON ct.responsavel_id = u.id
            LEFT JOIN cliente cl ON ct.id_cliente = cl.id
            LEFT JOIN parceiro p ON ct.id_parceiro = p.id
            WHERE ct.id_cliente = @IdCliente AND ct.id_empresa = @IdEmpresa AND ct.ativo = true
            ORDER BY ct.nome";

        return (await connection.QueryAsync<Contato>(sql, new { IdCliente = idCliente, IdEmpresa = idEmpresa })).ToList();
    }

    public async Task<List<Contato>> ObterPorParceiroAsync(int idParceiro, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT ct.id, ct.id_empresa, ct.id_estabelecimento, ct.cnpj_empresa,
                   ct.nome, ct.cargo, ct.email, ct.telefone, ct.celular,
                   ct.id_cliente, ct.id_parceiro, ct.observacao,
                   ct.responsavel_id, ct.ativo, ct.data_cadastro,
                   u.nome as responsavel_nome,
                   cl.razao_social as cliente_nome,
                   p.razao_social as parceiro_nome
            FROM contato ct
            LEFT JOIN usuario u ON ct.responsavel_id = u.id
            LEFT JOIN cliente cl ON ct.id_cliente = cl.id
            LEFT JOIN parceiro p ON ct.id_parceiro = p.id
            WHERE ct.id_parceiro = @IdParceiro AND ct.id_empresa = @IdEmpresa AND ct.ativo = true
            ORDER BY ct.nome";

        return (await connection.QueryAsync<Contato>(sql, new { IdParceiro = idParceiro, IdEmpresa = idEmpresa })).ToList();
    }

    public async Task<int> CriarAsync(Contato contato)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            INSERT INTO contato (id_empresa, id_estabelecimento, cnpj_empresa,
                                 nome, cargo, email, telefone, celular,
                                 id_cliente, id_parceiro, observacao, responsavel_id)
            VALUES (@IdEmpresa, @IdEstabelecimento, @CnpjEmpresa,
                    @Nome, @Cargo, @Email, @Telefone, @Celular,
                    @IdCliente, @IdParceiro, @Observacao, @ResponsavelId)
            RETURNING id";

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            contato.IdEmpresa,
            contato.IdEstabelecimento,
            contato.CnpjEmpresa,
            contato.Nome,
            contato.Cargo,
            contato.Email,
            contato.Telefone,
            contato.Celular,
            contato.IdCliente,
            contato.IdParceiro,
            contato.Observacao,
            contato.ResponsavelId
        });
    }

    public async Task AtualizarAsync(Contato contato)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            UPDATE contato 
            SET nome = @Nome,
                cargo = @Cargo,
                email = @Email,
                telefone = @Telefone,
                celular = @Celular,
                id_cliente = @IdCliente,
                id_parceiro = @IdParceiro,
                observacao = @Observacao,
                responsavel_id = @ResponsavelId
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        await connection.ExecuteAsync(sql, new
        {
            contato.Id,
            contato.IdEmpresa,
            contato.Nome,
            contato.Cargo,
            contato.Email,
            contato.Telefone,
            contato.Celular,
            contato.IdCliente,
            contato.IdParceiro,
            contato.Observacao,
            contato.ResponsavelId
        });
    }

    public async Task ExcluirAsync(int id, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            UPDATE contato SET ativo = false
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        await connection.ExecuteAsync(sql, new { Id = id, IdEmpresa = idEmpresa });
    }
}
