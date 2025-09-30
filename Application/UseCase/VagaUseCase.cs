using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Application.Interface;
using ChallangeDotnet.Application.Mapper;
using ChallangeDotnet.Domain.Entities;
using ChallangeDotnet.Domain.Interface;

using System.Net;

namespace ChallangeDotnet.Application.UseCase
{
    public class VagaUseCase : IVagaUseCase
    {
        private readonly IVagaRepository _repo;

        public VagaUseCase(IVagaRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<VagaEntity?>> AdicionarVagaAsync(VagaDto entity)
        {
            try
            {
                var result = await _repo.AdicionarAsync(entity.ToVagaEntity());
                return OperationResult<VagaEntity?>.Success(result);
            }
            catch
            {
                return OperationResult<VagaEntity?>.Failure("Não foi possível salvar a vaga", (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<OperationResult<VagaEntity?>> DeletarVagaAsync(int Id)
        {
            try
            {
                var result = await _repo.DeletarAsync(Id);
                if (result is null)
                    return OperationResult<VagaEntity?>.Failure("Vaga não foi encontrada", (int)HttpStatusCode.NotFound);

                return OperationResult<VagaEntity?>.Success(result);
            }
            catch
            {
                return OperationResult<VagaEntity?>.Failure("Não foi possível deletar a vaga", (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<OperationResult<VagaEntity?>> EditarVagaAsync(int Id, VagaDto entity)
        {
            try
            {
                var result = await _repo.EditarAsync(Id, entity.ToVagaEntity());
                if (result is null)
                    return OperationResult<VagaEntity?>.Failure("Vaga não foi encontrada", (int)HttpStatusCode.NotFound);

                return OperationResult<VagaEntity?>.Success(result);
            }
            catch
            {
                return OperationResult<VagaEntity?>.Failure("Não foi possível editar a vaga", (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<VagaEntity>>>> ObterTodasVagasAsync(int Deslocamento = 0, int RegistrosRetornado = 3)
        {
            var result = await _repo.ObterTodosAsync(Deslocamento, RegistrosRetornado);

            if (result.Data is null || !result.Data.Any())
                return OperationResult<PageResultModel<IEnumerable<VagaEntity>>>.Failure("Não foi encontrado dados", (int)HttpStatusCode.NoContent);

            return OperationResult<PageResultModel<IEnumerable<VagaEntity>>>.Success(result);
        }

        public async Task<OperationResult<VagaEntity?>> ObterUmaVagaAsync(int Id)
        {
            var result = await _repo.ObterUmAsync(Id);

            if (result is null)
                return OperationResult<VagaEntity?>.Failure("Vaga não encontrada", (int)HttpStatusCode.NotFound);

            return OperationResult<VagaEntity?>.Success(result);
        }
    }
}
