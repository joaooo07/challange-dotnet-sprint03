using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Application.Interface;
using ChallangeDotnet.Application.Mapper;
using ChallangeDotnet.Domain.Entities;
using ChallangeDotnet.Domain.Interface;

using System.Net;

namespace ChallangeDotnet.Application.UseCase
{
    public class MotoUseCase : IMotoUseCase
    {
        private readonly IMotoRepository _repo;

        public MotoUseCase(IMotoRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<MotoEntity?>> AdicionarMotoAsync(MotoDto entity)
        {
            try
            {
                var result = await _repo.AdicionarAsync(entity.ToMotoEntity());
                return OperationResult<MotoEntity?>.Success(result);
            }
            catch
            {
                return OperationResult<MotoEntity?>.Failure("Não foi possível salvar a moto", (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<OperationResult<MotoEntity?>> DeletarMotoAsync(int Id)
        {
            try
            {
                var result = await _repo.DeletarAsync(Id);
                if (result is null)
                    return OperationResult<MotoEntity?>.Failure("Moto não foi encontrada", (int)HttpStatusCode.NotFound);

                return OperationResult<MotoEntity?>.Success(result);
            }
            catch
            {
                return OperationResult<MotoEntity?>.Failure("Não foi possível deletar a moto", (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<OperationResult<MotoEntity?>> EditarMotoAsync(int Id, MotoDto entity)
        {
            try
            {
                var result = await _repo.EditarAsync(Id, entity.ToMotoEntity());
                if (result is null)
                    return OperationResult<MotoEntity?>.Failure("Moto não foi encontrada", (int)HttpStatusCode.NotFound);

                return OperationResult<MotoEntity?>.Success(result);
            }
            catch
            {
                return OperationResult<MotoEntity?>.Failure("Não foi possível editar a moto", (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<MotoEntity>>>> ObterTodasMotosAsync(int Deslocamento = 0, int RegistrosRetornado = 3)
        {
            var result = await _repo.ObterTodosAsync(Deslocamento, RegistrosRetornado);

            if (result.Data is null || !result.Data.Any())
                return OperationResult<PageResultModel<IEnumerable<MotoEntity>>>.Failure("Não foi encontrado dados", (int)HttpStatusCode.NoContent);

            return OperationResult<PageResultModel<IEnumerable<MotoEntity>>>.Success(result);
        }

        public async Task<OperationResult<MotoEntity?>> ObterUmaMotoAsync(int Id)
        {
            var result = await _repo.ObterUmAsync(Id);

            if (result is null)
                return OperationResult<MotoEntity?>.Failure("Moto não encontrada", (int)HttpStatusCode.NotFound);

            return OperationResult<MotoEntity?>.Success(result);
        }
    }
}
