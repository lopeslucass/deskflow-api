# DeskFlow API

API REST desenvolvida em ASP.NET Core para gerenciamento de chamados de Helpdesk de TI.

O projeto permite cadastrar categorias, abrir e acompanhar chamados, registrar interações durante o atendimento e finalizar chamados com a solução aplicada.

## Tecnologias utilizadas

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server
- Git e GitHub

## Arquitetura

O projeto foi organizado utilizando separação em camadas:

```text
Controller
    ↓
Service
    ↓
Repository
    ↓
Entity Framework Core
    ↓
SQL Server
```

### Controllers

Responsáveis por receber as requisições HTTP, encaminhar as operações para os Services e retornar as respostas da API.

### Services

Concentram as regras de negócio e validações da aplicação.

### Repositories

Responsáveis pelo acesso aos dados através do Entity Framework Core.

### Models

Contêm as entidades, enums e DTOs utilizados pela aplicação.

### Middlewares

A aplicação possui um middleware global para tratamento de exceções, responsável por converter erros da aplicação em respostas HTTP apropriadas sem expor detalhes internos do servidor.

## Entidades principais

### Categoria

Representa uma categoria utilizada para classificar os chamados.

Principais propriedades:

- Id
- Nome

Uma categoria pode possuir vários chamados.

### Chamado

Representa uma solicitação de suporte.

Principais propriedades:

- Id
- Titulo
- Descricao
- Prioridade
- Status
- SolicitanteNome
- DataAbertura
- DataFechamento
- Solucao
- CategoriaId

As prioridades disponíveis são:

- Baixa
- Media
- Alta

Os possíveis status são:

- Aberto
- EmAndamento
- Fechado

### Interacao

Representa uma mensagem ou atualização registrada durante o atendimento de um chamado.

Principais propriedades:

- Id
- ChamadoId
- Autor
- Mensagem
- DataRegistro

Um chamado pode possuir várias interações.

## Regras de negócio

Ao criar um chamado, o sistema define automaticamente o status como `Aberto` e registra a data e hora de abertura.

Um chamado no status `Aberto` pode ser iniciado, passando para `EmAndamento`.

Para fechar um chamado é obrigatório informar uma solução. Ao finalizar o atendimento, o status passa para `Fechado` e a data de fechamento é registrada automaticamente.

Não é permitido adicionar novas interações em chamados fechados.

Uma categoria que possui chamados vinculados não pode ser excluída. Essa regra é validada pela aplicação e também protegida no banco de dados através da restrição da chave estrangeira.

## Endpoints

### Categorias

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/categorias` | Lista todas as categorias |
| GET | `/api/categorias/{id}` | Busca uma categoria pelo ID |
| POST | `/api/categorias` | Cadastra uma categoria |
| PUT | `/api/categorias/{id}` | Atualiza uma categoria |
| DELETE | `/api/categorias/{id}` | Exclui uma categoria |

### Chamados

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/chamados` | Lista os chamados |
| GET | `/api/chamados/{id}` | Busca os detalhes de um chamado |
| POST | `/api/chamados` | Abre um novo chamado |
| PATCH | `/api/chamados/{id}/iniciar` | Inicia o atendimento de um chamado |
| PATCH | `/api/chamados/{id}/fechar` | Finaliza um chamado |
| POST | `/api/chamados/{id}/interacoes` | Adiciona uma interação ao chamado |

As operações de iniciar e fechar utilizam `PATCH`, pois representam alterações parciais no estado de um chamado.

## Filtros de chamados

O endpoint:

```http
GET /api/chamados
```

aceita filtros opcionais através da query string:

- `status`
- `prioridade`
- `categoriaId`

Os filtros podem ser utilizados individualmente ou combinados.

Exemplo:

```http
GET /api/chamados?status=Aberto&prioridade=Alta&categoriaId=2
```

## Exemplos de requisições

### Criar categoria

```http
POST /api/categorias
Content-Type: application/json
```

```json
{
  "nome": "Hardware"
}
```

### Criar chamado

```http
POST /api/chamados
Content-Type: application/json
```

```json
{
  "titulo": "Computador não inicia",
  "descricao": "O computador não apresenta imagem ao ser ligado.",
  "prioridade": "Alta",
  "solicitanteNome": "Lucas",
  "categoriaId": 1
}
```

### Iniciar chamado

```http
PATCH /api/chamados/1/iniciar
```

### Fechar chamado

```http
PATCH /api/chamados/1/fechar
Content-Type: application/json
```

```json
{
  "solucao": "Memória RAM substituída e equipamento testado."
}
```

### Adicionar interação

```http
POST /api/chamados/1/interacoes
Content-Type: application/json
```

```json
{
  "autor": "Lucas",
  "mensagem": "Equipamento encaminhado para análise."
}
```

## Banco de dados

A aplicação utiliza SQL Server com Entity Framework Core.

A estrutura do banco é controlada através de migrations.

Para aplicar as migrations:

```bash
dotnet ef database update
```

Também está disponível um script SQL gerado a partir das migrations em:

```text
DeskFlow.API/Scripts/DeskFlow.sql
```

O banco possui as tabelas principais:

```text
Categorias
Chamados
Interacoes
```

Os relacionamentos são:

```text
Categoria 1 ───── N Chamados

Chamado   1 ───── N Interacoes
```

A exclusão de uma categoria com chamados vinculados é restringida para preservar a integridade dos dados.

## Como executar o projeto

### Pré-requisitos

Para executar a aplicação é necessário possuir:

- .NET 10 SDK
- SQL Server
- Entity Framework Core CLI

Clone o repositório:

```bash
git clone https://github.com/lopeslucass/deskflow-api.git
```

Entre na pasta do projeto:

```bash
cd deskflow-api/DeskFlow.API
```

Restaure as dependências:

```bash
dotnet restore
```

Configure a conexão com o SQL Server no arquivo `appsettings.json`.

Exemplo:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=DeskFlowDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Aplique as migrations:

```bash
dotnet ef database update
```

Execute a API:

```bash
dotnet run
```

O endereço utilizado pela aplicação será exibido no terminal após a inicialização.

## Estrutura do projeto

```text
DeskFlow.API/
├── Controllers/
├── Data/
├── Exceptions/
├── Middlewares/
├── Migrations/
├── Models/
│   ├── DTOs/
│   ├── Entidades/
│   └── Enums/
├── Repositories/
├── Scripts/
├── Services/
├── Program.cs
└── appsettings.json
```

## Tratamento de erros

A API possui tratamento global de exceções através de um middleware personalizado.

Entre os códigos HTTP utilizados estão:

| Código | Significado |
|---|---|
| 200 | Requisição realizada com sucesso |
| 201 | Recurso criado com sucesso |
| 204 | Operação realizada sem conteúdo de retorno |
| 400 | Requisição ou dados inválidos |
| 404 | Recurso não encontrado |
| 409 | Conflito com uma regra de negócio |
| 500 | Erro interno inesperado |

Erros internos são registrados através do sistema de logging da aplicação sem expor detalhes técnicos na resposta enviada ao cliente.

## Autor

Desenvolvido por Lucas Lopes de Freitas como projeto avaliativo de desenvolvimento de API REST com ASP.NET Core.