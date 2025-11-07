using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Application.Interface;
using ChallangeDotnet.Application.Mapper;
using ChallangeDotnet.Domain.Entities;
using ChallangeDotnet.Domain.Interface;

using System.Net;

namespace ChallangeDotnet.Application.UseCase
{
    public class UsuarioUseCase : IUsuarioUseCase
    {
        private readonly IUsuarioRepository _repo;

        public UsuarioUseCase(IUsuarioRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<UsuarioEntity?>> AdicionarUsuarioAsync(UsuarioDto entity)
        {
            try
            {
                var result = await _repo.AdicionarAsync(entity.ToUsuarioEntity());
                return OperationResult<UsuarioEntity?>.Success(result);
            }
            catch
            {
                return OperationResult<UsuarioEntity?>.Failure("Não foi possível salvar o usuário", (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<OperationResult<UsuarioEntity?>> DeletarUsuarioAsync(int Id)
        {
            try
            {
                var result = await _repo.DeletarAsync(Id);
                if (result is null)
                    return OperationResult<UsuarioEntity?>.Failure("Usuário não foi encontrado", (int)HttpStatusCode.NotFound);

                return OperationResult<UsuarioEntity?>.Success(result);
            }
            catch
            {
                return OperationResult<UsuarioEntity?>.Failure("Não foi possível deletar o usuário", (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<OperationResult<UsuarioEntity?>> EditarUsuarioAsync(int Id, UsuarioDto entity)
        {
            try
            {
                var result = await _repo.EditarAsync(Id, entity.ToUsuarioEntity());
                if (result is null)
                    return OperationResult<UsuarioEntity?>.Failure("Usuário não foi encontrado", (int)HttpStatusCode.NotFound);

                return OperationResult<UsuarioEntity?>.Success(result);
            }
            catch
            {
                return OperationResult<UsuarioEntity?>.Failure("Não foi possível editar o usuário", (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<UsuarioEntity>>>> ObterTodosUsuariosAsync(int Deslocamento = 0, int RegistrosRetornado = 3)
        {
            var result = await _repo.ObterTodosAsync(Deslocamento, RegistrosRetornado);

            if (result.Data is null || !result.Data.Any())
                return OperationResult<PageResultModel<IEnumerable<UsuarioEntity>>>.Failure("Não foi encontrado dados", (int)HttpStatusCode.NoContent);

            return OperationResult<PageResultModel<IEnumerable<UsuarioEntity>>>.Success(result);
        }

        public async Task<OperationResult<UsuarioEntity?>> ObterUmUsuarioAsync(int Id)
        {
            var result = await _repo.ObterUmAsync(Id);

            if (result is null)
                return OperationResult<UsuarioEntity?>.Failure("Usuário não encontrado", (int)HttpStatusCode.NotFound);

            return OperationResult<UsuarioEntity?>.Success(result);
        }

        public async Task<UsuarioEntity?> ObterPorEmailAsync(string email)
        {
            return await _repo  .ObterPorEmailAsync(email);
        }

    }
}
