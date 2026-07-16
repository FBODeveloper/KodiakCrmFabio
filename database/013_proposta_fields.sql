-- =====================================================
-- KODIAK CRM - Script 013: Novos campos na tabela proposta
-- =====================================================

-- Adicionar novos campos à tabela proposta
ALTER TABLE proposta ADD COLUMN IF NOT EXISTS numero VARCHAR(20);
ALTER TABLE proposta ADD COLUMN IF NOT EXISTS data_proposta DATE;
ALTER TABLE proposta ADD COLUMN IF NOT EXISTS forma_pagamento VARCHAR(255);
ALTER TABLE proposta ADD COLUMN IF NOT EXISTS prazo_entrega VARCHAR(255);
ALTER TABLE proposta ADD COLUMN IF NOT EXISTS cliente_id INTEGER REFERENCES cliente(id);
ALTER TABLE proposta ADD COLUMN IF NOT EXISTS contato_id INTEGER REFERENCES contato(id);

-- Tornar parceiro nullable (já deve ser, mas garantir)
ALTER TABLE proposta ALTER COLUMN id_parceiro DROP NOT NULL;

-- Índices
CREATE INDEX IF NOT EXISTS idx_proposta_numero ON proposta(numero);
CREATE INDEX IF NOT EXISTS idx_proposta_cliente ON proposta(cliente_id);
CREATE INDEX IF NOT EXISTS idx_proposta_contato ON proposta(contato_id);
CREATE INDEX IF NOT EXISTS idx_proposta_data_proposta ON proposta(data_proposta);
