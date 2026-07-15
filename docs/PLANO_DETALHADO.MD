# Plano Detalhado de Desenvolvimento - CRM KodiakERP

## Fase 0 - Infraestrutura Base

### Etapa 1: Estrutura de Pastas e Solução .NET
- Criar solução `KodiakCrm.sln`
- Criar projetos:
  - `KodiakCrm.Core` (entidades, interfaces)
  - `KodiakCrm.UseCases` (serviços, regras negócio)
  - `KodiakCrm.Infrastructure` (repositórios, Dapper)
  - `KodiakCrm.Api` (controllers, middleware)
- Configurar dependências entre projetos

### Etapa 2: Configuração do Banco
- Script SQL inicial (migrations)
- Configuração connection string
- Serviço de conexão com Dapper

### Etapa 3: Middleware Básico
- Tratamento de erros global
- Logging configurado
- CORS habilitado

---

## Fase 1 - Autenticação

### Etapa 4: Tabela de Usuários
```sql
CREATE TABLE usuario (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(100) NOT NULL,
    id_estabelecimento VARCHAR(100) NOT NULL,
    cnpj_empresa VARCHAR(100) NOT NULL,
    id_usuario_kodiak INTEGER,
    nome VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    senha_hash VARCHAR(255) NOT NULL,
    ativo BOOLEAN DEFAULT true,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

### Etapa 5: API Autenticação
- POST `/api/auth/login` → retorna JWT
- POST `/api/auth/validate` → valida token
- Middleware JWT configurado

### Etapa 6: Testes de Autenticação
- Login com credenciais válidas
- Login com credenciais inválidas
- Acesso sem token (deve retornar 401)

---

## Fase 2 - Parceiros

### Etapa 7: Tabelas
```sql
CREATE TABLE parceiro (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(100) NOT NULL,
    id_estabelecimento VARCHAR(100) NOT NULL,
    cnpj_empresa VARCHAR(100) NOT NULL,
    razao_social VARCHAR(255) NOT NULL,
    nome_fantasia VARCHAR(255),
    cpf_cnpj VARCHAR(14),
    tipo_pessoa CHAR(1), -- J=Jurídica, F=Física
    email VARCHAR(255),
    telefone VARCHAR(20),
    celular VARCHAR(20),
    observacao TEXT,
    ativo BOOLEAN DEFAULT true,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE parceiro_endereco (
    id SERIAL PRIMARY KEY,
    id_parceiro INTEGER REFERENCES parceiro(id),
    tipo_endereco VARCHAR(20), -- principal, cobranca, entrega
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
```

### Etapa 8: API Parceiros
- GET `/api/parceiros` → lista com paginação
- GET `/api/parceiros/{id}` → busca por ID
- POST `/api/parceiros` → cria novo
- PUT `/api/parceiros/{id}` → atualiza
- DELETE `/api/parceiros/{id}` → soft delete (ativo=false)

### Etapa 9: Validações
- CNPJ/CPF único por empresa
- Razão social obrigatória
- Email formato válido

---

## Fase 3 - Leads

### Etapa 10: Tabela Lead
```sql
CREATE TABLE lead (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(100) NOT NULL,
    id_estabelecimento VARCHAR(100) NOT NULL,
    cnpj_empresa VARCHAR(100) NOT NULL,
    nome VARCHAR(255) NOT NULL,
    empresa VARCHAR(255),
    email VARCHAR(255),
    telefone VARCHAR(20),
    source VARCHAR(100), --origem do lead
    status VARCHAR(50) DEFAULT 'novo',
    id_parceiro INTEGER REFERENCES parceiro(id), --lead convertido
    observacao TEXT,
    responsavel_id INTEGER REFERENCES usuario(id),
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

### Etapa 11: API Leads
- GET `/api/leads` → lista com filtros
- GET `/api/leads/{id}` → detalhes
- POST `/api/leads` → cria lead
- PUT `/api/leads/{id}` → atualiza
- PUT `/api/leads/{id}/converter` → converte em parceiro

### Etapa 12: Conversão Lead → Parceiro
- Cria parceiro automaticamente
- Vincula ID do parceiro ao lead
- Atualiza status para 'convertido'

---

## Fase 4 - Funil de Vendas

### Etapa 13: Tabelas
```sql
CREATE TABLE funil (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(100) NOT NULL,
    nome VARCHAR(100) NOT NULL,
    ativo BOOLEAN DEFAULT true
);

CREATE TABLE funil_estagio (
    id SERIAL PRIMARY KEY,
    id_funil INTEGER REFERENCES funil(id),
    nome VARCHAR(100) NOT NULL,
    ordem INTEGER NOT NULL,
    probabilidade INTEGER DEFAULT 0 -- 0-100
);

CREATE TABLE oportunidade (
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
```

### Etapa 14: API Oportunidades
- CRUD completo
- Moveção entre estágios
- Filtros por responsável, estágio, período

### Etapa 15: Kanban (Frontend)
- Board visual com drag & drop
- Colunas = estágios do funil
- Cards = oportunidades

---

## Fase 5 - Atividades

### Etapa 16: Tabelas
```sql
CREATE TABLE atividade (
    id SERIAL PRIMARY KEY,
    id_empresa VARCHAR(100) NOT NULL,
    id_estabelecimento VARCHAR(100) NOT NULL,
    cnpj_empresa VARCHAR(100) NOT NULL,
    tipo VARCHAR(50) NOT NULL, -- ligacao, reuniao, visita, tarefa, email, whatsapp
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
```

### Etapa 17: API Atividades
- CRUD completo
- Filtros por tipo, parceiro, oportunidade, período
- Marcar como concluída

### Etapa 18: Histórico de Contatos
- Timeline de atividades por parceiro
- Filtros por tipo e período

---

## Fase 6 - Propostas

### Etapa 19: Tabelas
```sql
CREATE TABLE proposta (
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

CREATE TABLE proposta_item (
    id SERIAL PRIMARY KEY,
    id_proposta INTEGER REFERENCES proposta(id),
    descricao VARCHAR(255) NOT NULL,
    quantidade INTEGER NOT NULL,
    valor_unitario DECIMAL(15,2) NOT NULL,
    valor_total DECIMAL(15,2) NOT NULL
);
```

### Etapa 20: API Propostas
- CRUD completo
- Cálculo automático de totais
- Status: rascunho, enviada, aprovada, rejeitada

### Etapa 21: Geração
- Exportar proposta (PDF ou Markdown)

---

## Fase 7 - Dashboard

### Etapa 22: Queries Indicadores
- Leads por status
- Oportunidades por estágio
- Valor do funil
- Taxa de conversão
- Atividades pendentes
- Top vendedores

### Etapa 23: API Métricas
- GET `/api/dashboard/resumo`
- GET `/api/dashboard/funil`
- GET `/api/dashboard/leads`
- GET `/api/dashboard/atividades`

### Etapa 24: Dashboard Frontend
- Cards de resumo
- Gráficos simples (barras, pizza)
- Filtros por período

---

## Fase 8 - Frontend Básico

### Etapa 25: Setup React
- Criar projeto Vite + React + TS
- Configurar rotas
- Configurar Axios + interceptores

### Etapa 26: Layout
- Componente de Layout (sidebar, header)
- Página de Login
- Rota protegida

### Etapa 27: Telas Parceiros
- Lista com paginação
- Formulário de criação/edição
- Detalhes do parceiro

---

## Fase 9 - Frontend Avançado

### Etapa 28: Telas Leads
- Lista de leads
- Formulário
- Conversão lead → parceiro

### Etapa 29: Funil/Kanban
- Board Kanban
- Drag & drop entre estágios
- Cards de oportunidades

### Etapa 30: Dashboard
- Tela de indicadores
- Gráficos

---

## Fase 10 - Finalização

### Etapa 31: Swagger/OpenAPI
- Documentar todos os endpoints
- Adicionar autenticação no Swagger

### Etapa 32: Testes e Ajustes
- Testes manuais
- Ajustes finais
- Validação multi-tenant

---

## Ordem de Execução Recomendada

1. Fase 0 → Fundação
2. Fase 1 → Auth (pode testar login)
3. Fase 2 → Parceiros (cadastro básico)
4. Fase 3 → Leads (captura de interesse)
5. Fase 4 → Funil (pipeline de vendas)
6. Fase 5 → Atividades (histórico)
7. Fase 6 → Propostas (comercial)
8. Fase 7 → Dashboard (visão geral)
9. Fase 8 → Frontend básico
10. Fase 9 → Frontend completo
11. Fase 10 → Publicação

Cada fase gera um incremento funcional e testável.
