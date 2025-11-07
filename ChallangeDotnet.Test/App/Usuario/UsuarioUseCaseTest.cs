using Moq;
using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Application.UseCase;
using ChallangeDotnet.Domain.Entities;
using ChallangeDotnet.Domain.Interface;
using System.Net;

namespace ChallangeDotnet.Tests.App.Usuario
{
    public class UsuarioUseCaseTest
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepository;
        private readonly UsuarioUseCase _usuarioUseCase;

        public UsuarioUseCaseTest()
        {
            _usuarioRepository = new Mock<IUsuarioRepository>();
            _usuarioUseCase = new UsuarioUseCase(_usuarioRepository.Object);
        }

        [Fact(DisplayName = "ObterTodosUsuariosAsync - Retorna usuários com sucesso")]
        [Trait("UseCase", "Usuario")]
        public async Task ObterTodos_DeveRetornarUsuarios()
        {
            var usuarios = new List<UsuarioEntity>
            {
                new UsuarioEntity { Id = 1, Nome = "João", Email = "joao@teste.com", Senha = "123", Ativo = true },
                new UsuarioEntity { Id = 2, Nome = "Maria", Email = "maria@teste.com", Senha = "abc", Ativo = true }
            };

            var pageResult = new PageResultModel<IEnumerable<UsuarioEntity>>
            {
                Data = usuarios,
                Deslocamento = 0,
                RegistrosRetornado = 2,
                TotalRegistros = 2
            };

            _usuarioRepository
                .Setup(r => r.ObterTodosAsync(0, 3))
                .ReturnsAsync(pageResult);

            var resultado = await _usuarioUseCase.ObterTodosUsuariosAsync(0, 3);

            Assert.True(resultado.IsSuccess);
            Assert.Equal(200, resultado.StatusCode);
            Assert.Equal(2, resultado.Value!.Data.Count());
        }

        [Fact(DisplayName = "ObterTodosUsuariosAsync - Retorna NoContent quando não há usuários")]
        [Trait("UseCase", "Usuario")]
        public async Task ObterTodos_DeveRetornarNoContent()
        {
            var pageResult = new PageResultModel<IEnumerable<UsuarioEntity>>
            {
                Data = new List<UsuarioEntity>(),
                TotalRegistros = 0
            };

            _usuarioRepository
                .Setup(r => r.ObterTodosAsync(0, 3))
                .ReturnsAsync(pageResult);

            var resultado = await _usuarioUseCase.ObterTodosUsuariosAsync(0, 3);

            Assert.False(resultado.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NoContent, resultado.StatusCode);
        }

        [Fact(DisplayName = "ObterUmUsuarioAsync - Retorna um usuário existente")]
        [Trait("UseCase", "Usuario")]
        public async Task ObterUm_DeveRetornarUsuario()
        {
            var usuario = new UsuarioEntity { Id = 1, Nome = "João", Email = "joao@teste.com", Senha = "123" };

            _usuarioRepository
                .Setup(r => r.ObterUmAsync(1))
                .ReturnsAsync(usuario);

            var resultado = await _usuarioUseCase.ObterUmUsuarioAsync(1);

            Assert.True(resultado.IsSuccess);
            Assert.Equal(200, resultado.StatusCode);
            Assert.Equal("João", resultado.Value!.Nome);
        }

        [Fact(DisplayName = "ObterUmUsuarioAsync - Retorna NotFound quando não existe")]
        [Trait("UseCase", "Usuario")]
        public async Task ObterUm_DeveRetornarNotFound()
        {
            _usuarioRepository
                .Setup(r => r.ObterUmAsync(99))
                .ReturnsAsync((UsuarioEntity?)null);

            var resultado = await _usuarioUseCase.ObterUmUsuarioAsync(99);

            Assert.False(resultado.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, resultado.StatusCode);
        }

        [Fact(DisplayName = "AdicionarUsuarioAsync - Adiciona um usuário com sucesso")]
        [Trait("UseCase", "Usuario")]
        public async Task Adicionar_DeveRetornarSucesso()
        {
            var dto = new UsuarioDto("João", "joao@teste.com", "123", true);
            var entity = new UsuarioEntity { Id = 1, Nome = "João", Email = "joao@teste.com", Senha = "123", Ativo = true };

            _usuarioRepository
                .Setup(r => r.AdicionarAsync(It.IsAny<UsuarioEntity>()))
                .ReturnsAsync(entity);

            var resultado = await _usuarioUseCase.AdicionarUsuarioAsync(dto);

            Assert.True(resultado.IsSuccess);
            Assert.Equal(200, resultado.StatusCode);
            Assert.Equal("João", resultado.Value!.Nome);
        }

        [Fact(DisplayName = "EditarUsuarioAsync - Edita um usuário existente")]
        [Trait("UseCase", "Usuario")]
        public async Task Editar_DeveRetornarSucesso()
        {
            var dto = new UsuarioDto("João Atualizado", "joao@teste.com", "321", true);
            var entity = new UsuarioEntity { Id = 1, Nome = "João Atualizado", Email = "joao@teste.com", Senha = "321", Ativo = true };

            _usuarioRepository
                .Setup(r => r.EditarAsync(1, It.IsAny<UsuarioEntity>()))
                .ReturnsAsync(entity);

            var resultado = await _usuarioUseCase.EditarUsuarioAsync(1, dto);

            Assert.True(resultado.IsSuccess);
            Assert.Equal(200, resultado.StatusCode);
            Assert.Equal("João Atualizado", resultado.Value!.Nome);
        }

        [Fact(DisplayName = "EditarUsuarioAsync - Retorna NotFound se usuário não existir")]
        [Trait("UseCase", "Usuario")]
        public async Task Editar_DeveRetornarNotFound()
        {
            var dto = new UsuarioDto("Maria", "maria@teste.com", "321", true);

            _usuarioRepository
                .Setup(r => r.EditarAsync(99, It.IsAny<UsuarioEntity>()))
                .ReturnsAsync((UsuarioEntity?)null);

            var resultado = await _usuarioUseCase.EditarUsuarioAsync(99, dto);

            Assert.False(resultado.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, resultado.StatusCode);
        }

        [Fact(DisplayName = "DeletarUsuarioAsync - Deleta um usuário existente")]
        [Trait("UseCase", "Usuario")]
        public async Task Deletar_DeveRetornarSucesso()
        {
            var entity = new UsuarioEntity { Id = 1, Nome = "João", Email = "joao@teste.com" };

            _usuarioRepository
                .Setup(r => r.DeletarAsync(1))
                .ReturnsAsync(entity);

            var resultado = await _usuarioUseCase.DeletarUsuarioAsync(1);

            Assert.True(resultado.IsSuccess);
            Assert.Equal(200, resultado.StatusCode);
        }

        [Fact(DisplayName = "DeletarUsuarioAsync - Retorna NotFound quando usuário não existe")]
        [Trait("UseCase", "Usuario")]
        public async Task Deletar_DeveRetornarNotFound()
        {
            _usuarioRepository
                .Setup(r => r.DeletarAsync(99))
                .ReturnsAsync((UsuarioEntity?)null);

            var resultado = await _usuarioUseCase.DeletarUsuarioAsync(99);

            Assert.False(resultado.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, resultado.StatusCode);
        }
    }
}
