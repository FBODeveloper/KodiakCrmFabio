-- =====================================================
-- KODIAK CRM - Script 004: Lead Estágios e Temperatura
-- =====================================================

-- =====================================================
-- TABELA: lead_estagio (fluxo kanban por empresa)
-- =====================================================
CREATE TABLE IF NOT EXISTS lead_estagio (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(100) NOT NULL,
    nome VARCHAR(100) NOT NULL,
    ordem INTEGER NOT NULL,
    cor VARCHAR(7) DEFAULT '#3b82f6'
);

CREATE INDEX IF NOT EXISTS idx_lead_estagio_empresa ON lead_estagio(id_empresa);

-- =====================================================
-- NOVAS COLUNAS NA TABELA: lead
-- =====================================================
ALTER TABLE lead ADD COLUMN IF NOT EXISTS temperatura VARCHAR(10) DEFAULT 'frio';
ALTER TABLE lead ADD COLUMN IF NOT EXISTS id_estagio INTEGER REFERENCES lead_estagio(id);

-- =====================================================
-- DADOS INICIAIS: estágios padrão para a empresa existente
-- =====================================================
INSERT INTO lead_estagio (id_empresa, nome, ordem, cor)
VALUES
    ('13452799000107', 'Novo', 1, '#3b82f6'),
    ('13452799000107', 'Contato', 2, '#f59e0b'),
    ('13452799000107', 'Qualificado', 3, '#10b981'),
    ('13452799000107', 'Proposta', 4, '#8b5cf6'),
    ('13452799000107', 'Ganho', 5, '#22c55e'),
    ('13452799000107', 'Perdido', 6, '#ef4444');
