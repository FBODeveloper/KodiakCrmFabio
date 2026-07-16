-- Migração 011: Notificações
-- Tabela de notificações in-app

CREATE TABLE IF NOT EXISTS notificacao (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(50) NOT NULL,
    usuario_id INTEGER NOT NULL,
    titulo VARCHAR(255) NOT NULL,
    mensagem TEXT NOT NULL,
    tipo VARCHAR(50) NOT NULL DEFAULT 'sistema',
    entidade VARCHAR(50),
    entidade_id INTEGER,
    lida BOOLEAN NOT NULL DEFAULT false,
    data_criacao TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    data_leitura TIMESTAMPTZ,
    CONSTRAINT fk_notificacao_empresa FOREIGN KEY (id_empresa) REFERENCES empresa(cnpj)
);

COMMENT ON TABLE notificacao IS 'Notificações in-app por usuário';
COMMENT ON COLUMN notificacao.tipo IS 'Tipo: sistema, followup, atividade, oportunidade, lead';
COMMENT ON COLUMN notificacao.lida IS 'Se o usuário já leu a notificação';

-- Índices
CREATE INDEX IF NOT EXISTS idx_notificacao_empresa ON notificacao(id_empresa);
CREATE INDEX IF NOT EXISTS idx_notificacao_usuario ON notificacao(usuario_id);
CREATE INDEX IF NOT EXISTS idx_notificacao_lida ON notificacao(usuario_id, lida);
CREATE INDEX IF NOT EXISTS idx_notificacao_data ON notificacao(data_criacao DESC);
