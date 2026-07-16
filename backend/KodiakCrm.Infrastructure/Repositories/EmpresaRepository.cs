using Dapper;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.Infrastructure.Repositories;

public class EmpresaRepository : IEmpresaRepository
{
    private readonly IDatabaseConnection _database;

    public EmpresaRepository(IDatabaseConnection database)
    {
        _database = database;
    }

    public async Task<Empresa?> ObterPorCnpjAsync(string cnpj)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT cnpj, razao_social, nome_fantasia, telefone, email, endereco,
                   quantidade_usuarios_contratados, ativo, data_cadastro
            FROM empresa 
            WHERE cnpj = @Cnpj AND ativo = true";

        return await connection.QueryFirstOrDefaultAsync<Empresa>(sql, new { Cnpj = cnpj });
    }

    public async Task<EmpresaListResult> ObterListaAsync(string? busca, int pagina, int itensPorPagina)
    {
        using var connection = _database.GetConnection();

        var whereClause = "WHERE ativo = true";
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            whereClause += " AND (razao_social ILIKE @Busca OR nome_fantasia ILIKE @Busca OR cnpj ILIKE @Busca)";
            parameters.Add("Busca", $"%{busca}%");
        }

        var countSql = $"SELECT COUNT(*) FROM empresa {whereClause}";
        var total = await connection.ExecuteScalarAsync<int>(countSql, parameters);

        var offset = (pagina - 1) * itensPorPagina;
        parameters.Add("Offset", offset);
        parameters.Add("Limit", itensPorPagina);

        var sql = $@"
            SELECT cnpj, razao_social, nome_fantasia, telefone, email, endereco,
                   quantidade_usuarios_contratados, ativo, data_cadastro
            FROM empresa 
            {whereClause}
            ORDER BY razao_social
            LIMIT @Limit OFFSET @Offset";

        var itens = (await connection.QueryAsync<Empresa>(sql, parameters)).ToList();

        return new EmpresaListResult
        {
            Itens = itens,
            Total = total
        };
    }

    public async Task CriarAsync(Empresa empresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            INSERT INTO empresa (cnpj, razao_social, nome_fantasia, telefone, email, endereco, quantidade_usuarios_contratados)
            VALUES (@Cnpj, @RazaoSocial, @NomeFantasia, @Telefone, @Email, @Endereco, @QuantidadeUsuariosContratados)";

        await connection.ExecuteAsync(sql, new
        {
            empresa.Cnpj,
            empresa.RazaoSocial,
            empresa.NomeFantasia,
            empresa.Telefone,
            empresa.Email,
            empresa.Endereco,
            empresa.QuantidadeUsuariosContratados
        });
    }

    public async Task AtualizarAsync(Empresa empresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            UPDATE empresa 
            SET razao_social = @RazaoSocial,
                nome_fantasia = @NomeFantasia,
                telefone = @Telefone,
                email = @Email,
                endereco = @Endereco,
                quantidade_usuarios_contratados = @QuantidadeUsuariosContratados
            WHERE cnpj = @Cnpj";

        await connection.ExecuteAsync(sql, new
        {
            empresa.Cnpj,
            empresa.RazaoSocial,
            empresa.NomeFantasia,
            empresa.Telefone,
            empresa.Email,
            empresa.Endereco,
            empresa.QuantidadeUsuariosContratados
        });
    }

    public async Task<int> ContarUsuariosAsync(string cnpj)
    {
        using var connection = _database.GetConnection();
        const string sql = "SELECT COUNT(*) FROM usuario WHERE id_empresa = @IdEmpresa AND ativo = true";

        return await connection.ExecuteScalarAsync<int>(sql, new { IdEmpresa = cnpj });
    }
}
