
# 🚀 ChallangeDotnet API

API RESTful construída em **.NET 8 (Web API)**, seguindo boas práticas de arquitetura **DDD (Domain Driven Design)** e princípios REST.
O projeto foi desenvolvido como parte da disciplina **Advanced Business Development with .NET**.

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
* **Vagas** → Recursos limitados a serem alocados (domínio realista e relacionado às motos)

---

## 🔗 Endpoints REST

Todos os endpoints seguem boas práticas REST:

* **Paginação** (`Deslocamento`, `RegistrosRetornado`)
* **HATEOAS** (links de navegação nas respostas)
* **Status codes adequados** (`200`, `201`, `204`, `400`, `404`)

### Usuário

* `GET /api/v1/usuario` → Lista usuários (paginado)
* `GET /api/v1/usuario/{id}` → Obtém usuário por ID
* `POST /api/v1/usuario` → Cria novo usuário
* `PUT /api/v1/usuario/{id}` → Edita usuário
* `DELETE /api/v1/usuario/{id}` → Remove usuário
* `POST /api/v1/usuario/login` → Login simples (sem token)

### Moto

* `GET /api/v1/moto` → Lista motos (paginado)
* `GET /api/v1/moto/{id}` → Obtém moto por ID
* `POST /api/v1/moto` → Cria nova moto
* `PUT /api/v1/moto/{id}` → Edita moto
* `DELETE /api/v1/moto/{id}` → Remove moto

### Unidade

* `GET /api/v1/unidade` → Lista unidades
* (demais endpoints seguem o mesmo padrão CRUD)

---

## 🤖 Endpoint de Machine Learning (ML)

O projeto inclui um endpoint de **Machine Learning**, responsável por realizar predições com base em modelos **ML.NET** integrados.

### Endpoint ML

* `POST /api/v1/ml/predict` → Recebe dados e retorna uma previsão baseada em modelo treinado

#### Exemplo de requisição

```bash
curl -X POST "https://localhost:5001/api/v1/ml/predict" \
-H "Content-Type: application/json" \
-d '{
  "cilindradas": 160,
  "peso": 120,
  "ano": 2023
}'
```

#### Exemplo de resposta

```json
{
  "predicao": "Aprovado",
  "confianca": 0.87
}
```

O controlador `MLController` é responsável por carregar o modelo `.zip` do ML.NET e gerar a inferência em tempo real.

---

## 📘 Swagger / OpenAPI

O projeto utiliza **Swagger** para documentação automática:

* Descrição detalhada de **endpoints e parâmetros**
* **Exemplos de payloads** (`SwaggerRequestExample`, `SwaggerResponseExample`)
* Modelos de dados descritos a partir das entidades e DTOs

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
   git clone https://github.com/joaooo07/challange-dotnet-sprint03.git
   cd ChallangeDotnet
   ```

2. Configure a connection string:

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

5. Acesse o Swagger:

   ```
   http://localhost:5000/swagger
   ```

---

## 🧪 Testes Automatizados

O projeto possui uma suíte completa de testes com **xUnit**, **Moq**, **FluentAssertions** e **WebApplicationFactory**.

### Estrutura de Testes

📂 **ChallangeDotnet.Test**

```
├── UsuarioControllerTest.cs      → Testa endpoints REST de usuário
├── UsuarioRepositoryTest.cs      → Testa camada de repositório (EF Core InMemory)
├── UsuarioUseCaseTest.cs         → Testa lógica de negócio (casos de uso)
├── UnidadeControllerTest.cs      → Testes da controller de Unidade
├── UnidadeRepositoryTest.cs      → Testes de persistência de Unidade
├── UnidadeUseCaseTest.cs         → Testes de regras de negócio de Unidade
└── MLControllerTest.cs           → Testa endpoint de predição ML.NET
```

### Execução dos testes

```bash
dotnet test
```

### Exemplo de teste de integração

```csharp
[Fact(DisplayName = "POST /api/v1/usuario - Deve criar um novo usuário")]
public async Task Post_DeveCriarUsuario()
{
    var client = _factory.CreateClient();
    var response = await client.PostAsJsonAsync("/api/v1/usuario", new {
        Nome = "Teste API",
        Email = "teste@teste.com",
        Senha = "123456",
        Ativo = true
    });

    response.StatusCode.Should().Be(HttpStatusCode.OK);
}
```

---

## ✅ Checklist de Requisitos

* [x] 3+ entidades principais (Usuário, Unidade, Moto)
* [x] Endpoints CRUD com boas práticas REST
* [x] Swagger/OpenAPI documentado com exemplos
* [x] Repositório público no GitHub com README completo
* [x] Testes automatizados (`dotnet test`)
* [x] Endpoint ML.NET implementado
* [ ] VagaController em desenvolvimento

---

