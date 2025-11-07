using ChallangeDotnet.Domain.Entities;
using ChallangeDotnet.Infraestructure.Data.AppData;
using ChallangeDotnet.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChallangeDotnet.Tests.App.Unidade
{
    public class UnidadeRepositoryTest
    {
        private ApplicationContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationContext(options);
        }

        private static UnidadeEntity BuildUnidade(
            string? nome = null,
            string? codigo = null,
            string? observacao = null,
            bool ativa = true)
        {
            return new UnidadeEntity
            {
                Nome = nome ?? "Unidade Teste",
                Codigo = codigo ?? "UNI-001",
                Observacao = observacao ?? "Unidade de teste automática",
                Ativa = ativa
            };
        }

        [Fact(DisplayName = "ObterTodosAsync - Retorna todas as unidades")]
        [Trait("Repository", "Unidade")]
        public async Task ObterTodos_DeveRetornarUnidades()
        {
            using var context = CreateContext();
            var repository = new UnidadeRepository(context);

            var u1 = BuildUnidade("Unidade A", "A01");
            var u2 = BuildUnidade("Unidade B", "B02");
            var u3 = BuildUnidade("Unidade C", "C03");

            context.Unidade.AddRange(u1, u2, u3);
            await context.SaveChangesAsync();

            var retorno = await repository.ObterTodosAsync(0, 2);

            Assert.NotNull(retorno);
            Assert.Equal(3, retorno.TotalRegistros);
            Assert.Equal(2, retorno.Data.Count());
        }

        [Fact(DisplayName = "ObterUmAsync - Retorna unidade por ID")]
        [Trait("Repository", "Unidade")]
        public async Task ObterUm_DeveRetornarUnidade()
        {
            using var context = CreateContext();
            var repository = new UnidadeRepository(context);

            var unidade = BuildUnidade("Unidade X", "X01");
            context.Unidade.Add(unidade);
            await context.SaveChangesAsync();

            var retorno = await repository.ObterUmAsync(unidade.Id);

            Assert.NotNull(retorno);
            Assert.Equal("Unidade X", retorno.Nome);
            Assert.Equal("X01", retorno.Codigo);
        }

        [Fact(DisplayName = "AdicionarAsync - Adiciona nova unidade")]
        [Trait("Repository", "Unidade")]
        public async Task Adicionar_DeveAdicionarUnidade()
        {
            using var context = CreateContext();
            var repository = new UnidadeRepository(context);

            var unidade = BuildUnidade("Nova Unidade", "N01");
            var retorno = await repository.AdicionarAsync(unidade);

            Assert.NotNull(retorno);
            Assert.Equal("Nova Unidade", retorno.Nome);
        }

        [Fact(DisplayName = "EditarAsync - Atualiza unidade existente")]
        [Trait("Repository", "Unidade")]
        public async Task Editar_DeveAtualizarUnidade()
        {
            using var context = CreateContext();
            var repository = new UnidadeRepository(context);

            var unidade = BuildUnidade("Antiga", "A99");
            context.Unidade.Add(unidade);
            await context.SaveChangesAsync();

            unidade.Nome = "Atualizada";
            var retorno = await repository.EditarAsync(unidade.Id, unidade);

            Assert.NotNull(retorno);
            Assert.Equal("Atualizada", retorno.Nome);
        }

        [Fact(DisplayName = "DeletarAsync - Remove unidade existente")]
        [Trait("Repository", "Unidade")]
        public async Task Deletar_DeveRemoverUnidade()
        {
            using var context = CreateContext();
            var repository = new UnidadeRepository(context);

            var unidade = BuildUnidade("Excluir", "E01");
            context.Unidade.Add(unidade);
            await context.SaveChangesAsync();

            var retorno = await repository.DeletarAsync(unidade.Id);

            Assert.NotNull(retorno);
            var removida = await context.Unidade.FindAsync(unidade.Id);
            Assert.Null(removida);
        }
    }
}
