-- =====================================================
-- KODIAK CRM - Script de Criação do Banco de Dados
-- =====================================================

-- Criar banco de dados (executar separadamente se necessário)
-- CREATE DATABASE kodiak_crm;

-- =====================================================
-- TABELA: usuario
-- =====================================================
CREATE TABLE IF NOT EXISTS usuario (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(100) NOT NULL,
    id_estabelecimento VARCHAR(100) NOT NULL,
    cnpj_empresa VARCHAR(100) NOT NULL,
    id_usuario_kodiak INTEGER,
    nome VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    senha_hash VARCHAR(255) NOT NULL,
    ativo BOOLEAN DEFAULT true,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT uk_usuario_email_empresa UNIQUE (email, id_empresa)
);

CREATE INDEX IF NOT EXISTS idx_usuario_empresa ON usuario(id_empresa);
CREATE INDEX IF NOT EXISTS idx_usuario_email ON usuario(email);

-- =====================================================
-- TABELA: parceiro
-- =====================================================
CREATE TABLE IF NOT EXISTS parceiro (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(100) NOT NULL,
    id_estabelecimento VARCHAR(100) NOT NULL,
    cnpj_empresa VARCHAR(100) NOT NULL,
    razao_social VARCHAR(255) NOT NULL,
    nome_fantasia VARCHAR(255),
    cpf_cnpj VARCHAR(14),
    tipo_pessoa CHAR(1) CHECK (tipo_pessoa IN ('J', 'F')),
    email VARCHAR(255),
    telefone VARCHAR(20),
    celular VARCHAR(20),
    id_parceiro_kodiakerp INTEGER,
    observacao TEXT,
    ativo BOOLEAN DEFAULT true,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IF NOT EXISTS idx_parceiro_empresa ON parceiro(id_empresa);
CREATE INDEX IF NOT EXISTS idx_parceiro_cpf_cnpj ON parceiro(cpf_cnpj);
CREATE INDEX IF NOT EXISTS idx_parceiro_kodiakerp ON parceiro(id_parceiro_kodiakerp);

-- =====================================================
-- TABELA: parceiro_endereco
-- =====================================================
CREATE TABLE IF NOT EXISTS parceiro_endereco (
    id SERIAL PRIMARY KEY,
    id_parceiro INTEGER NOT NULL REFERENCES parceiro(id) ON DELETE CASCADE,
    tipo_endereco VARCHAR(20) DEFAULT 'principal',
    logradouro VARCHAR(255),
    numero VARCHAR(20),
    complemento VARCHAR(100),
    bairro VARCHAR(100),
    id_municipio INTEGER,
    id_estado INTEGER,
    id_pais INTEGER,
    cep VARCHAR(8),
    principal BOOLEAN DEFAULT false
);

CREATE INDEX IF NOT EXISTS idx_parceiro_endereco_parceiro ON parceiro_endereco(id_parceiro);

-- =====================================================
-- TABELA: lead
-- =====================================================
CREATE TABLE IF NOT EXISTS lead (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(100) NOT NULL,
    id_estabelecimento VARCHAR(100) NOT NULL,
    cnpj_empresa VARCHAR(100) NOT NULL,
    nome VARCHAR(255) NOT NULL,
    empresa VARCHAR(255),
    email VARCHAR(255),
    telefone VARCHAR(20),
    source VARCHAR(100),
    status VARCHAR(50) DEFAULT 'novo',
    id_parceiro INTEGER REFERENCES parceiro(id),
    observacao TEXT,
    responsavel_id INTEGER REFERENCES usuario(id),
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IF NOT EXISTS idx_lead_empresa ON lead(id_empresa);
CREATE INDEX IF NOT EXISTS idx_lead_status ON lead(status);
CREATE INDEX IF NOT EXISTS idx_lead_responsavel ON lead(responsavel_id);

-- =====================================================
-- TABELA: funil
-- =====================================================
CREATE TABLE IF NOT EXISTS funil (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(100) NOT NULL,
    nome VARCHAR(100) NOT NULL,
    ativo BOOLEAN DEFAULT true
);

CREATE INDEX IF NOT EXISTS idx_funil_empresa ON funil(id_empresa);

-- =====================================================
-- TABELA: funil_estagio
-- =====================================================
CREATE TABLE IF NOT EXISTS funil_estagio (
    id SERIAL PRIMARY KEY,
    id_funil INTEGER NOT NULL REFERENCES funil(id) ON DELETE CASCADE,
    nome VARCHAR(100) NOT NULL,
    ordem INTEGER NOT NULL,
    probabilidade INTEGER DEFAULT 0 CHECK (probabilidade BETWEEN 0 AND 100)
);

CREATE INDEX IF NOT EXISTS idx_funil_estagio_funil ON funil_estagio(id_funil);

-- =====================================================
-- TABELA: oportunidade
-- =====================================================
CREATE TABLE IF NOT EXISTS oportunidade (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(100) NOT NULL,
    id_estabelecimento VARCHAR(100) NOT NULL,
    cnpj_empresa VARCHAR(100) NOT NULL,
    titulo VARCHAR(255) NOT NULL,
    id_parceiro INTEGER REFERENCES parceiro(id),
    id_estagio INTEGER REFERENCES funil_estagio(id),
    valor DECIMAL(15,2),
    data_previsao DATE,
    responsavel_id INTEGER REFERENCES usuario(id),
    observacao TEXT,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IF NOT EXISTS idx_oportunidade_empresa ON oportunidade(id_empresa);
CREATE INDEX IF NOT EXISTS idx_oportunidade_estagio ON oportunidade(id_estagio);
CREATE INDEX IF NOT EXISTS idx_oportunidade_responsavel ON oportunidade(responsavel_id);

-- =====================================================
-- TABELA: atividade
-- =====================================================
CREATE TABLE IF NOT EXISTS atividade (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(100) NOT NULL,
    id_estabelecimento VARCHAR(100) NOT NULL,
    cnpj_empresa VARCHAR(100) NOT NULL,
    tipo VARCHAR(50) NOT NULL,
    titulo VARCHAR(255) NOT NULL,
    descricao TEXT,
    id_parceiro INTEGER REFERENCES parceiro(id),
    id_oportunidade INTEGER REFERENCES oportunidade(id),
    responsavel_id INTEGER REFERENCES usuario(id),
    data_inicio TIMESTAMP,
    data_fim TIMESTAMP,
    concluida BOOLEAN DEFAULT false,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IF NOT EXISTS idx_atividade_empresa ON atividade(id_empresa);
CREATE INDEX IF NOT EXISTS idx_atividade_tipo ON atividade(tipo);
CREATE INDEX IF NOT EXISTS idx_atividade_parceiro ON atividade(id_parceiro);
CREATE INDEX IF NOT EXISTS idx_atividade_oportunidade ON atividade(id_oportunidade);
CREATE INDEX IF NOT EXISTS idx_atividade_responsavel ON atividade(responsavel_id);

-- =====================================================
-- TABELA: proposta
-- =====================================================
CREATE TABLE IF NOT EXISTS proposta (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(100) NOT NULL,
    id_estabelecimento VARCHAR(100) NOT NULL,
    cnpj_empresa VARCHAR(100) NOT NULL,
    titulo VARCHAR(255) NOT NULL,
    id_parceiro INTEGER REFERENCES parceiro(id),
    id_oportunidade INTEGER REFERENCES oportunidade(id),
    valor_total DECIMAL(15,2),
    data_validade DATE,
    status VARCHAR(50) DEFAULT 'rascunho',
    observacao TEXT,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IF NOT EXISTS idx_proposta_empresa ON proposta(id_empresa);
CREATE INDEX IF NOT EXISTS idx_proposta_parceiro ON proposta(id_parceiro);
CREATE INDEX IF NOT EXISTS idx_proposta_status ON proposta(status);

-- =====================================================
-- TABELA: proposta_item
-- =====================================================
CREATE TABLE IF NOT EXISTS proposta_item (
    id SERIAL PRIMARY KEY,
    id_proposta INTEGER NOT NULL REFERENCES proposta(id) ON DELETE CASCADE,
    descricao VARCHAR(255) NOT NULL,
    quantidade INTEGER NOT NULL,
    valor_unitario DECIMAL(15,2) NOT NULL,
    valor_total DECIMAL(15,2) NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_proposta_item_proposta ON proposta_item(id_proposta);
