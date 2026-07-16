# Contexto de CRM - KodiakCrm

## Objetivo
CRM multi-tenant comercializável para empresas clientes gerenciarem seus próprios usuários, leads, oportunidades, atividades, propostas, clientes e contatos.

## Fluxo principal
Lead → Qualificação → Oportunidade → Proposta → Negociação → Ganha/Perdida → Cliente

## Stack
- **Backend:** .NET 10, Dapper + PostgreSQL 18, Clean Architecture
- **Frontend:** React 19 + TypeScript + Vite
- **Portas:** API 5279, Frontend 5173
- **Git:** https://github.com/FBODeveloper/KodiakCrmFabio.git

## Entidades Principais

### Lead
- Kanban drag-and-drop por estágios do funil
- Temperatura: quente/morno/frio
- Email não é único (cada interação = lead separado)
- Responsável vinculado

### Oportunidade
- Criada via conversão de lead (herda responsável/parceiro)
- Título automático: "Oportunidade de [nome do lead]"
- Status calculado: aberta/ganha/perdida
- Motivo de perda obrigatório ao perder

### Cliente
- Criado a partir de oportunidade ganha OU cadastro direto
- Origem: conversao, cadastro_direto, indicacao, outro
- Contatos vinculados

### Contato
- Sub-entidade vinculada a cliente OU parceiro

### Atividade
- Tipos: tarefa, ligacao, reunião, email, visita, demonstracao
- Vinculada a oportunidade, parceiro ou lead
- Notificação automática quando atrasada

### Proposta
- Status: rascunho, enviada, aprovada, rejeitada
- Nº sequencial automático por ano (ex: 1001/2026)
- Campos: data_proposta, forma_pagamento (texto), prazo_entrega (texto)
- Vinculada a cliente + contato (parceiro é opcional/legado)
- Itens dinâmicos com quantidade e valor unitário
- Observação abaixo dos itens
- Exclusão lógica (soft delete)

### Histórico
- Append-only com dados_antes/dados_depois em JSON
- Toda ação gera registro imutável

### Automações
- Lead criado → follow-up automático em 3 dias + notificação
- Atividade atrasada → notificação automática

## Funcionalidades (16 Fases Concluídas)

| Fase | Descrição |
|------|-----------|
| 0-6 | Infraestrutura, Auth, Parceiros, Leads, Funil, Oportunidades, Atividades, Propostas |
| 7 | Métricas avançadas (ticket médio, conversão, produtividade) |
| 8 | Configurações (empresa, usuários, logs) |
| 9 | Relatórios (vendas, atividades, performance) |
| 10 | Notificações (sistema + badge) |
| 11 | UX (Toast, Pagination, LoadingButton) |
| 12 | Busca global (SearchBar) |
| 13 | Automações (follow-up, atividades atrasadas) |
| 14 | Perfil do usuário |
| 15 | Filtros avançados (FilterBar reutilizável em 5 páginas) |
| 16 | Correções Teste Frontend 1 (DateOnly fix, ícones CRUD, propostas campos novos, histórico, filtros) |

### Migrações
- 001-012: criadas anteriormente
- **013**: novos campos na tabela proposta (numero, data_proposta, forma_pagamento, prazo_entrega, cliente_id, contato_id)

## Próximos Passos
- Segundo teste frontend para validar correções
- Exportação CSV dos relatórios
- Dashboard personalizável
- Audit log admin
- Backup/restore
- API pública
