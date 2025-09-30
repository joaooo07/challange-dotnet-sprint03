using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Application.Interface;
using ChallangeDotnet.Application.Mapper;
using ChallangeDotnet.Domain.Entities;
using ChallangeDotnet.Domain.Interface;
using System.Net;

namespace ChallangeDotnet.Application.UseCase
{
    public class UnidadeUseCase : IUnidadeUseCase
    {
        private readonly IUnidadeRepository _repo;

        public UnidadeUseCase(IUnidadeRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<UnidadeEntity?>> AdicionarUnidadeAsync(UnidadeDto entity)
        {
            try
            {
                var result = await _repo.AdicionarAsync(entity.ToUnidadeEntity());
                return OperationResult<UnidadeEntity?>.Success(result);
            }
            catch
            {
                return OperationResult<UnidadeEntity?>.Failure("Não foi possível salvar a unidade", (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<OperationResult<UnidadeEntity?>> DeletarUnidadeAsync(int Id)
        {
            try
            {
                var result = await _repo.DeletarAsync(Id);
                if (result is null)
                    return OperationResult<UnidadeEntity?>.Failure("Unidade não foi encontrada", (int)HttpStatusCode.NotFound);

                return OperationResult<UnidadeEntity?>.Success(result);
            }
            catch
            {
                return OperationResult<UnidadeEntity?>.Failure("Não foi possível deletar a unidade", (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<OperationResult<UnidadeEntity?>> EditarUnidadeAsync(int Id, UnidadeDto entity)
        {
            try
            {
                var result = await _repo.EditarAsync(Id, entity.ToUnidadeEntity());
                if (result is null)
                    return OperationResult<UnidadeEntity?>.Failure("Unidade não foi encontrada", (int)HttpStatusCode.NotFound);

                return OperationResult<UnidadeEntity?>.Success(result);
            }
            catch
            {
                return OperationResult<UnidadeEntity?>.Failure("Não foi possível editar a unidade", (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<UnidadeEntity>>>> ObterTodasUnidadesAsync(int Deslocamento = 0, int RegistrosRetornado = 3)
        {
            var result = await _repo.ObterTodosAsync(Deslocamento, RegistrosRetornado);

            if (result.Data is null || !result.Data.Any())
                return OperationResult<PageResultModel<IEnumerable<UnidadeEntity>>>.Failure("Não foi encontrado dados", (int)HttpStatusCode.NoContent);

            return OperationResult<PageResultModel<IEnumerable<UnidadeEntity>>>.Success(result);
        }

        public async Task<OperationResult<UnidadeEntity?>> ObterUmaUnidadeAsync(int Id)
        {
            var result = await _repo.ObterUmAsync(Id);

            if (result is null)
                return OperationResult<UnidadeEntity?>.Failure("Unidade não encontrada", (int)HttpStatusCode.NotFound);

            return OperationResult<UnidadeEntity?>.Success(result);
        }
    }
}
