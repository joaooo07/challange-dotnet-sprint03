using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using ChallangeDotnet.Application.Interface;

namespace ChallangeDotnet.Test.App.Usuario
{

    public class UsuarioWebApplicationFactory : WebApplicationFactory<Program>
    {

        public Mock<IUsuarioUseCase> UsuarioUseCaseMock { get; } = new();

        protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {

                services.RemoveAll(typeof(IUsuarioUseCase));
                services.AddSingleton(UsuarioUseCaseMock.Object);

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = UsuarioTestAuthHandler.Scheme;
                    options.DefaultChallengeScheme = UsuarioTestAuthHandler.Scheme;
                })
                .AddScheme<AuthenticationSchemeOptions, UsuarioTestAuthHandler>(
                    UsuarioTestAuthHandler.Scheme, _ => { });
            });
        }
    }
}
