-- Fase 5: Tabela cliente
CREATE TABLE cliente (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(50) NOT NULL,
    id_estabelecimento VARCHAR(50) NOT NULL,
    cnpj_empresa VARCHAR(14) NOT NULL,
    razao_social VARCHAR(200) NOT NULL,
    nome_fantasia VARCHAR(200),
    cnpj_cpf VARCHAR(14),
    email VARCHAR(200),
    telefone VARCHAR(20),
    celular VARCHAR(20),
    endereco VARCHAR(500),
    observacao TEXT,
    origem VARCHAR(100),
    data_conversao TIMESTAMP,
    id_oportunidade INTEGER,
    responsavel_id INTEGER,
    ativo BOOLEAN DEFAULT true,
    data_cadastro TIMESTAMP DEFAULT NOW()
);

CREATE INDEX idx_cliente_empresa ON cliente(id_empresa);
CREATE INDEX idx_cliente_ativo ON cliente(ativo);
CREATE INDEX idx_cliente_oportunidade ON cliente(id_oportunidade);

COMMENT ON TABLE cliente IS 'Clientes convertidos de oportunidades ou cadastrados diretamente';
COMMENT ON COLUMN cliente.origem IS 'Origem do cliente: oportunidade, manual, importacao';
COMMENT ON COLUMN cliente.data_conversao IS 'Data em que a oportunidade foi ganha e o cliente criado';
COMMENT ON COLUMN cliente.id_oportunidade IS 'ID da oportunidade que gerou este cliente';
