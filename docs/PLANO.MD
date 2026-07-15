# Plano de Desenvolvimento - CRM KodiakERP

Visão geral das etapas de desenvolvimento, do básico ao avançado.

---

## Fase 0 - Infraestrutura Base
1. Configurar soluções .NET (Clean Architecture)
2. Criar projeto Database (scripts SQL iniciais)
3. Configurar connection strings e logging básico

## Fase 1 - Autenticação e Usuários
4. Tabela de usuários CRM (com id_usuario_kodiak)
5. API de autenticação (login + JWT)
6. Middleware de autenticação

## Fase 2 - Cadastro de Parceiros
7. Tabelas de parceiros eendereços
8. API CRUD parceiros
9. Validações e regras de negócio

## Fase 3 - Gestão de Leads
10. Tabela de leads
11. API CRUD leads
12. Conversão lead → parceiro

## Fase 4 - Funil de Vendas e Oportunidades
13. Tabelas de funil e oportunidades
14. API CRUD oportunidades
15. Kanban board (frontend)

## Fase 5 - Atividades
16. Tabelas de atividades (contatos, tarefas, agendamentos)
17. API CRUD atividades
18. Histórico de contatos

## Fase 6 - Propostas
19. Tabela de propostas
20. API CRUD propostas
21. Geração de proposta (PDF/markdown)

## Fase 7 - Dashboard e Indicadores
22. Queries de indicadores
23. API de métricas
24. Dashboard visual

## Fase 8 - Frontend Básico
25. Setup React + rotas
26. Layout e autenticação
27. Telas de parceiros

## Fase 9 - Frontend Avançado
28. Telas de leads e funil
29. Kanban interativo
30. Dashboard

## Fase 10 - Integrações
31. Webhooks básicos
32. Documentação API (Swagger)

---

Cada fase gera incrementos testáveis e funcionais.
