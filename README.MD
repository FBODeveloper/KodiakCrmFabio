# Kodiak CRM

Sistema CRM integrado ao Kodiak ERP.

## Visão Geral

CRM para gestão de relacionamento com clientes, leads e oportunidades de vendas.

## Stack

| Camada | Tecnologia |
|--------|------------|
| Backend | .NET 10, Dapper, Npgsql |
| Frontend | React 19, TypeScript, Vite |
| Banco | PostgreSQL 18 |
| Auth | JWT + API Key |

## Estrutura do Projeto

```
kodiak-crm/
├── docs/                    # Documentação
│   ├── README.MD           # Visão geral
│   ├── AGENTS.MD           # Contexto para IA
│   ├── BACKEND.MD          # Configurações backend
│   ├── FRONTEND.MD         # Configurações frontend
│   ├── DATABASE.MD         # Configurações banco
│   └── PLANO.MD            # Plano de desenvolvimento
├── backend/                 # API .NET
│   ├── KodiakCrm.sln
│   ├── KodiakCrm.Core/
│   ├── KodiakCrm.UseCases/
│   ├── KodiakCrm.Infrastructure/
│   └── KodiakCrm.Api/
└── frontend/               # React
    └── kodiak-crm-web/
```

## Funcionalidades

- Parceiros CRM
- Gestão de Leads
- Funil de Vendas (Kanban)
- Oportunidades
- Atividades e Contatos
- Propostas Comerciais
- Dashboards

## Como Rodar

### Backend
```bash
cd backend
dotnet restore
dotnet run --project KodiakCrm.Api
```

### Frontend
```bash
cd frontend/kodiak-crm-web
npm install
npm run dev
```

## Documentação

Consulte a pasta `docs/` para mais detalhes.
