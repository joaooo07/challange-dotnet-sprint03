using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Domain.Entities;


namespace ChallangeDotnet.Application.Interface
{
    public interface IVagaUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<VagaEntity>>>> ObterTodasVagasAsync(int Deslocamento = 0, int RegistrosRetornado = 3);
        Task<OperationResult<VagaEntity?>> ObterUmaVagaAsync(int Id);
        Task<OperationResult<VagaEntity?>> AdicionarVagaAsync(VagaDto entity);
        Task<OperationResult<VagaEntity?>> EditarVagaAsync(int Id, VagaDto entity);
        Task<OperationResult<VagaEntity?>> DeletarVagaAsync(int Id);
    }
}
