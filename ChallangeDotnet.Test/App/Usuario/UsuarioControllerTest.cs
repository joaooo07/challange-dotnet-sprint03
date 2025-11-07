using System.Net;
using System.Text;
using System.Text.Json;
using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Domain.Entities;
using Moq;

namespace ChallangeDotnet.Test.App.Usuario
{
    public class UsuarioControllerTest : IClassFixture<UsuarioWebApplicationFactory>
    {
        private readonly UsuarioWebApplicationFactory _factory;

        public UsuarioControllerTest(UsuarioWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact(DisplayName = "GET - Retorna todos os usuários")]
        [Trait("Controller", "Usuario")]
        public async Task Get_RetornaUsuarios()
        {
            var usuarios = new PageResultModel<IEnumerable<UsuarioEntity>>
            {
                Data = new List<UsuarioEntity>
                {
                    new UsuarioEntity { Id = 1, Nome = "João", Email = "joao@test.com", Senha = "123", Ativo = true },
                    new UsuarioEntity { Id = 2, Nome = "Maria", Email = "maria@test.com", Senha = "456", Ativo = true }
                },
                Deslocamento = 0,
                RegistrosRetornado = 2,
                TotalRegistros = 2
            };

            var retorno = OperationResult<PageResultModel<IEnumerable<UsuarioEntity>>>.Success(usuarios, 200);

            _factory.UsuarioUseCaseMock
                .Setup(x => x.ObterTodosUsuariosAsync(0, 3))
                .ReturnsAsync(retorno);

            using var client = _factory.CreateClient();
            var response = await client.GetAsync("api/v1/usuario");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "GET - Retorna NoContent quando não há usuários")]
        [Trait("Controller", "Usuario")]
        public async Task Get_RetornaNoContent()
        {
            var retorno = OperationResult<PageResultModel<IEnumerable<UsuarioEntity>>>.Failure("Nenhum usuário encontrado", (int)HttpStatusCode.NoContent);

            _factory.UsuarioUseCaseMock
                .Setup(x => x.ObterTodosUsuariosAsync(0, 3))
                .ReturnsAsync(retorno);

            using var client = _factory.CreateClient();
            var response = await client.GetAsync("api/v1/usuario");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(DisplayName = "GET - Retorna usuário por ID")]
        [Trait("Controller", "Usuario")]
        public async Task GetById_RetornaUsuario()
        {
            var usuario = new UsuarioEntity { Id = 1, Nome = "João", Email = "joao@test.com", Senha = "123", Ativo = true };
            var retorno = OperationResult<UsuarioEntity?>.Success(usuario, 200);

            _factory.UsuarioUseCaseMock
                .Setup(x => x.ObterUmUsuarioAsync(1))
                .ReturnsAsync(retorno);

            using var client = _factory.CreateClient();
            var response = await client.GetAsync("api/v1/usuario/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST - Cria um novo usuário")]
        [Trait("Controller", "Usuario")]
        public async Task Post_CriaUsuario()
        {
            var usuarioNovo = new UsuarioEntity { Id = 1, Nome = "João", Email = "joao@test.com", Senha = "123", Ativo = true };
            var retorno = OperationResult<UsuarioEntity?>.Success(usuarioNovo, (int)HttpStatusCode.Created);

            _factory.UsuarioUseCaseMock
                .Setup(x => x.AdicionarUsuarioAsync(It.IsAny<UsuarioDto>()))
                .ReturnsAsync(retorno);

            using var client = _factory.CreateClient();

            var content = new StringContent(JsonSerializer.Serialize(new UsuarioDto("João", "joao@test.com", "123", true)), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/v1/usuario", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "PUT - Atualiza um usuário")]
        [Trait("Controller", "Usuario")]
        public async Task Put_AtualizaUsuario()
        {
            var usuarioAtualizado = new UsuarioEntity { Id = 1, Nome = "João Atualizado", Email = "joao@test.com", Senha = "1234", Ativo = true };
            var retorno = OperationResult<UsuarioEntity?>.Success(usuarioAtualizado, 200);

            _factory.UsuarioUseCaseMock
                .Setup(x => x.EditarUsuarioAsync(1, It.IsAny<UsuarioDto>()))
                .ReturnsAsync(retorno);

            using var client = _factory.CreateClient();

            var content = new StringContent(JsonSerializer.Serialize(new UsuarioDto("João Atualizado", "joao@test.com", "1234", true)), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("api/v1/usuario/1", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "DELETE - Deleta um usuário")]
        [Trait("Controller", "Usuario")]
        public async Task Delete_DeletaUsuario()
        {
            var usuarioDeletado = new UsuarioEntity { Id = 1, Nome = "João", Email = "joao@test.com", Senha = "123", Ativo = true };
            var retorno = OperationResult<UsuarioEntity?>.Success(usuarioDeletado, 200);

            _factory.UsuarioUseCaseMock
                .Setup(x => x.DeletarUsuarioAsync(1))
                .ReturnsAsync(retorno);

            using var client = _factory.CreateClient();
            var response = await client.DeleteAsync("api/v1/usuario/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
