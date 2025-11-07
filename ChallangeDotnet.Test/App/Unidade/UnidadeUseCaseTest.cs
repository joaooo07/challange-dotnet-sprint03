using ChallangeDotnet.Application.UseCase;
using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Domain.Entities;
using ChallangeDotnet.Domain.Interface;
using Moq;

namespace ChallangeDotnet.Tests.App.Unidade
{
    public class UnidadeUseCaseTest
    {
        private readonly Mock<IUnidadeRepository> _mockRepo;
        private readonly UnidadeUseCase _useCase;

        public UnidadeUseCaseTest()
        {
            _mockRepo = new Mock<IUnidadeRepository>();
            _useCase = new UnidadeUseCase(_mockRepo.Object);
        }

        private UnidadeDto BuildDto(
            string? nome = null,
            string? codigo = null,
            string? observacao = null,
            bool ativa = true)
        {
            return new UnidadeDto(
                codigo ?? "U01",
                nome ?? "Unidade Teste",
                ativa,
                observacao ?? "Unidade de teste"
            );


        }

        [Fact(DisplayName = "ObterTodosAsync - Retorna lista de unidades")]
        [Trait("UseCase", "Unidade")]
        public async Task ObterTodos_DeveRetornarLista()
        {
            var lista = new List<UnidadeEntity>
            {
                new() { Id = 1, Nome = "Unidade A", Codigo = "A01", Ativa = true },
                new() { Id = 2, Nome = "Unidade B", Codigo = "B02", Ativa = false },
            };

            var retorno = new PageResultModel<IEnumerable<UnidadeEntity>>
            {
                Data = lista,
                Deslocamento = 0,
                RegistrosRetornado = 10,
                TotalRegistros = 30
            };


            _mockRepo.Setup(r => r.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>()))
                     .ReturnsAsync(retorno);

            var result = await _useCase.ObterTodasUnidadesAsync();

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Data.Count());
        }

        [Fact(DisplayName = "ObterUmAsync - Retorna unidade por ID")]
        [Trait("UseCase", "Unidade")]
        public async Task ObterUm_DeveRetornarUnidade()
        {
            var unidade = new UnidadeEntity { Id = 1, Nome = "Unidade X", Codigo = "X01", Ativa = true };

            _mockRepo.Setup(r => r.ObterUmAsync(1)).ReturnsAsync(unidade);

            var result = await _useCase.ObterUmaUnidadeAsync(1);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("Unidade X", result.Value.Nome);
        }

        [Fact(DisplayName = "AdicionarAsync - Adiciona unidade com sucesso")]
        [Trait("UseCase", "Unidade")]
        public async Task Adicionar_DeveAdicionarUnidade()
        {
            var dto = BuildDto();
            var entity = new UnidadeEntity { Id = 1, Nome = dto.Nome, Codigo = dto.Codigo, Ativa = dto.Ativa };

            _mockRepo.Setup(r => r.AdicionarAsync(It.IsAny<UnidadeEntity>())).ReturnsAsync(entity);

            var result = await _useCase.AdicionarUnidadeAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(dto.Nome, result.Value.Nome);
        }

        [Fact(DisplayName = "EditarAsync - Atualiza unidade existente")]
        [Trait("UseCase", "Unidade")]
        public async Task Editar_DeveAtualizarUnidade()
        {
            var dto = BuildDto("Nova Unidade", "U02");
            var entity = new UnidadeEntity { Id = 1, Nome = "Antiga", Codigo = "U01" };

            _mockRepo.Setup(r => r.EditarAsync(1, It.IsAny<UnidadeEntity>())).ReturnsAsync(entity);

            var result = await _useCase.EditarUnidadeAsync(1, dto);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
        }

        [Fact(DisplayName = "DeletarAsync - Remove unidade existente")]
        [Trait("UseCase", "Unidade")]
        public async Task Deletar_DeveRemoverUnidade()
        {
            var entity = new UnidadeEntity { Id = 1, Nome = "Excluir", Codigo = "E01" };

            _mockRepo.Setup(r => r.DeletarAsync(1)).ReturnsAsync(entity);

            var result = await _useCase.DeletarUnidadeAsync(1);

            Assert.True(result.IsSuccess);
            Assert.Equal("Excluir", result.Value.Nome);
        }
    }
}
