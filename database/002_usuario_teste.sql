-- Script para inserir usuário de teste
-- Para gerar o hash da senha, use o endpoint POST /api/auth/hash-senha
-- ou execute no console C#:
--   using var sha256 = System.Security.Cryptography.SHA256.Create();
--   var hash = Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes("sua_senha")));
--   Console.WriteLine(hash);

INSERT INTO usuario (id_empresa, id_estabelecimento, cnpj_empresa, nome, email, senha_hash)
VALUES (
    'EMP001', 
    'EST001', 
    '12345678000190', 
    'Administrador', 
    'admin@kodiak.com', 
    'CBFDAC6008F9CAB4083784CBD1874F76618D2A97'
);
