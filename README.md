---

# 🚀 ChallangeDotnet API

API RESTful construída em **.NET 8 (Web API)**, seguindo boas práticas de arquitetura **DDD (Domain Driven Design)** e princípios REST.
O projeto foi desenvolvido como parte da disciplina **Advanced Business Development with .NET**.

---



---

## 🏗️ Arquitetura do Projeto

O projeto foi estruturado em **camadas**, seguindo Clean Architecture:

📂 **Application**

* `Dtos` → Objetos de transferência de dados
* `Interface` → Interfaces dos casos de uso
* `Mapper` → Conversão entre DTOs e Entidades
* `UseCase` → Implementações dos casos de uso (lógica de aplicação)

📂 **Domain**

* `Entities` → Entidades de negócio
* `Interface` → Contratos de repositórios

📂 **Infrastructure**

* `Data/AppData` → `ApplicationContext` (DbContext com EF Core + Oracle)
* `Data/Repositories` → Implementações dos repositórios

📂 **Presentation**

* `Controllers` → Exposição dos endpoints REST

📂 **Migrations**

* Histórico do EF Core

📄 **appsettings.json**

* Configurações (conexão com banco Oracle, etc.)

---

## 📦 Entidades Principais

1. **Usuários** → Representam pessoas que interagem com o sistema
2. **Unidades** → Estruturas organizacionais (filiais/pátios)
3. **Motos** → Veículos que podem ser gerenciados pelo sistema
4. **Vagas** → Entidade em implementação (espaços/vagas para veículos)

### Justificativa do domínio

* **Usuários** → Controle de acesso e autenticação
* **Unidades** → Organização de recursos por local/filial
* **Motos** → Inventário físico de veículos (ex.: gestão de pátio/estacionamento)
* **Vagas** → Recursos limitados a serem alocados para veículos (domínio realista e relacionado às motos)

---

## 🔗 Endpoints REST

Todos os endpoints seguem boas práticas REST:

* **Paginação** (`Deslocamento`, `RegistrosRetornado`)
* **HATEOAS** (links de navegação incluídos nas respostas)
* **Status codes adequados** (`200`, `201`, `204`, `400`, `404`)

### Exemplos de endpoints:

#### Usuário

* `GET /api/v1/usuario` → Lista usuários (paginado)
* `GET /api/v1/usuario/{id}` → Obtém usuário por ID
* `POST /api/v1/usuario` → Cria novo usuário
* `PUT /api/v1/usuario/{id}` → Edita usuário
* `DELETE /api/v1/usuario/{id}` → Remove usuário
* `POST /api/v1/usuario/login` → Login simples (sem token)

#### Moto

* `GET /api/v1/moto` → Lista motos (paginado)
* `GET /api/v1/moto/{id}` → Obtém moto por ID
* `POST /api/v1/moto` → Cria nova moto
* `PUT /api/v1/moto/{id}` → Edita moto
* `DELETE /api/v1/moto/{id}` → Remove moto

#### Unidade

* `GET /api/v1/unidade` → Lista unidades
* (demais endpoints seguem o mesmo padrão CRUD)

---

## 📘 Swagger / OpenAPI

O projeto utiliza **Swagger** para documentação automática:

* Descrição detalhada de **endpoints e parâmetros**
* **Exemplos de payloads** (request/response) com `SwaggerRequestExample` e `SwaggerResponseExample`
* Modelos de dados descritos a partir das entidades/DTOs

Acesse em:

```
https://localhost:5001/swagger
```

---

## ⚙️ Como executar a API

### Pré-requisitos

* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* Banco Oracle XE (ou compatível)

### Passos

1. Clone o repositório:

   ```bash
   git clone https://github.com/<seu-usuario>/ChallangeDotnet.git
   cd ChallangeDotnet
   ```

2. Configure a connection string no `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "Oracle": "User Id=system;Password=oracle;Data Source=localhost:1521/XEPDB1"
   }
   ```

3. Rode as migrations:

   ```bash
   dotnet ef database update --context ApplicationContext
   ```

4. Execute a API:

   ```bash
   dotnet run --project ChallangeDotnet
   ```

5. Acesse o Swagger em:

   ```
   http://localhost:5000/swagger
   ```

---

## 🧪 Testes

O projeto possui testes automatizados com **xUnit**.

Para rodar os testes:

```bash
dotnet test
```

---

## 📖 Exemplos de uso (via curl)

### Criar um usuário

```bash
curl -X POST "https://localhost:5001/api/v1/usuario" \
-H "Content-Type: application/json" \
-d '{
  "nome": "João da Silva",
  "email": "joao@email.com",
  "senha": "123456",
  "ativo": true
}'
```

### Criar uma moto

```bash
curl -X POST "https://localhost:5001/api/v1/moto" \
-H "Content-Type: application/json" \
-d '{
  "modelo": "CG 160 Titan",
  "marca": "Honda",
  "ano": 2024
}'
```

---

## ✅ Checklist de Requisitos

* [x] 3+ entidades principais (Usuário, Unidade, Moto)
* [x] Endpoints CRUD com boas práticas REST (paginação, HATEOAS, status codes)
* [x] Swagger/OpenAPI documentado com exemplos de payloads
* [x] Repositório público no GitHub com README completo
* [x] Testes automatizados configurados (`dotnet test`)
* [ ] VagaController implementado (em progresso)


