using Dapper;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.Infrastructure.Repositories;

public class ParceiroRepository : IParceiroRepository
{
    private readonly IDatabaseConnection _database;

    public ParceiroRepository(IDatabaseConnection database)
    {
        _database = database;
    }

    public async Task<Parceiro?> ObterPorIdAsync(int id, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, id_estabelecimento, cnpj_empresa,
                   razao_social, nome_fantasia, cpf_cnpj, tipo_pessoa,
                   email, telefone, celular, id_parceiro_kodiakerp,
                   observacao, ativo, data_cadastro
            FROM parceiro 
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        return await connection.QueryFirstOrDefaultAsync<Parceiro>(sql, new { Id = id, IdEmpresa = idEmpresa });
    }

    public async Task<Parceiro?> ObterPorCpfCnpjAsync(string cpfCnpj, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, id_estabelecimento, cnpj_empresa,
                   razao_social, nome_fantasia, cpf_cnpj, tipo_pessoa,
                   email, telefone, celular, id_parceiro_kodiakerp,
                   observacao, ativo, data_cadastro
            FROM parceiro 
            WHERE cpf_cnpj = @CpfCnpj AND id_empresa = @IdEmpresa AND ativo = true";

        return await connection.QueryFirstOrDefaultAsync<Parceiro>(sql, new { CpfCnpj = cpfCnpj, IdEmpresa = idEmpresa });
    }

    public async Task<Parceiro?> ObterPorKodiakErpAsync(int idParceiroKodiakErp, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, id_estabelecimento, cnpj_empresa,
                   razao_social, nome_fantasia, cpf_cnpj, tipo_pessoa,
                   email, telefone, celular, id_parceiro_kodiakerp,
                   observacao, ativo, data_cadastro
            FROM parceiro 
            WHERE id_parceiro_kodiakerp = @IdParceiroKodiakErp AND id_empresa = @IdEmpresa";

        return await connection.QueryFirstOrDefaultAsync<Parceiro>(sql, new { IdParceiroKodiakErp = idParceiroKodiakErp, IdEmpresa = idEmpresa });
    }

    public async Task<ParceiroListResult> ObterListaAsync(string idEmpresa, string? busca, int pagina, int itensPorPagina)
    {
        using var connection = _database.GetConnection();

        var whereClause = "WHERE id_empresa = @IdEmpresa AND ativo = true";
        var parameters = new DynamicParameters();
        parameters.Add("IdEmpresa", idEmpresa);

        if (!string.IsNullOrWhiteSpace(busca))
        {
            whereClause += " AND (razao_social ILIKE @Busca OR nome_fantasia ILIKE @Busca OR cpf_cnpj ILIKE @Busca)";
            parameters.Add("Busca", $"%{busca}%");
        }

        var countSql = $"SELECT COUNT(*) FROM parceiro {whereClause}";
        var total = await connection.ExecuteScalarAsync<int>(countSql, parameters);

        var offset = (pagina - 1) * itensPorPagina;
        parameters.Add("Offset", offset);
        parameters.Add("Limit", itensPorPagina);

        var sql = $@"
            SELECT id, id_empresa, id_estabelecimento, cnpj_empresa,
                   razao_social, nome_fantasia, cpf_cnpj, tipo_pessoa,
                   email, telefone, celular, id_parceiro_kodiakerp,
                   observacao, ativo, data_cadastro
            FROM parceiro 
            {whereClause}
            ORDER BY razao_social
            LIMIT @Limit OFFSET @Offset";

        var itens = (await connection.QueryAsync<Parceiro>(sql, parameters)).ToList();

        return new ParceiroListResult
        {
            Itens = itens,
            Total = total
        };
    }

    public async Task<int> CriarAsync(Parceiro parceiro)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            INSERT INTO parceiro (id_empresa, id_estabelecimento, cnpj_empresa,
                                  razao_social, nome_fantasia, cpf_cnpj, tipo_pessoa,
                                  email, telefone, celular, id_parceiro_kodiakerp, observacao)
            VALUES (@IdEmpresa, @IdEstabelecimento, @CnpjEmpresa,
                    @RazaoSocial, @NomeFantasia, @CpfCnpj, @TipoPessoa,
                    @Email, @Telefone, @Celular, @IdParceiroKodiakErp, @Observacao)
            RETURNING id";

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            parceiro.IdEmpresa,
            parceiro.IdEstabelecimento,
            parceiro.CnpjEmpresa,
            parceiro.RazaoSocial,
            parceiro.NomeFantasia,
            parceiro.CpfCnpj,
            parceiro.TipoPessoa,
            parceiro.Email,
            parceiro.Telefone,
            parceiro.Celular,
            parceiro.IdParceiroKodiakErp,
            parceiro.Observacao
        });
    }

    public async Task AtualizarAsync(Parceiro parceiro)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            UPDATE parceiro 
            SET razao_social = @RazaoSocial,
                nome_fantasia = @NomeFantasia,
                cpf_cnpj = @CpfCnpj,
                tipo_pessoa = @TipoPessoa,
                email = @Email,
                telefone = @Telefone,
                celular = @Celular,
                observacao = @Observacao
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        await connection.ExecuteAsync(sql, new
        {
            parceiro.Id,
            parceiro.IdEmpresa,
            parceiro.RazaoSocial,
            parceiro.NomeFantasia,
            parceiro.CpfCnpj,
            parceiro.TipoPessoa,
            parceiro.Email,
            parceiro.Telefone,
            parceiro.Celular,
            parceiro.Observacao
        });
    }

    public async Task AtualizarKodiakErpAsync(int id, int idParceiroKodiakErp)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            UPDATE parceiro 
            SET id_parceiro_kodiakerp = @IdParceiroKodiakErp
            WHERE id = @Id";

        await connection.ExecuteAsync(sql, new { Id = id, IdParceiroKodiakErp = idParceiroKodiakErp });
    }

    public async Task<bool> ExisteCpfCnpjAsync(string cpfCnpj, string idEmpresa, int? idExcluir = null)
    {
        using var connection = _database.GetConnection();
        var sql = "SELECT COUNT(*) FROM parceiro WHERE cpf_cnpj = @CpfCnpj AND id_empresa = @IdEmpresa AND ativo = true";
        var parameters = new DynamicParameters();
        parameters.Add("CpfCnpj", cpfCnpj);
        parameters.Add("IdEmpresa", idEmpresa);

        if (idExcluir.HasValue)
        {
            sql += " AND id != @IdExcluir";
            parameters.Add("IdExcluir", idExcluir.Value);
        }

        var count = await connection.ExecuteScalarAsync<int>(sql, parameters);
        return count > 0;
    }
}
