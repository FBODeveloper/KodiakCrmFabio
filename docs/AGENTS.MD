# Agents - Contexto do Projeto KodiakCRM

## Visão Geral
CRM para o KodiakERP, desenvolvido com .NET 10 (Backend) e React 19 (Frontend).

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
- Inclui mapeamento explícito da entity `Empresa`

## Estrutura do Projeto

```
kodiak-crm/
├── backend/
│   ├── KodiakCrm.Api/           # Controllers, Program.cs, Swagger
│   ├── KodiakCrm.Core/          # Entities, DTOs, Interfaces
│   ├── KodiakCrm.Infrastructure/ # Repositories, Data (DapperConfig, DatabaseConnection)
│   └── KodiakCrm.UseCases/      # Services (AuthService, etc.)
├── frontend/
│   └── kodiak-crm-web/
│       └── src/
│           ├── api/axios.ts         # Instância Axios
│           ├── contexts/AuthContext.tsx  # Auth, isAdmin, isGerente
│           ├── pages/               # Todas as páginas
│           ├── types/index.ts       # Interfaces TypeScript
│           └── index.css            # CSS com dark mode
├── database/                    # Scripts de migration SQL
└── docs/                        # Documentação
```

## Entidades e Tabelas

### empresa (PK: cnpj)
- `cnpj` VARCHAR(14) PK
- `razao_social` VARCHAR
- `nome_fantasia` VARCHAR
- `quantidade_usuarios_contratados` INTEGER (default 1)
- `ativo` BOOLEAN (default true)
- `data_cadastro` TIMESTAMP

### usuario
- `id` SERIAL PK
- `id_empresa` VARCHAR(100) FK → empresa.cnpj
- `id_estabelecimento`, `cnpj_empresa` VARCHAR
- `nome`, `email`, `senha_hash` VARCHAR
- `perfil` VARCHAR ('admin', 'gerente', 'usuario')
- `avatar` VARCHAR (URL)
- `data_nascimento` DATE
- `ativo` BOOLEAN, `data_cadastro` TIMESTAMP

### parceiro
- `id` SERIAL PK
- `id_empresa`, `id_estabelecimento`, `cnpj_empresa` VARCHAR
- `razao_social`, `nome_fantasia` VARCHAR
- `cpf_cnpj` VARCHAR
- `tipo_pessoa` VARCHAR (corrigido de char para string)
- `email`, `telefone`, `celular` VARCHAR
- `id_parceiro_kodiakerp` INTEGER (nullable, vínculo com ERP)
- `observacao` TEXT
- `ativo` BOOLEAN, `data_cadastro` TIMESTAMP

### lead
- `id` SERIAL PK
- `id_empresa`, `id_estabelecimento`, `cnpj_empresa` VARCHAR
- `nome`, `empresa`, `email`, `telefone`, `source` VARCHAR
- `status` VARCHAR ('novo', 'convertido')
- `id_parceiro` INTEGER (nullable)
- `responsavel_id` INTEGER (nullable)
- `observacao` TEXT
- `ativo` BOOLEAN, `data_cadastro` TIMESTAMP

### funil, funil_estagio, oportunidade, atividade, proposta, proposta_item
- Seguem padrão multi-tenant com `id_empresa`

## Funcionalidades Implementadas

### Backend (API)
1. **Auth** - Login JWT com hash SHA256, claims (id_empresa, id_estabelecimento, cnpj_empresa, role)
2. **Parceiros** - CRUD + sync com ERP (`POST /api/parceiros/sincronizar-erp` AllowAnonymous)
3. **Leads** - CRUD + conversão para parceiro
4. **Funil** - CRUD
5. **Oportunidades** - CRUD + movimentação Kanban
6. **Atividades** - CRUD + filtro por parceiro/oportunidade
7. **Propostas** - CRUD + alteração de status + itens
8. **Dashboard** - Resumo de métricas
9. **Empresas** - CRUD (admin)
10. **Usuários Gestão** - CRUD com controle de limite (empresa.QuantidadeUsuariosContratados)

### Frontend (React)
1. **Login** - Formulário com CNPJ + email + senha
2. **Layout** - Sidebar com navegação + dark mode toggle + avatar do usuário
3. **Dashboard** - Cards de métricas
4. **Parceiros** - List, Form, Detalhes
5. **Leads** - List, Form
6. **Empresas** - List, Form (admin)
7. **Usuários** - List, Form com seletor de empresa (admin)
8. **Oportunidades** - List, Form
9. **Atividades** - List, Form
10. **Propostas** - List, Form com itens dinâmicos

### Dark Mode
- CSS variables `:root` (light) e `[data-theme="dark"]` (dark)
- Variáveis: `--sidebar-bg`, `--input-bg`, `--card-shadow`
- Toggle no sidebar footer (☀️/🌙)
- Persistência no `localStorage`
- Aplicação no carregamento via `App.tsx`

## Regras de Negócio

### Multi-tenant
- Todas as queries filtram por `id_empresa`
- `id_empresa` = CNPJ da empresa (obtido do JWT)

### Empresa
- CNPJ é a PK da empresa (formato: 14 dígitos)
- `quantidade_usuarios_contratados` controla limite de usuários

### Usuários
- Admin pode criar usuários em qualquer empresa (seletor no formulário)
- Não-admin cria apenas na própria empresa (via JWT)
- Controle de limite: `ContarPorEmpresaAsync(idEmpresa) < empresa.QuantidadeUsuariosContratados`
- Perfis: `admin`, `gerente`, `usuario` (claim `role` no JWT)

### Parceiros
- `tipo_pessoa` é string ('F' ou 'J'), não char
- Endpoint `sincronizar-erp` (AllowAnonymous) para sincronizar do KodiakERP

## Bugs Corrigidos
1. **Login não funcionava** - Dapper não mapeava `senha_hash` → `SenhaHash` (snake_case). Solução: `DapperConfig.cs` com `ToPascalCase`
2. **TipoPessoa** - Era `char?`, corrigido para `string?` em entidade e todos os DTOs (causava erro no cadastro)
3. **Empresa Erro ao salvar** - `EmpresaRepository.CriarAsync` usava `ExecuteScalarAsync<int>` com `RETURNING cnpj` (string). Solução: trocado para `ExecuteAsync`
4. **CNPJ muito longo** - Frontend enviava com formatação (18 chars), banco aceita 14. Solução: `handleChange` filtra só dígitos
5. **Dark mode sidebar** - Sidebar usava `var(--text)` que fica cinza claro no dark mode. Solução: variável `--sidebar-bg` dedicada
6. **Catch genérico** - Adicionado `try/catch (Exception)` em todos os controllers (Lead, Funil, Atividade, Proposta, Oportunidade, Empresa)

## Arquivos Importantes

### Backend
- `Program.cs` - DI completa
- `DapperConfig.cs` - Mapeamento snake_case → PascalCase
- `EmpresaRepository.cs` - CRUD empresa
- `UsuarioRepository.cs` - CRUD usuário com queries de contagem
- `UsuarioGestaoService.cs` - Controle de limite de usuários
- `UsuarioGestaoController.cs` - Aceita `idEmpresa` do DTO para admin
- `AuthService.cs` - Login JWT com retorno de empresa

### Frontend
- `App.tsx` - Todas as rotas registradas
- `AuthContext.tsx` - Auth, isAdmin, isGerente
- `Layout.tsx` - Sidebar com dark mode toggle
- `UsuarioForm.tsx` - Seletor de empresa para admin
- `index.css` - CSS variables + dark mode
- `types/index.ts` - Todas as interfaces

## Pendente / Próximos Passos
- Testar cadastro de parceiro, oportunidades, atividades e propostas
- Itens de proposta (CSS já adicionado)
- Implementar更多 funcionalidades CRM (agendamentos, tarefas, anotações)
- WhatsApp/Email integration
- Google Calendar/Outlook integration
