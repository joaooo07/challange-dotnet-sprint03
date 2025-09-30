using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Domain.Entities;

namespace ChallangeDotnet.Application.Interface
{
    public interface IMotoUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<MotoEntity>>>> ObterTodasMotosAsync(int Deslocamento = 0, int RegistrosRetornado = 3);
        Task<OperationResult<MotoEntity?>> ObterUmaMotoAsync(int Id);
        Task<OperationResult<MotoEntity?>> AdicionarMotoAsync(MotoDto entity);
        Task<OperationResult<MotoEntity?>> EditarMotoAsync(int Id, MotoDto entity);
        Task<OperationResult<MotoEntity?>> DeletarMotoAsync(int Id);
    }
}
