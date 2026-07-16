-- Fase 6: Tabela contato
CREATE TABLE contato (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(50) NOT NULL,
    id_estabelecimento VARCHAR(50) NOT NULL,
    cnpj_empresa VARCHAR(14) NOT NULL,
    nome VARCHAR(200) NOT NULL,
    cargo VARCHAR(100),
    email VARCHAR(200),
    telefone VARCHAR(20),
    celular VARCHAR(20),
    id_cliente INTEGER,
    id_parceiro INTEGER,
    observacao TEXT,
    responsavel_id INTEGER,
    ativo BOOLEAN DEFAULT true,
    data_cadastro TIMESTAMP DEFAULT NOW()
);

CREATE INDEX idx_contato_empresa ON contato(id_empresa);
CREATE INDEX idx_contato_cliente ON contato(id_cliente);
CREATE INDEX idx_contato_parceiro ON contato(id_parceiro);
CREATE INDEX idx_contato_ativo ON contato(ativo);

COMMENT ON TABLE contato IS 'Contatos vinculados a clientes ou parceiros';
COMMENT ON COLUMN contato.id_cliente IS 'ID do cliente ao qual este contato pertence';
COMMENT ON COLUMN contato.id_parceiro IS 'ID do parceiro ao qual este contato pertence';
