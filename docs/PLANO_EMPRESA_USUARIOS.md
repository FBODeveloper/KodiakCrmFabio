# Plano: Empresa, Usuários e Controle de Acesso

## Objetivo
1. Criar entidade `Empresa` com CNPJ como `id_empresa` (PK único)
2. Enriquecer entidade `Usuario` com avatar, data_nascimento e perfil (admin/gerente/usuario)
3. CRUD de empresas (apenas admin)
4. CRUD de usuários (admin da empresa)
5. Restringir acesso por número de usuários contratados
6. Migrar dados existentes (EMP001 → CNPJ)

---

## 1. Banco de Dados

### Nova tabela: `empresa`
```sql
CREATE TABLE IF NOT EXISTS empresa (
    cnpj VARCHAR(14) PRIMARY KEY,           -- CNPJ sem pontuação, ex: 12345678000190
    razao_social VARCHAR(255) NOT NULL,
    nome_fantasia VARCHAR(255),
    quantidade_usuarios_contratados INTEGER DEFAULT 1,
    ativo BOOLEAN DEFAULT true,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

### Alterações na tabela `usuario`
```sql
ALTER TABLE usuario ADD COLUMN IF NOT EXISTS avatar TEXT;           -- URL ou base64
ALTER TABLE usuario ADD COLUMN IF NOT EXISTS data_nascimento DATE;
ALTER TABLE usuario ADD COLUMN IF NOT EXISTS perfil VARCHAR(20) DEFAULT 'usuario';
-- perfis: 'admin', 'gerente', 'usuario'
```

### Migrar dados existentes
```sql
-- Inserir empresa baseada no usuario existente
INSERT INTO empresa (cnpj, razao_social, nome_fantasia, quantidade_usuarios_contratados)
VALUES ('12345678000190', 'Empresa Teste', 'Kodiak', 5)
ON CONFLICT (cnpj) DO NOTHING;

-- Atualizar usuario para usar CNPJ como id_empresa e definir como admin
UPDATE usuario SET
    id_empresa = '12345678000190',
    id_estabelecimento = '12345678000190',
    cnpj_empresa = '12345678000190',
    perfil = 'admin'
WHERE id = 1;
```

**Arquivo:** `database/003_empresa_e_perfil.sql`

---

## 2. Backend — Entidades

### Nova entidade: `Empresa.cs`
```
KodiakCrm.Core/Entities/Empresa.cs
- Cnpj (string) — PK
- RazaoSocial (string)
- NomeFantasia (string?)
- QuantidadeUsuariosContratados (int)
- Ativo (bool)
- DataCadastro (DateTime)
```

### Atualizar `Usuario.cs`
Adicionar:
- Avatar (string?) — URL ou base64
- DataNascimento (DateTime?)
- Perfil (string) — "admin", "gerente", "usuario"

### Atualizar `BaseEntity.cs`
Sem alterações (empresa não herda BaseEntity pois tem CNPJ como PK).

---

## 3. Backend — Interfaces

### Nova: `IEmpresaRepository.cs`
```
ObterPorCnpjAsync(cnpj)
ObterListaAsync(busca, pagina, itensPorPagina)
CriarAsync(empresa)
AtualizarAsync(empresa)
ContarUsuariosAsync(cnpj) → int
```

### Atualizar `IUsuarioRepository.cs`
Adicionar:
```
ObterListaAsync(idEmpresa, busca, perfil, pagina, itensPorPagina)
ObterPorIdAsync(id) — sem idEmpresa (admin vê todos da empresa)
AtualizarAsync(usuario)
ExcluirAsync(id) — soft delete
ContarPorEmpresaAsync(idEmpresa) → int
```

---

## 4. Backend — Repositories

### Novo: `EmpresaRepository.cs`
- Queries na nova tabela `empresa`

### Atualizar `UsuarioRepository.cs`
- Adicionar queries para lista, update, soft delete, contagem
- Atualizar SELECT para incluir avatar, data_nascimento, perfil

### Atualizar `DapperConfig.cs`
- Adicionar mapeamento para `Empresa`

---

## 5. Backend — Services

### Novo: `EmpresaService.cs`
- CRUD de empresas
- Validação de CNPJ único

### Atualizar `AuthService.cs`
- Retornar perfil no LoginResponse/UsuarioDTO
- Retornar dados da empresa no login (razao_social, quantidade_usuarios_contratados)

### Novo: `UsuarioGestaoService.cs` (ou adicionar no AuthService)
- Criar usuário (verificar limite de usuários da empresa)
- Listar usuários da empresa
- Atualizar usuário
- Excluir usuário (soft delete)
- **Validação de limite:** `ContarPorEmpresaAsync(idEmpresa) < empresa.QuantidadeUsuariosContratados`

### Atualizar `ParceiroService.cs`, `LeadService.cs`, etc.
- Garantir que todas as queries filtram por `id_empresa` do JWT (já feito)

---

## 6. Backend — Controllers

### Novo: `EmpresaController.cs`
- `[Authorize(Roles = "admin")]`
- `GET /api/empresas` — listar (só super-admin no futuro, por enquanto retorna a empresa do usuário)
- `GET /api/empresas/{cnpj}` — obter por CNPJ
- `POST /api/empresas` — criar empresa
- `PUT /api/empresas/{cnpj}` — atualizar empresa

### Novo: `UsuarioGestaoController.cs`
- `[Authorize(Roles = "admin,gerente")]`
- `GET /api/usuarios` — listar usuários da empresa
- `GET /api/usuarios/{id}` — obter usuário
- `POST /api/usuarios` — criar (verifica limite)
- `PUT /api/usuarios/{id}` — atualizar
- `DELETE /api/usuarios/{id}` — soft delete

### Atualizar `AuthController.cs`
- Login retorna: perfil, empresa.razao_social, empresa.quantidade_usuarios_contratados

---

## 7. DTOs

### Novo: `EmpresaDTOs.cs`
```
EmpresaDTO { Cnpj, RazaoSocial, NomeFantasia, QuantidadeUsuariosContratados, Ativo, DataCadastro }
EmpresaCreateDTO { Cnpj, RazaoSocial, NomeFantasia, QuantidadeUsuariosContratados }
EmpresaUpdateDTO { RazaoSocial, NomeFantasia, QuantidadeUsuariosContratados }
```

### Atualizar `AuthDTOs.cs`
```
LoginRequest → sem alteração
LoginResponse → adicionar Perfil no UsuarioDTO
UsuarioDTO → adicionar Perfil, Avatar, DataNascimento
```

### Novo: `UsuarioGestaoDTOs.cs`
```
UsuarioGestaoDTO { Id, Nome, Email, Perfil, Avatar, DataNascimento, Ativo, DataCadastro }
UsuarioCreateDTO { Nome, Email, Senha, Perfil, Avatar?, DataNascimento? }
UsuarioUpdateDTO { Nome, Email?, Perfil, Avatar?, DataNascimento?, Ativo? }
```

---

## 8. Backend — Program.cs

- Registrar `IEmpresaRepository` / `EmpresaRepository`
- Registrar `EmpresaService`
- Registrar `UsuarioGestaoService` (ou o nome que escolhermos)

---

## 9. Frontend — Tipos

### Atualizar `types/index.ts`
```
Empresa { cnpj, razaoSocial, nomeFantasia, quantidadeUsuariosContratados, ativo, dataCadastro }
Usuario → adicionar perfil, avatar, dataNascimento
LoginResponse/Usuario → adicionar perfil
```

### Novo tipo: `UsuarioGestao`
```
UsuarioGestao { id, nome, email, perfil, avatar, dataNascimento, ativo, dataCadastro }
```

---

## 10. Frontend — Páginas novas

### `Empresas.tsx` (listagem)
- Tabela de empresas (só aparece se perfil = admin)
- Botão "Nova Empresa"

### `EmpresaForm.tsx` (cadastro/edit)
- Formulário: CNPJ, Razão Social, Nome Fantasia, Qtd Usuários Contratados

### `Usuarios.tsx` (listagem)
- Tabela de usuários da empresa
- Botão "Novo Usuário"
- Badge do perfil

### `UsuarioForm.tsx` (cadastro/edit)
- Formulário: Nome, Email, Senha, Perfil (select), Avatar (upload URL), Data Nascimento

---

## 11. Frontend — Layout/Nav

### Atualizar `Layout.tsx`
- Adicionar links: "Empresas" e "Usuarios" no sidebar (só para admin)
- Mostrar avatar + nome do usuário no footer

### Atualizar `App.tsx`
- Novas rotas: `/empresas`, `/empresas/novo`, `/empresas/:cnpj`
- Novas rotas: `/usuarios`, `/usuarios/novo`, `/usuarios/:id`

### Atualizar `Login.tsx`
- Campo "Empresa" → agora aceita CNPJ (com máscara ou placeholder adequado)
- Ou: remover campo empresa do login se o backend resolver por outro meio (manter por enquanto)

### Atualizar `AuthContext.tsx`
- Adicionar `perfil` ao usuario no contexto
- Helper `isAdmin`, `isGerente`

---

## 12. Controle de Limite de Usuários

### Fluxo:
1. Admin clica "Novo Usuário"
2. Frontend chama `POST /api/usuarios`
3. Backend: `UsuarioGestaoService.CriarAsync`
   - Busca empresa pelo CNPJ (id_empresa do JWT)
   - Conta usuários ativos: `ContarPorEmpresaAsync(idEmpresa)`
   - Compara com `empresa.QuantidadeUsuariosContratados`
   - Se >= limite → retorna erro "Limite de usuários atingido"
   - Senão → insere

---

## 13. Script de Migração

**Arquivo:** `database/003_empresa_e_perfil.sql`
- Criar tabela `empresa`
- Inserir empresa de teste (CNPJ 12345678000190)
- Adicionar colunas em `usuario` (avatar, data_nascimento, perfil)
- Atualizar usuário existente (id_empresa → CNPJ, perfil → admin)

---

## Ordem de Implementação

1. **Banco:** Script 003 (empresa + colunas usuario + migração)
2. **Core:** Entidade Empresa, atualizar Usuario
3. **Core:** Interfaces (IEmpresaRepository, atualizar IUsuarioRepository)
4. **Core:** DTOs (EmpresaDTOs, atualizar AuthDTOs, UsuarioGestaoDTOs)
5. **Infrastructure:** EmpresaRepository, atualizar UsuarioRepository, DapperConfig
6. **UseCases:** EmpresaService, UsuarioGestaoService, atualizar AuthService
7. **Api:** EmpresaController, UsuarioGestaoController, atualizar AuthController, Program.cs
8. **Frontend:** Types, AuthContext, Layout, Login
9. **Frontend:** Empresas (list+form), Usuarios (list+form)
10. **Testar:** Login, CRUD empresa, CRUD usuário, verificação de limite
