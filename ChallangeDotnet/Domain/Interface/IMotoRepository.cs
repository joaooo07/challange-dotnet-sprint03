using ChallangeDotnet.Domain.Entities;

namespace ChallangeDotnet.Domain.Interface
{
    public interface IMotoRepository
    {
        Task<PageResultModel<IEnumerable<MotoEntity>>> ObterTodosAsync(int Deslocamento = 0, int RegistrosRetornado = 3);
        Task<MotoEntity?> ObterUmAsync(int Id);
        Task<MotoEntity?> AdicionarAsync(MotoEntity entity);
        Task<MotoEntity?> EditarAsync(int Id, MotoEntity entity);
        Task<MotoEntity?> DeletarAsync(int Id);
    }
}
