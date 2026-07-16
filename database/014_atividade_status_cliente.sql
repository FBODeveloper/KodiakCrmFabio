-- =====================================================
-- KODIAK CRM - Script 014: Novos campos na tabela atividade
-- =====================================================

-- Adicionar status, cliente e responsavel
ALTER TABLE atividade ADD COLUMN IF NOT EXISTS status VARCHAR(20) DEFAULT 'pendente';
ALTER TABLE atividade ADD COLUMN IF NOT EXISTS cliente_id INTEGER REFERENCES cliente(id);
ALTER TABLE atividade ADD COLUMN IF NOT EXISTS responsavel_id_col INTEGER REFERENCES usuario(id);

-- Sincronizar dados existentes: concluida=true -> status='concluido'
UPDATE atividade SET status = 'concluido' WHERE concluida = true AND status = 'pendente';

-- Garantir que responsavel_id existente seja copiado para a nova coluna se necessario
-- (responsavel_id já existe na tabela, então não precisamos copiar)

-- Índices
CREATE INDEX IF NOT EXISTS idx_atividade_status ON atividade(status);
CREATE INDEX IF NOT EXISTS idx_atividade_cliente ON atividade(cliente_id);
