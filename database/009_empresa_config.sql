-- Migração 009: Configurações da empresa
-- Adiciona colunas de contato na empresa + tabela empresa_config

-- Adicionar colunas na tabela empresa
ALTER TABLE empresa ADD COLUMN IF NOT EXISTS telefone VARCHAR(20);
ALTER TABLE empresa ADD COLUMN IF NOT EXISTS email VARCHAR(255);
ALTER TABLE empresa ADD COLUMN IF NOT EXISTS endereco VARCHAR(500);

-- Tabela empresa_config (configurações do sistema por empresa)
CREATE TABLE IF NOT EXISTS empresa_config (
    id SERIAL PRIMARY KEY,
    cnpj_empresa VARCHAR(18) NOT NULL REFERENCES empresa(cnpj),
    tema VARCHAR(20) NOT NULL DEFAULT 'light',
    fuso_horario VARCHAR(50) NOT NULL DEFAULT 'America/Sao_Paulo',
    moeda VARCHAR(10) NOT NULL DEFAULT 'BRL',
    idioma VARCHAR(10) NOT NULL DEFAULT 'pt-BR',
    notificacoes_email BOOLEAN NOT NULL DEFAULT true,
    notificacoes_sistema BOOLEAN NOT NULL DEFAULT true,
    data_atualizacao TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    CONSTRAINT uk_empresa_config UNIQUE (cnpj_empresa)
);

-- Comentários
COMMENT ON TABLE empresa_config IS 'Configurações do sistema por empresa';
COMMENT ON COLUMN empresa_config.cnpj_empresa IS 'FK para empresa (CNPJ)';
COMMENT ON COLUMN empresa_config.tema IS 'Tema da interface: light ou dark';
COMMENT ON COLUMN empresa_config.fuso_horario IS 'Fuso horário da empresa';
COMMENT ON COLUMN empresa_config.moeda IS 'Moeda padrão (BRL, USD, EUR)';
COMMENT ON COLUMN empresa_config.idioma IS 'Idioma da interface';

-- Inserir config padrão para empresas existentes sem config
INSERT INTO empresa_config (cnpj_empresa)
SELECT cnpj FROM empresa
WHERE NOT EXISTS (SELECT 1 FROM empresa_config WHERE cnpj_empresa = empresa.cnpj)
ON CONFLICT DO NOTHING;
