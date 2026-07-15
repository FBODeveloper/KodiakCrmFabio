# Contexto de CRM

## Objetivo

Este documento descreve o fluxo de negócio esperado de um CRM para
orientar uma IA na compreensão das entidades e suas relações.

## Fluxo principal

Lead -\> Qualificação -\> Oportunidade -\> Proposta -\> Negociação -\>
Ganha/Perdida -\> Cliente

### Lead

Pessoa ou empresa que demonstrou interesse. Não representa uma venda.

Campos típicos: - Origem - Nome - Empresa - Contato - Responsável -
Status

### Qualificação

Verifica necessidade, orçamento, autoridade e prazo. Resultados: -
Descartado - Permanecer Lead - Converter em Oportunidade

### Oportunidade

Representa um negócio real.

Possui: - Empresa - Contatos - Responsável - Pipeline - Valor previsto -
Probabilidade - Data prevista de fechamento

Uma empresa pode possuir diversas oportunidades.

### Pipeline

Etapas sugeridas: 1. Nova 2. Primeiro contato 3. Reunião agendada 4.
Diagnóstico 5. Proposta enviada 6. Negociação 7. Contrato 8. Ganha 9.
Perdida

### Atividades

Pertencem à oportunidade. Tipos: - Tarefa - Ligação - Reunião - Email -
Visita - Demonstração

Cada atividade possui: - Responsável - Data - Situação - Observações

### Agenda

Compromissos com data e hora. Pode estar vinculada à oportunidade.

### Histórico

Toda ação deve gerar histórico imutável.

Exemplos: - Lead criado - Ligação realizada - Reunião concluída -
Proposta enviada - Alteração de etapa - Venda concluída

### Empresa

Centraliza todas as informações comerciais.

Relacionamentos: Empresa ├── Contatos ├── Oportunidades ├── Histórico
└── Clientes

### Contatos

Uma empresa pode possuir vários contatos.

Exemplo: - Financeiro - Compras - TI - Diretor

### Cliente

Uma oportunidade ganha pode converter automaticamente a empresa em
cliente.

## Regras

-   Nunca criar oportunidade sem empresa.
-   Nunca apagar histórico.
-   Uma oportunidade pode possuir várias atividades.
-   Uma empresa pode possuir várias oportunidades.
-   Leads descartados permanecem para estatísticas.
-   Oportunidades perdidas registram motivo da perda.

## Métricas

-   Conversão Lead -\> Oportunidade
-   Conversão Oportunidade -\> Venda
-   Tempo médio por etapa
-   Ticket médio
-   Motivos de perda
-   Produtividade por vendedor

## Modelo conceitual

Empresa └── Contatos

Empresa └── Oportunidades ├── Pipeline ├── Atividades ├── Agenda ├──
Histórico ├── Propostas └── Produtos

Quando a oportunidade é Ganha: 1. Converter em Cliente. 2. Gerar
orçamento/pedido quando aplicável. 3. Iniciar implantação/onboarding.

Este documento deve ser considerado como a regra de negócio base para
desenvolvimento de um CRM integrado a um ERP.
