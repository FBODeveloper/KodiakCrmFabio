-- Migração 010: Relatórios
-- Tabela para salvar relatórios gerados + dados de relatório

-- Tabela de relatórios gerados
CREATE TABLE IF NOT EXISTS relatorio_gerado (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(50) NOT NULL,
    cnpj_empresa VARCHAR(18) NOT NULL,
    tipo VARCHAR(50) NOT NULL,
    titulo VARCHAR(255) NOT NULL,
    parametros JSONB,
    resultado JSONB NOT NULL,
    usuario_id INTEGER,
    usuario_nome VARCHAR(255),
    data_geracao TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    CONSTRAINT fk_relatorio_empresa FOREIGN KEY (cnpj_empresa) REFERENCES empresa(cnpj)
);

COMMENT ON TABLE relatorio_gerado IS 'Relatórios gerados e salvos no sistema';
COMMENT ON COLUMN relatorio_gerado.tipo IS 'Tipo: vendas, atividades, performance, funil';
COMMENT ON COLUMN relatorio_gerado.parametros IS 'Filtros aplicados no relatório (JSON)';
COMMENT ON COLUMN relatorio_gerado.resultado IS 'Dados do relatório (JSON)';

-- Índices
CREATE INDEX IF NOT EXISTS idx_relatorio_empresa ON relatorio_gerado(id_empresa);
CREATE INDEX IF NOT EXISTS idx_relatorio_tipo ON relatorio_gerado(tipo);
CREATE INDEX IF NOT EXISTS idx_relatorio_data ON relatorio_gerado(data_geracao DESC);
