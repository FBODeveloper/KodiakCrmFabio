using Dapper;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly IDatabaseConnection _database;

    public ClienteRepository(IDatabaseConnection database)
    {
        _database = database;
    }

    public async Task<Cliente?> ObterPorIdAsync(int id, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT c.id, c.id_empresa, c.id_estabelecimento, c.cnpj_empresa,
                   c.razao_social, c.nome_fantasia, c.cnpj_cpf,
                   c.email, c.telefone, c.celular, c.endereco,
                   c.observacao, c.origem, c.data_conversao,
                   c.id_oportunidade, c.responsavel_id, c.ativo, c.data_cadastro,
                   u.nome as responsavel_nome
            FROM cliente c
            LEFT JOIN usuario u ON c.responsavel_id = u.id
            WHERE c.id = @Id AND c.id_empresa = @IdEmpresa";

        return await connection.QueryFirstOrDefaultAsync<Cliente>(sql, new { Id = id, IdEmpresa = idEmpresa });
    }

    public async Task<ClienteListResult> ObterListaAsync(string idEmpresa, string? busca, int pagina, int itensPorPagina)
    {
        using var connection = _database.GetConnection();

        var whereClause = "WHERE c.id_empresa = @IdEmpresa AND c.ativo = true";
        var parameters = new DynamicParameters();
        parameters.Add("IdEmpresa", idEmpresa);

        if (!string.IsNullOrWhiteSpace(busca))
        {
            whereClause += " AND (c.razao_social ILIKE @Busca OR c.nome_fantasia ILIKE @Busca OR c.cnpj_cpf ILIKE @Busca OR c.email ILIKE @Busca)";
            parameters.Add("Busca", $"%{busca}%");
        }

        var countSql = $"SELECT COUNT(*) FROM cliente c {whereClause}";
        var total = await connection.ExecuteScalarAsync<int>(countSql, parameters);

        var offset = (pagina - 1) * itensPorPagina;
        parameters.Add("Offset", offset);
        parameters.Add("Limit", itensPorPagina);

        var sql = $@"
            SELECT c.id, c.id_empresa, c.id_estabelecimento, c.cnpj_empresa,
                   c.razao_social, c.nome_fantasia, c.cnpj_cpf,
                   c.email, c.telefone, c.celular, c.endereco,
                   c.observacao, c.origem, c.data_conversao,
                   c.id_oportunidade, c.responsavel_id, c.ativo, c.data_cadastro,
                   u.nome as responsavel_nome
            FROM cliente c
            LEFT JOIN usuario u ON c.responsavel_id = u.id
            {whereClause}
            ORDER BY c.data_cadastro DESC
            LIMIT @Limit OFFSET @Offset";

        var itens = (await connection.QueryAsync<Cliente>(sql, parameters)).ToList();

        return new ClienteListResult
        {
            Itens = itens,
            Total = total
        };
    }

    public async Task<int> CriarAsync(Cliente cliente)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            INSERT INTO cliente (id_empresa, id_estabelecimento, cnpj_empresa,
                                 razao_social, nome_fantasia, cnpj_cpf,
                                 email, telefone, celular, endereco,
                                 observacao, origem, data_conversao,
                                 id_oportunidade, responsavel_id)
            VALUES (@IdEmpresa, @IdEstabelecimento, @CnpjEmpresa,
                    @RazaoSocial, @NomeFantasia, @CnpjCpf,
                    @Email, @Telefone, @Celular, @Endereco,
                    @Observacao, @Origem, @DataConversao,
                    @IdOportunidade, @ResponsavelId)
            RETURNING id";

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            cliente.IdEmpresa,
            cliente.IdEstabelecimento,
            cliente.CnpjEmpresa,
            cliente.RazaoSocial,
            cliente.NomeFantasia,
            cliente.CnpjCpf,
            cliente.Email,
            cliente.Telefone,
            cliente.Celular,
            cliente.Endereco,
            cliente.Observacao,
            cliente.Origem,
            cliente.DataConversao,
            cliente.IdOportunidade,
            cliente.ResponsavelId
        });
    }

    public async Task AtualizarAsync(Cliente cliente)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            UPDATE cliente 
            SET razao_social = @RazaoSocial,
                nome_fantasia = @NomeFantasia,
                cnpj_cpf = @CnpjCpf,
                email = @Email,
                telefone = @Telefone,
                celular = @Celular,
                endereco = @Endereco,
                observacao = @Observacao,
                responsavel_id = @ResponsavelId
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        await connection.ExecuteAsync(sql, new
        {
            cliente.Id,
            cliente.IdEmpresa,
            cliente.RazaoSocial,
            cliente.NomeFantasia,
            cliente.CnpjCpf,
            cliente.Email,
            cliente.Telefone,
            cliente.Celular,
            cliente.Endereco,
            cliente.Observacao,
            cliente.ResponsavelId
        });
    }

    public async Task ExcluirAsync(int id, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            UPDATE cliente SET ativo = false
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        await connection.ExecuteAsync(sql, new { Id = id, IdEmpresa = idEmpresa });
    }
}
