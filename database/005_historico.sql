-- Migration 005: Tabela de histórico imutável
-- Toda ação do CRM gera um registro nesta tabela (append-only, sem UPDATE/DELETE)

CREATE TABLE IF NOT EXISTS historico (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(100) NOT NULL,
    id_estabelecimento VARCHAR(100),
    cnpj_empresa VARCHAR(100),
    
    -- O que foi alterado
    entidade VARCHAR(50) NOT NULL,       -- 'lead', 'oportunidade', 'proposta', 'atividade', 'parceiro'
    entidade_id INTEGER NOT NULL,        -- ID do registro alterado
    
    -- O que aconteceu
    acao VARCHAR(50) NOT NULL,           -- 'criado', 'alterado', 'etapa_alterada', 'status_alterado', 'excluido', 'convertido'
    descricao TEXT NOT NULL,             -- 'Lead Marcus Johnson movido para Qualificado'
    
    -- Dados antes e depois (JSON)
    dados_antes JSONB,                  -- snapshot anterior (nullable para 'criado')
    dados_depois JSONB,                 -- snapshot posterior (nullable para 'excluido')
    
    -- Quem fez
    usuario_id INTEGER,
    usuario_nome VARCHAR(200),
    
    -- Quando
    data_acao TIMESTAMP NOT NULL DEFAULT NOW(),
    
    -- Multi-tenant
    CONSTRAINT fk_historico_empresa FOREIGN KEY (id_empresa) 
        REFERENCES empresa(cnpj) ON DELETE RESTRICT
);

-- Índices para performance
CREATE INDEX IF NOT EXISTS idx_historico_empresa ON historico(id_empresa);
CREATE INDEX IF NOT EXISTS idx_historico_entidade ON historico(entidade, entidade_id);
CREATE INDEX IF NOT EXISTS idx_historico_data ON historico(data_acao DESC);

-- Comentários
COMMENT ON TABLE historico IS 'Log imutável de todas as ações do CRM (append-only)';
COMMENT ON COLUMN historico.acao IS 'Tipo da ação: criado, alterado, etapa_alterada, status_alterado, excluido, convertido';
COMMENT ON COLUMN historico.dados_antes IS 'Snapshot JSON do estado anterior (NULL para criação)';
COMMENT ON COLUMN historico.dados_depois IS 'Snapshot JSON do estado posterior (NULL para exclusão)';
