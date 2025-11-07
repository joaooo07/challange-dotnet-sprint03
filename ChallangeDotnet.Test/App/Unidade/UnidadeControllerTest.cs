using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ChallangeDotnet.Application.Interface;
using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Domain.Entities;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ChallangeDotnet.Tests.App.Unidade
{
    public class UnidadeTestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string Scheme = "TestAuth";

        public UnidadeTestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder) : base(options, logger, encoder) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "tester"),
                new Claim(ClaimTypes.Role, "admin")
            };

            var identity = new ClaimsIdentity(claims, Scheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

    public class UnidadeWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<IUnidadeUseCase> UnidadeUseCaseMock { get; } = new();

        protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(IUnidadeUseCase));
                services.AddSingleton(UnidadeUseCaseMock.Object);

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = UnidadeTestAuthHandler.Scheme;
                    options.DefaultChallengeScheme = UnidadeTestAuthHandler.Scheme;
                })
                .AddScheme<AuthenticationSchemeOptions, UnidadeTestAuthHandler>(
                    UnidadeTestAuthHandler.Scheme, _ => { });
            });
        }
    }

    public class UnidadeControllerTest : IClassFixture<UnidadeWebApplicationFactory>
    {
        private readonly UnidadeWebApplicationFactory _factory;

        public UnidadeControllerTest(UnidadeWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact(DisplayName = "GET - Retorna todas as unidades")]
        [Trait("Controller", "Unidade")]
        public async Task Get_DeveRetornarTodasUnidades()
        {
            var unidades = new PageResultModel<IEnumerable<UnidadeEntity>>
            {
                Data = new List<UnidadeEntity>
                {
                    new UnidadeEntity { Id = 1, Nome = "Unidade A", Codigo = "UA", Ativa = true, Observacao = "Principal" },
                    new UnidadeEntity { Id = 2, Nome = "Unidade B", Codigo = "UB", Ativa = true, Observacao = "Secundária" }
                },
                TotalRegistros = 2,
                Deslocamento = 0,
                RegistrosRetornado = 2
            };

            var retorno = OperationResult<PageResultModel<IEnumerable<UnidadeEntity>>>.Success(unidades, 200);

            _factory.UnidadeUseCaseMock
                .Setup(x => x.ObterTodasUnidadesAsync(0, 3))
                .ReturnsAsync(retorno);

            using var client = _factory.CreateClient();

            var response = await client.GetAsync("api/v1/unidade?Deslocamento=0&RegistrosRetornado=3");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "GET - Retorna unidade por ID")]
        [Trait("Controller", "Unidade")]
        public async Task Get_DeveRetornarUnidadePorId()
        {
            var unidade = new UnidadeEntity { Id = 1, Nome = "Unidade Teste", Codigo = "UT01", Ativa = true };
            var retorno = OperationResult<UnidadeEntity?>.Success(unidade, 200);

            _factory.UnidadeUseCaseMock
                .Setup(x => x.ObterUmaUnidadeAsync(1))
                .ReturnsAsync(retorno);

            using var client = _factory.CreateClient();
            var response = await client.GetAsync("api/v1/unidade/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST - Cria uma nova unidade")]
        [Trait("Controller", "Unidade")]
        public async Task Post_DeveCriarUnidade()
        {
            var unidadeCriada = new UnidadeEntity { Id = 1, Nome = "Nova Unidade", Codigo = "NU01", Ativa = true };
            var retorno = OperationResult<UnidadeEntity?>.Success(unidadeCriada, (int)HttpStatusCode.Created);

            _factory.UnidadeUseCaseMock
                .Setup(x => x.AdicionarUnidadeAsync(It.IsAny<UnidadeDto>()))
                .ReturnsAsync(retorno);

            using var client = _factory.CreateClient();

            var content = new StringContent(
                JsonSerializer.Serialize(new
                {
                    Codigo = "NU01",
                    Nome = "Nova Unidade",
                    Ativa = true,
                    Observacao = "Teste"
                }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("api/v1/unidade", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "PUT - Atualiza uma unidade existente")]
        [Trait("Controller", "Unidade")]
        public async Task Put_DeveAtualizarUnidade()
        {
            var unidadeAtualizada = new UnidadeEntity { Id = 1, Nome = "Unidade Atualizada", Codigo = "UA01", Ativa = false };
            var retorno = OperationResult<UnidadeEntity?>.Success(unidadeAtualizada, 200);

            _factory.UnidadeUseCaseMock
                .Setup(x => x.EditarUnidadeAsync(1, It.IsAny<UnidadeDto>()))
                .ReturnsAsync(retorno);

            using var client = _factory.CreateClient();

            var content = new StringContent(
                JsonSerializer.Serialize(new
                {
                    Codigo = "UA01",
                    Nome = "Unidade Atualizada",
                    Ativa = false,
                    Observacao = "Atualizada"
                }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PutAsync("api/v1/unidade/1", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "DELETE - Remove uma unidade existente")]
        [Trait("Controller", "Unidade")]
        public async Task Delete_DeveRemoverUnidade()
        {
            var unidadeRemovida = new UnidadeEntity { Id = 1, Nome = "Unidade X", Codigo = "UX01", Ativa = false };
            var retorno = OperationResult<UnidadeEntity?>.Success(unidadeRemovida, 200);

            _factory.UnidadeUseCaseMock
                .Setup(x => x.DeletarUnidadeAsync(1))
                .ReturnsAsync(retorno);

            using var client = _factory.CreateClient();

            var response = await client.DeleteAsync("api/v1/unidade/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
