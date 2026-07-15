-- =====================================================
-- KODIAK CRM - Script 003: Empresa, Perfil de Usuários
-- =====================================================

-- =====================================================
-- TABELA: empresa
-- =====================================================
CREATE TABLE IF NOT EXISTS empresa (
    cnpj VARCHAR(14) PRIMARY KEY,
    razao_social VARCHAR(255) NOT NULL,
    nome_fantasia VARCHAR(255),
    quantidade_usuarios_contratados INTEGER DEFAULT 1,
    ativo BOOLEAN DEFAULT true,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- =====================================================
-- NOVAS COLUNAS NA TABELA: usuario
-- =====================================================
ALTER TABLE usuario ADD COLUMN IF NOT EXISTS avatar TEXT;
ALTER TABLE usuario ADD COLUMN IF NOT EXISTS data_nascimento DATE;
ALTER TABLE usuario ADD COLUMN IF NOT EXISTS perfil VARCHAR(20) DEFAULT 'usuario';

-- =====================================================
-- MIGRAÇÃO: Inserir empresa do usuário existente
-- =====================================================
INSERT INTO empresa (cnpj, razao_social, nome_fantasia, quantidade_usuarios_contratados)
VALUES ('13452799000107', 'Conttrotech Sistemas de Gestão', 'Conttrotech', 10)
ON CONFLICT (cnpj) DO NOTHING;

-- =====================================================
-- MIGRAÇÃO: Atualizar usuário existente
-- =====================================================
UPDATE usuario SET
    id_empresa = '13452799000107',
    id_estabelecimento = '13452799000107',
    cnpj_empresa = '13452799000107',
    perfil = 'admin'
WHERE id = 1;
