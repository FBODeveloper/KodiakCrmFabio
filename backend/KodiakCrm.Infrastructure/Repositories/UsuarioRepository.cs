using Dapper;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IDatabaseConnection _database;

    public UsuarioRepository(IDatabaseConnection database)
    {
        _database = database;
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, id_estabelecimento, cnpj_empresa, 
                   id_usuario_kodiak, nome, email, senha_hash, ativo, data_cadastro,
                   avatar, data_nascimento, perfil
            FROM usuario 
            WHERE email = @Email AND id_empresa = @IdEmpresa AND ativo = true";

        return await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email, IdEmpresa = idEmpresa });
    }

    public async Task<Usuario?> ObterPorIdAsync(int id, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            SELECT id, id_empresa, id_estabelecimento, cnpj_empresa, 
                   id_usuario_kodiak, nome, email, senha_hash, ativo, data_cadastro,
                   avatar, data_nascimento, perfil
            FROM usuario 
            WHERE id = @Id AND id_empresa = @IdEmpresa AND ativo = true";

        return await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Id = id, IdEmpresa = idEmpresa });
    }

    public async Task<int> CriarAsync(Usuario usuario)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            INSERT INTO usuario (id_empresa, id_estabelecimento, cnpj_empresa, id_usuario_kodiak, 
                                 nome, email, senha_hash, ativo, avatar, data_nascimento, perfil)
            VALUES (@IdEmpresa, @IdEstabelecimento, @CnpjEmpresa, @IdUsuarioKodiak,
                    @Nome, @Email, @SenhaHash, @Ativo, @Avatar, @DataNascimento, @Perfil)
            RETURNING id";

        return await connection.ExecuteScalarAsync<int>(sql, usuario);
    }

    public async Task<UsuarioListResult> ObterListaAsync(string idEmpresa, string? busca, string? perfil, int pagina, int itensPorPagina)
    {
        using var connection = _database.GetConnection();

        var whereClause = "WHERE id_empresa = @IdEmpresa AND ativo = true";
        var parameters = new DynamicParameters();
        parameters.Add("IdEmpresa", idEmpresa);

        if (!string.IsNullOrWhiteSpace(busca))
        {
            whereClause += " AND (nome ILIKE @Busca OR email ILIKE @Busca)";
            parameters.Add("Busca", $"%{busca}%");
        }

        if (!string.IsNullOrWhiteSpace(perfil))
        {
            whereClause += " AND perfil = @Perfil";
            parameters.Add("Perfil", perfil);
        }

        var countSql = $"SELECT COUNT(*) FROM usuario {whereClause}";
        var total = await connection.ExecuteScalarAsync<int>(countSql, parameters);

        var offset = (pagina - 1) * itensPorPagina;
        parameters.Add("Offset", offset);
        parameters.Add("Limit", itensPorPagina);

        var sql = $@"
            SELECT id, id_empresa, id_estabelecimento, cnpj_empresa, 
                   id_usuario_kodiak, nome, email, senha_hash, ativo, data_cadastro,
                   avatar, data_nascimento, perfil
            FROM usuario 
            {whereClause}
            ORDER BY nome
            LIMIT @Limit OFFSET @Offset";

        var itens = (await connection.QueryAsync<Usuario>(sql, parameters)).ToList();

        return new UsuarioListResult
        {
            Itens = itens,
            Total = total
        };
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        using var connection = _database.GetConnection();
        const string sql = @"
            UPDATE usuario 
            SET nome = @Nome,
                email = @Email,
                perfil = @Perfil,
                avatar = @Avatar,
                data_nascimento = @DataNascimento,
                ativo = @Ativo
            WHERE id = @Id AND id_empresa = @IdEmpresa";

        await connection.ExecuteAsync(sql, new
        {
            usuario.Id,
            usuario.IdEmpresa,
            usuario.Nome,
            usuario.Email,
            usuario.Perfil,
            usuario.Avatar,
            usuario.DataNascimento,
            usuario.Ativo
        });

        if (!string.IsNullOrEmpty(usuario.SenhaHash))
        {
            const string updateSenha = "UPDATE usuario SET senha_hash = @SenhaHash WHERE id = @Id";
            await connection.ExecuteAsync(updateSenha, new { usuario.Id, usuario.SenhaHash });
        }
    }

    public async Task ExcluirAsync(int id, string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = "UPDATE usuario SET ativo = false WHERE id = @Id AND id_empresa = @IdEmpresa";

        await connection.ExecuteAsync(sql, new { Id = id, IdEmpresa = idEmpresa });
    }

    public async Task<int> ContarPorEmpresaAsync(string idEmpresa)
    {
        using var connection = _database.GetConnection();
        const string sql = "SELECT COUNT(*) FROM usuario WHERE id_empresa = @IdEmpresa AND ativo = true";

        return await connection.ExecuteScalarAsync<int>(sql, new { IdEmpresa = idEmpresa });
    }
}
