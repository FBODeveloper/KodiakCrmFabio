-- ============================================================
-- Migration 015: Criar Funil padrão com estágios para cada empresa
-- ============================================================

-- Para cada empresa que não tenha nenhum funil, cria o funil padrão
DO $$
DECLARE
    empresa RECORD;
    funil_id INTEGER;
BEGIN
    FOR empresa IN SELECT cnpj FROM empresa LOOP
        -- Verifica se a empresa já tem funis
        IF NOT EXISTS (SELECT 1 FROM funil WHERE id_empresa = empresa.cnpj) THEN
            -- Cria o funil padrão
            INSERT INTO funil (id_empresa, nome)
            VALUES (empresa.cnpj, 'Funil de Vendas')
            RETURNING id INTO funil_id;

            -- Cria os estágios padrão
            INSERT INTO funil_estagio (id_funil, nome, ordem, probabilidade) VALUES
                (funil_id, 'Prospecção', 1, 10),
                (funil_id, 'Qualificação', 2, 25),
                (funil_id, 'Proposta', 3, 50),
                (funil_id, 'Negociação', 4, 75),
                (funil_id, 'Fechamento', 5, 90);
        END IF;
    END LOOP;
END $$;
