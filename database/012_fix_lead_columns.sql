-- =====================================================
-- KODIAK CRM - Script 012: Corrigir tabela lead
-- =====================================================

-- Adicionar colunas que faltam na tabela lead
ALTER TABLE lead ADD COLUMN IF NOT EXISTS ativo BOOLEAN DEFAULT true;
ALTER TABLE lead ADD COLUMN IF NOT EXISTS responsavel_nome VARCHAR(255);
ALTER TABLE lead ADD COLUMN IF NOT EXISTS responsavel_avatar TEXT;

-- Popular responsavel_nome baseado no responsavel_id
UPDATE lead l
SET responsavel_nome = u.nome,
    responsavel_avatar = u.avatar
FROM usuario u
WHERE l.responsavel_id = u.id
  AND (l.responsavel_nome IS NULL OR l.responsavel_nome = '');

-- Índices
CREATE INDEX IF NOT EXISTS idx_lead_ativo ON lead(ativo);

-- =====================================================
-- Lead estágios para empresas que não têm nenhum
-- =====================================================
-- Insere estágios padrão apenas para empresas que não possuem nenhum estágio
INSERT INTO lead_estagio (id_empresa, nome, ordem, cor)
SELECT DISTINCT l.id_empresa, est.nome, est.ordem, est.cor
FROM lead l
CROSS JOIN (VALUES
    ('Novo', 1, '#3b82f6'),
    ('Contato', 2, '#f59e0b'),
    ('Qualificado', 3, '#10b981'),
    ('Proposta', 4, '#8b5cf6'),
    ('Ganho', 5, '#22c55e'),
    ('Perdido', 6, '#ef4444')
) AS est(nome, ordem, cor)
WHERE NOT EXISTS (
    SELECT 1 FROM lead_estagio le WHERE le.id_empresa = l.id_empresa
)
AND l.id_empresa IS NOT NULL
AND l.id_empresa != ''
ON CONFLICT DO NOTHING;
