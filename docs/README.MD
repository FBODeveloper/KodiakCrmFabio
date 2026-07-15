# Kodiak ERP - CRM

> Contexto do projeto para assistentes de IA (ChatGPT, Codex, Claude, Gemini e similares).

---

# Objetivo

Este projeto é o módulo de **CRM (Customer Relationship Management)** do **Kodiak ERP**.

Seu objetivo é centralizar o relacionamento com clientes, fornecedores e prospects, permitindo registrar todas as interações comerciais, técnicas e administrativas realizadas pela empresa.

O CRM faz parte do ecossistema Kodiak ERP e compartilha diversas tabelas e conceitos existentes no ERP principal.

Este projeto deve priorizar:

- código simples
- alta legibilidade
- baixa complexidade
- facilidade de manutenção
- baixo acoplamento
- performance

Não criar arquiteturas complexas sem necessidade.

Sempre prefira soluções simples.

---

# Sobre o Kodiak ERP

Kodiak ERP é um ERP desenvolvido há vários anos.

Grande parte do banco de dados já existe.

O CRM utilizará essas informações ao invés de recriá-las.

Exemplos:

- parceiros
- municípios
- estados
- países
- usuários
- representantes
- vendedores
- produtos
- pedidos
- notas fiscais
- financeiro

Sempre que possível reutilizar informações existentes.

---

# Objetivos do CRM

O CRM será utilizado principalmente pelo setor comercial e suporte.

Entre as funcionalidades previstas:
- Multi-tenant
- Cadastro de parceiros CRM
- Gestão de Leads
- Funil de vendas
- Oportunidades
- Histórico de contatos
- Agendamentos
- Ligações
- WhatsApp
- Emails
- Tarefas
- Kanban
- Anotações
- Arquivos
- Propostas
- Visitas
- Indicadores
- Dashboards

Novas funcionalidades poderão ser adicionadas futuramente.

---

# Regras gerais

Sempre considere que:
- existe um banco PostgreSQL (após sua criação inicial)

- existe integração com o ERP (O KodiakERP irá utilizar a api criada nesse projeto para conversar com o CRM)
- evitar duplicação de informações

Sempre perguntar antes de criar novas tabelas quando houver dúvida se já existe estrutura equivalente.

---

# Filosofia de desenvolvimento

O projeto prioriza:

- SOLID (sem exageros)
- Clean Code
- KISS
- DRY
- YAGNI

Nunca aplicar padrões apenas porque são conhecidos.

Toda abstração deve possuir uma justificativa prática.

---

# Banco de dados

Banco principal:

PostgreSQL

Características:

- uso intenso de índices
- preferência por consultas simples
- evitar SELECT *
- sempre nomear colunas explicitamente
- utilizar parâmetros
- evitar SQL dinâmica quando possível
- todas as tabelas devem conter, obrigatoriamente, os campos id_empresa varchar(100), id_estabelecimento varchar(100), cnpj_empresa varchar(100), esses campos serão
importantes na separação multi-tenant
- cadastro de usuários além de campos tradicionais deverá conter também um campo chamado id_usuario_kodiak integer.

---

# Convenções SQL

Sempre:

- nomes em snake_case
- tabelas no singular
- PKs inteiras
- FKs seguindo o padrão id_tabela
- datas em timestamp/date conforme necessidade

Exemplo:

```sql
id_parceiro
id_empresa
id_estabelecimento
data_cadastro
```

---

# Convenções C#

Utilizar:

PascalCase

```csharp
ParceiroCRM
HistoricoContato
Lead
```

Campos privados:

```csharp
_databaseService
_repository
_logger
```

Interfaces:

```csharp
IParceiroRepository
ILeadService
```

DTO:

```text
ParceiroDTO
LeadDTO
HistoricoContatoDTO
```

Model:

```text
ParceiroModel
LeadModel
```

Repository:

```text
ParceiroRepository
LeadRepository
```

Service:

```text
LeadService
```

---

# Estilo de código

Priorizar métodos pequenos.

Evitar métodos acima de aproximadamente 50 linhas quando possível.

Evitar níveis profundos de identação.

Retornos antecipados são preferíveis.

Prefira nomes descritivos.

Evitar abreviações desnecessárias.

---

# Validação

A validação de regras de negócio deve ficar na camada responsável pelo domínio/aplicação.

Evitar colocar acesso ao banco dentro de DTOs.

Sempre que possível manter DTOs apenas como transporte de dados.

---

# Performance

Sempre considerar:

- índice disponível
- quantidade de registros
- custo dos JOINs
- paginação
- projeções pequenas

Não otimizar prematuramente.

Mas também não ignorar consultas potencialmente custosas.

---

# Segurança

Sempre utilizar:

- consultas parametrizadas
- validação de entrada
- tratamento de exceções
- logs de erro

Nunca concatenar parâmetros em SQL.

---

# Integrações futuras

O CRM poderá integrar com:

- WhatsApp
- Email
- Telefonia
- APIs REST
- Webhooks
- Agenda
- Google Calendar
- Outlook
- ERP Kodiak
- Aplicativos móveis

O código deve ser preparado para essas integrações sem criar abstrações desnecessárias.

---

# Como a IA deve responder

Ao gerar código:

1. Priorizar simplicidade.
2. Seguir o padrão já existente no projeto.
3. Não criar arquiteturas enterprise sem necessidade.
4. Não introduzir dependências externas sem justificativa.
5. Explicar quando existir mais de uma solução possível.
6. Informar impactos de performance quando relevantes.
7. Respeitar o padrão de nomenclatura existente.

---

# O que evitar

Evitar sugerir:

- Microservices
- Event Sourcing
- CQRS
- MediatR
- Arquiteturas extremamente complexas
- Patterns desnecessários
- Generic Repository universal
- Unit of Work quando não houver necessidade

A solução mais simples geralmente é a melhor.

---

# Prioridade

Em ordem de importância:

1. Correção
2. Legibilidade
3. Manutenção
4. Performance
5. Elegância

Código fácil de entender é preferível a código excessivamente sofisticado.

---

# Considerações finais

Sempre considerar que este projeto faz parte de um ERP já existente.

A prioridade é evoluir o sistema de forma incremental, mantendo compatibilidade com estruturas existentes e produzindo código claro, consistente e de fácil manutenção.