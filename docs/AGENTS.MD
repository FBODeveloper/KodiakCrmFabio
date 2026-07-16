# Agents - Contexto do Projeto KodiakCRM

## Visão Geral
CRM para o KodiakERP, desenvolvido com .NET 10 (Backend) e React 19 (Frontend).
**Produto comercializável**: CRM multi-tenant onde empresas clientes gerenciam seus próprios usuários.

## Stack Tecnológica

### Backend
- **Linguagem:** C# (.NET 10)
- **ORM:** Dapper + Npgsql (NÃO usar Entity Framework)
- **Arquitetura:** Clean Architecture (Core, UseCases, Infra, API)
- **Autenticação:** JWT com SHA256 hash (Base64) + API Key
- **Documentação:** Swagger (Swashbuckle.AspNetCore 6.9.0)
- **Porta:** 5279

### Frontend
- **Framework:** React 19 + TypeScript
- **Build:** Vite (porta 5173)
- **Rotas:** react-router-dom
- **HTTP:** Axios (baseURL: `http://localhost:5279/api`)
- **Estilos:** CSS puro com design system via CSS variables (dark mode)

### Banco de Dados
- **SGBD:** PostgreSQL 18
- **Conexão:** localhost:5432
- **Usuário:** postgres
- **Senha:** 123!asd
- **Database:** kodiak_crm
- **Schema:** snake_case, tabelas no singular
- **Migrações:** 11 arquivos (001-011)

## Convenções de Código

### SQL/PostgreSQL
- snake_case para nomes de tabelas e colunas
- PKs: `id` SERIAL INTEGER
- FKs: `id_tabela`
- Campos obrigatórios em todas as tabelas:
  - `id_empresa VARCHAR(100)` (CNPJ como PK da empresa)
  - `id_estabelecimento VARCHAR(100)`
  - `cnpj_empresa VARCHAR(100)`

### C# (PascalCase)
- Classes: ParceiroCRM, HistoricoContato
- Privados: _databaseService, _repository
- Interfaces: IParceiroRepository, ILeadService
- DTOs: ParceiroDTO, LeadDTO
- Repository: ParceiroRepository, LeadRepository
- Service: LeadService

### TypeScript (camelCase)
- Interfaces: Parceiro, Lead, Empresa
- Props interfaces nos componentes

### Mapeamento Dapper
- `DapperConfig.cs` com `CustomPropertyTypeMap` + `ToPascalCase` para mapear automaticamente snake_case (banco) → PascalCase (C#)
- **Dapper bug crítico**: DTOs sem type map — aliases SQL devem ser PascalCase. Entidades usam type map automático com snake_case
- Inclui type maps para: Lead, Oportunidade, Atividade, Proposta, PropostaItem, FunilEstagio, Empresa, Usuario, Parceiro, Historico, Cliente, Contato, EmpresaConfig, RelatorioGerado, Notificacao
- `DateOnlyTypeHandler` para DateOnly no Dapper

## Estrutura do Projeto

```
kodiak-crm/
├── backend/
│   ├── KodiakCrm.Api/           # Controllers, Program.cs, Swagger
│   ├── KodiakCrm.Core/          # Entities, DTOs, Interfaces
│   ├── KodiakCrm.Infrastructure/ # Repositories, Data (DapperConfig, DatabaseConnection)
│   └── KodiakCrm.UseCases/      # Services
├── frontend/
│   └── kodiak-crm-web/
│       └── src/
│           ├── api/axios.ts
│           ├── contexts/AuthContext.tsx
│           ├── components/        # Componentes reutilizáveis
│           │   ├── FilterBar.tsx   # Filtros avançados
│           │   ├── Toast.tsx       # Notificações globais
│           │   ├── Pagination.tsx  # Paginação
│           │   ├── LoadingButton.tsx # Botão com spinner
│           │   └── SearchBar.tsx   # Busca global
│           ├── pages/             # Todas as páginas
│           ├── types/index.ts     # Interfaces TypeScript
│           └── index.css          # CSS com dark mode
├── database/                    # 11 migrações SQL (001-011)
└── docs/                        # Documentação
```

## Entidades e Tabelas

### empresa (PK: cnpj)
- `cnpj` VARCHAR(14) PK
- `razao_social`, `nome_fantasia` VARCHAR
- `quantidade_usuarios_contratados` INTEGER (default 1)
- `telefone`, `email`, `endereco` VARCHAR
- `ativo` BOOLEAN (default true)
- `data_cadastro` TIMESTAMP

### usuario
- `id` SERIAL PK, `id_empresa` FK → empresa.cnpj
- `nome`, `email`, `senha_hash` VARCHAR
- `perfil` VARCHAR ('admin', 'gerente', 'usuario')
- `avatar` VARCHAR (URL), `data_nascimento` DATE
- `ativo` BOOLEAN, `data_cadastro` TIMESTAMP

### parceiro
- `id` SERIAL PK, multi-tenant
- `razao_social`, `nome_fantasia`, `cpf_cnpj` VARCHAR
- `tipo_pessoa` VARCHAR ('F' ou 'J')
- `email`, `telefone`, `celular` VARCHAR
- `id_parceiro_kodiakerp` INTEGER (nullable, vínculo com ERP)
- `observacao` TEXT, `ativo`, `data_cadastro`

### lead
- `id` SERIAL PK, multi-tenant
- `nome`, `empresa`, `email`, `telefone`, `source` VARCHAR
- `status` VARCHAR ('novo', 'convertido')
- `temperatura` VARCHAR ('quente', 'morno', 'frio')
- `estagio_id` INTEGER FK → funil_estagio
- `id_parceiro` INTEGER (nullable)
- `responsavel_id` INTEGER FK → usuario
- `observacao` TEXT, `ativo`, `data_cadastro`

### funil / funil_estagio / oportunidade / atividade / proposta / proposta_item
- Seguem padrão multi-tenant com `id_empresa`
- Oportunidade: `motivo_perda`, `data_previsao`, `data_cadastro`
- Atividade: `tipo`, `concluida`, `data_atividade`, `responsavel_id`
- Proposta: `status` (rascunho/enviada/aprovada/rejeitada), `valor_total`
- PropostaItem: `descricao`, `quantidade`, `valor_unitario`, `valor_total`

### historico
- `id` SERIAL PK, multi-tenant
- `tabela` VARCHAR, `registro_id` INTEGER, `acao` VARCHAR (CREATE/UPDATE/DELETE)
- `dados_antes` JSONB, `dados_depois` JSONB
- `usuario_id` INTEGER, `data_hora` TIMESTAMP
- **Append-only** — registros nunca são alterados/excluídos

### empresa_config
- `id` SERIAL PK, `cnpj_empresa` FK
- `notificacao_email` BOOLEAN, `notificacao_sistema` BOOLEAN
- `dias_followup_automatico` INTEGER (default 3)
- `horario_inicio`, `horario_fim` TIME

### relatorio_gerado
- `id` SERIAL PK, multi-tenant
- `tipo` VARCHAR, `periodo_inicio`, `periodo_fim` DATE
- `filtros_aplicados` JSONB, `resultado` JSONB
- `gerado_por` INTEGER FK → usuario

### notificacao
- `id` SERIAL PK, multi-tenant
- `usuario_id` INTEGER FK → usuario
- `titulo`, `mensagem` VARCHAR
- `tipo` VARCHAR (followup_atrasado, atividade_atrasada, etc.)
- `lida` BOOLEAN (default false), `data_criacao` TIMESTAMP

## Regras de Negócio

### Multi-tenant
- Todas as queries filtram por `id_empresa` (CNPJ do JWT)

### Usuários (multi-tenant SaaS)
- **Admin**: acessa qualquer empresa/qualquer perfil (seletor de empresa no form)
- **Gerente**: só própria empresa, só gerente/usuário
- **Usuário**: não cria usuários
- Controle de limite: `ContarPorEmpresaAsync(idEmpresa) < empresa.QuantidadeUsuariosContratados`
- Senhas: SHA256 hash (Base64) antes de comparar

### Lead
- Kanban drag-and-drop por estágios do funil
- Temperatura: quente/morno/frio
- Email **não é único** — cada interação é um lead separado
- Responsável vinculado ao lead

### Conversão Lead → Oportunidade
- `LeadService.ConverterAsync` cria Oportunidade, herda responsável/parceiro
- Título automático: "Oportunidade de [nome do lead]"
- Lead status → 'convertido', cria Cliente automaticamente se não existir

### Cliente
- Criado a partir de oportunidade ganha OU cadastro direto
- `origem` (conversao/cadastro_direto/indicacao/outro), `data_conversao`, `id_oportunidade`

### Contato
- Sub-entidade vinculada a cliente OU parceiro

### Automações
- Lead criado → follow-up automático em 3 dias + notificação
- Atividade atrasada → notificação automática

### Histórico
- Toda ação gera registro imutável com dados_antes/dados_depois em JSON

### Oportunidades - Filtros
- `status` na API mapeia para: "ganha" (motivo_perda IS NULL + EXISTS cliente), "perdida" (motivo_perda IS NOT NULL), "aberta" (motivo_perda IS NULL + NOT EXISTS cliente)

## Funcionalidades Implementadas (15 Fases)

### Fase 0-6: Infraestrutura e Funcionalidades Base
- Auth JWT, Parceiros, Leads (kanban+temperatura), Funil, Oportunidades, Atividades, Propostas

### Fase 7: Métricas Avançadas
- Dashboard: Ticket médio, conversão, produtividade por vendedor
- 8 cards métricos + tabela produtividade vendedores

### Fase 8: Configurações
- Migração 009: colunas empresa + empresa_config
- ConfigController, Configuracoes.tsx (3 abas: Empresa/Usuários/Logs)

### Fase 9: Relatórios
- Migração 010: relatorio_gerado JSONB
- RelatorioService (vendas/atividades/performance)
- RelatorioController (4 endpoints), Relatorios.tsx (3 abas)

### Fase 10: Notificações
- Migração 011: tabela notificacao
- NotificacaoService, NotificacaoController (CRUD + resumo)
- Notificacoes.tsx, Layout badge vermelho (polling 30s)

### Fase 11: UX Global
- Toast.tsx (notificação global), Pagination.tsx, LoadingButton.tsx

### Fase 12: Pesquisa Global
- SearchBar.tsx — busca no header (leads/clientes/parceiros/contatos), debounce 300ms

### Fase 13: Automações
- AutomacaoService: follow-up automático ao criar lead, notificação atividade atrasada

### Fase 14: Perfil
- PerfilController (GET/PUT perfil, PUT senha com validação SHA256)
- Perfil.tsx (avatar, dados, alterar senha), clique avatar → /perfil

### Fase 15: Filtros Avançados
- FilterBar.tsx componente reutilizável (busca textual/select/data)
- Aplicado em Leads (status/temperatura/datas), Oportunidades (status/datas), Atividades (tipo/concluida/datas), Clientes (origem/datas), Propostas (status/datas)
- Backend: todos 5 controllers/repositories/services atualizados com WHERE dinâmico

## Git Remote
- `https://github.com/FBODeveloper/KodiakCrmFabio.git` (main branch)
- Último commit: `d7457e3`

## Pendente / Próximos Passos
- Exportação CSV dos relatórios
- Dashboard personalizável
- Audit log admin
- Backup/restore
- API pública
