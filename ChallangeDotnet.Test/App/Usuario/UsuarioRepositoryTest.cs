using ChallangeDotnet.Domain.Entities;
using ChallangeDotnet.Infraestructure.Data.AppData;
using ChallangeDotnet.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChallangeDotnet.Tests.App.Usuario
{
    public class UsuarioRepositoryTest
    {
        private ApplicationContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationContext(options);
        }

        private static UsuarioEntity BuildUsuario(
            string? nome = null,
            string? email = null,
            string? senha = null,
            bool ativo = true)
        {
            return new UsuarioEntity
            {
                Nome = nome ?? "Usuário Teste",
                Email = email ?? "teste@teste.com",
                Senha = senha ?? "123456",
                Ativo = ativo
            };
        }

        [Fact(DisplayName = "ObterTodosAsync - Retorna todos os usuários")]
        [Trait("Repository", "Usuario")]
        public async Task ObterTodos_DeveRetornarUsuarios()
        {
            using var context = CreateContext();
            var repository = new UsuarioRepository(context);

            var u1 = BuildUsuario("João", "joao@email.com");
            var u2 = BuildUsuario("Maria", "maria@email.com");
            var u3 = BuildUsuario("Pedro", "pedro@email.com");

            context.Usuario.AddRange(u1, u2, u3);
            await context.SaveChangesAsync();

            var retorno = await repository.ObterTodosAsync(0, 2);

            Assert.NotNull(retorno);
            Assert.Equal(3, retorno.TotalRegistros);
            Assert.Equal(2, retorno.Data.Count());
        }

        [Fact(DisplayName = "ObterUmAsync - Retorna usuário por ID")]
        [Trait("Repository", "Usuario")]
        public async Task ObterUm_DeveRetornarUsuario()
        {
            using var context = CreateContext();
            var repository = new UsuarioRepository(context);

            var usuario = BuildUsuario("Carlos", "carlos@email.com");
            context.Usuario.Add(usuario);
            await context.SaveChangesAsync();

            var retorno = await repository.ObterUmAsync(usuario.Id);

            Assert.NotNull(retorno);
            Assert.Equal(usuario.Id, retorno.Id);
            Assert.Equal("Carlos", retorno.Nome);
        }
    }
}
