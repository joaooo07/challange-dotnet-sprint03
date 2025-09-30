using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Domain.Entities;

namespace ChallangeDotnet.Application.Interface
{
    public interface IUnidadeUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<UnidadeEntity>>>> ObterTodasUnidadesAsync(int Deslocamento = 0, int RegistrosRetornado = 3);
        Task<OperationResult<UnidadeEntity?>> ObterUmaUnidadeAsync(int Id);
        Task<OperationResult<UnidadeEntity?>> AdicionarUnidadeAsync(UnidadeDto entity);
        Task<OperationResult<UnidadeEntity?>> EditarUnidadeAsync(int Id, UnidadeDto entity);
        Task<OperationResult<UnidadeEntity?>> DeletarUnidadeAsync(int Id);
    }
}
