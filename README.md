# Residential Expenses

Aplicacao full-stack para gerenciamento de financas residenciais compartilhadas. Permite que moradores de uma residencia registrem receitas e despesas, categorizem transacoes e acompanhem o balanco financeiro individual e coletivo atraves de um dashboard interativo.

## Screenshots

<div align="center">

### Login & Registro
<p>
  <img src="imgs/FireShot Capture 001 - front - [localhost].png" alt="Login" width="45%">
  <img src="imgs/FireShot Capture 002 - front - [localhost].png" alt="Registro" width="45%">
</p>

### Dashboard
<img src="imgs/FireShot Capture 004 - front - [localhost].png" alt="Dashboard" width="90%">

### Moradores
<p>
  <img src="imgs/FireShot Capture 005 - front - [localhost].png" alt="Moradores" width="45%">
  <img src="imgs/FireShot Capture 006 - front - [localhost].png" alt="Editar Morador" width="45%">
</p>

### Categorias & Transacoes
<p>
  <img src="imgs/FireShot Capture 007 - front - [localhost].png" alt="Categorias" width="45%">
  <img src="imgs/FireShot Capture 008 - front - [localhost].png" alt="Transacoes" width="45%">
</p>

### Perfil
<img src="imgs/FireShot Capture 003 - front - [localhost].png" alt="Perfil" width="90%">

</div>

## Funcionalidades

- **Autenticacao** - Registro e login com JWT, senhas criptografadas com BCrypt
- **Dashboard** - Resumo financeiro com totais de receita, despesa e saldo por morador, grafico comparativo
- **Moradores** - CRUD completo para gerenciar os moradores da residencia
- **Categorias** - Criacao de categorias com proposito (Despesa, Receita ou Ambos)
- **Transacoes** - Registro de receitas e despesas por morador e categoria, filtro por morador
- **Perfil** - Edicao de dados pessoais, alteracao de senha e exclusao de conta
- **Regra de negocio** - Menores de 18 anos so podem registrar despesas, nao receitas

## Tech Stack

### Backend
| Tecnologia | Versao | Uso |
|---|---|---|
| .NET / ASP.NET Core | 9.0 | Framework web |
| Entity Framework Core | 9.0 | ORM |
| PostgreSQL | 15 | Banco de dados |
| JWT | - | Autenticacao |
| BCrypt.Net | 4.1 | Hash de senhas |
| AutoMapper | - | Mapeamento de objetos |
| FluentValidation | - | Validacao de requests |
| Scalar | 2.13 | Documentacao da API |

### Frontend
| Tecnologia | Versao | Uso |
|---|---|---|
| React | 19 | Biblioteca UI |
| TypeScript | 6.0 | Tipagem estatica |
| Vite | 8.0 | Build tool |
| React Router | 7.14 | Roteamento |
| TanStack Query | 5.96 | Gerenciamento de estado servidor |
| React Hook Form + Zod | 7.72 / 4.3 | Formularios e validacao |
| TailwindCSS | 4.2 | Estilizacao |
| shadcn/ui + Radix UI | - | Componentes UI |
| Recharts | 3.8 | Graficos |
| Axios | 1.14 | Cliente HTTP |
| Orval | 8.6 | Geracao de hooks a partir da OpenAPI |

### Infraestrutura
| Tecnologia | Uso |
|---|---|
| Docker Compose | Container do PostgreSQL |

## Arquitetura

### Backend - Clean Architecture

```
back/src/
├── ResidentialExpenses.API            # Controllers, Filters, Middleware
├── ResidentialExpenses.Application    # Use Cases, Validators, Mappings
├── ResidentialExpenses.Domain         # Entities, Enums, Repository Interfaces
├── ResidentialExpenses.Infrastructure # EF Core, Repositories, Security (JWT, BCrypt)
├── ResidentialExpenses.Communication  # Request/Response DTOs
└── ResidentialExpenses.Exceptions     # Custom Exceptions
```

### Frontend - Feature-based

```
front/src/
├── api/                # Axios instance + hooks gerados pelo Orval
├── components/
│   ├── ui/             # Componentes shadcn/ui
│   ├── layout/         # AppLayout, Sidebar, AuthGuard
│   ├── features/       # Dialogs de formulario por feature
│   └── shared/         # DataTable, ConfirmDialog, EmptyState
├── contexts/           # AuthContext + AuthProvider
├── constants/          # Enums e constantes
├── lib/                # Utilitarios (formatCurrency, cn, etc.)
└── pages/              # Paginas (Login, Register, Dashboard, etc.)
```

### Modelo de Dados

```
User ←→ Person (N:N)     Person → Transaction (1:N)     Category → Transaction (1:N)
┌──────────┐              ┌──────────┐                    ┌──────────┐
│ User     │              │ Person   │                    │ Category │
├──────────┤  user_person ├──────────┤                    ├──────────┤
│ Id       │◄────────────►│ Id       │                    │ Id       │
│ Email    │              │ Name     │    ┌─────────────┐ │Descriptio│
│ Name     │              │ Age      │───►│ Transaction │◄│ Purpose  │
│ Password │              └──────────┘    ├─────────────┤ └──────────┘
│ CreatedAt│                              │ Id          │
└──────────┘                              │ Description │  Purpose:
                                          │ Value       │  0 = Despesa
                                          │ Type        │  1 = Receita
                                          │ PersonId    │  2 = Ambos
                                          │ CategoryId  │
                                          └─────────────┘  Type:
                                                           0 = Despesa
                                                           1 = Receita
```

## Pre-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js](https://nodejs.org/) (v18+)
- [Docker](https://www.docker.com/) e Docker Compose

## Como Executar

### 1. Banco de Dados

Na raiz do projeto, crie um arquivo `.env`:

```env
POSTGRESQL_USERNAME=seu_usuario
POSTGRESQL_PASSWORD=sua_senha
POSTGRESQL_DATABASE=residentialexpenses
POSTGRESQL_PORT=5432
```

Suba o container do PostgreSQL:

```bash
docker compose up -d
```

### 2. Backend

```bash
cd back/src/ResidentialExpenses.API
```

Configure `appsettings.Development.json` com a connection string e chave JWT:

```json
{
  "Settings": {
    "Connection": {
      "DatabaseConnection": "Host=localhost;Port=5432;Database=residentialexpenses;Username=seu_usuario;Password=sua_senha;"
    },
    "Jwt": {
      "ExpiresMinutes": 120,
      "SigningKey": "sua-chave-secreta-com-pelo-menos-64-caracteres-aqui-para-seguranca"
    }
  }
}
```

Execute:

```bash
dotnet run
```

A API estara disponivel em `https://localhost:7248`. A documentacao interativa fica em `https://localhost:7248/scalar/v1`.

### 3. Frontend

```bash
cd front
npm install
```

Para gerar os hooks da API (requer backend rodando):

```bash
npm run generate
```

Inicie o servidor de desenvolvimento:

```bash
npm run dev
```

O frontend estara disponivel em `http://localhost:5173`.

## Endpoints da API

### Autenticacao

| Metodo | Rota | Descricao | Auth |
|---|---|---|---|
| `POST` | `/api/login` | Login do usuario | Nao |
| `POST` | `/api/user` | Registro de novo usuario | Nao |
| `GET` | `/api/user` | Perfil do usuario | Sim |
| `PUT` | `/api/user` | Atualizar perfil | Sim |
| `DELETE` | `/api/user` | Excluir conta | Sim |

### Moradores

| Metodo | Rota | Descricao | Auth |
|---|---|---|---|
| `POST` | `/api/person` | Cadastrar morador | Sim |
| `GET` | `/api/person` | Listar moradores | Sim |
| `PUT` | `/api/person/{id}` | Atualizar morador | Sim |
| `DELETE` | `/api/person/{id}` | Remover morador | Sim |

### Categorias

| Metodo | Rota | Descricao | Auth |
|---|---|---|---|
| `POST` | `/api/category` | Criar categoria | Sim |
| `GET` | `/api/category` | Listar categorias | Sim |

### Transacoes

| Metodo | Rota | Descricao | Auth |
|---|---|---|---|
| `POST` | `/api/transaction` | Registrar transacao | Sim |
| `GET` | `/api/transaction/person/{personId}` | Transacoes por morador | Sim |
| `GET` | `/api/transaction/totals` | Resumo de totais | Sim |

Todas as respostas seguem o formato envelope:

```json
{
  "success": true,
  "data": { },
  "errors": [],
  "metadata": { }
}
```

## Scripts Disponiveis

### Frontend (`front/`)

| Script | Descricao |
|---|---|
| `npm run dev` | Servidor de desenvolvimento |
| `npm run build` | Build de producao |
| `npm run preview` | Preview do build |
| `npm run lint` | Lint com ESLint |
| `npm run generate` | Gera hooks da API via Orval (requer backend) |
| `npm run generate:local` | Gera hooks a partir do schema local |

### Backend (`back/`)

| Comando | Descricao |
|---|---|
| `dotnet run` | Executa a API |
| `dotnet build` | Compila o projeto |
| `dotnet test` | Executa os testes |
