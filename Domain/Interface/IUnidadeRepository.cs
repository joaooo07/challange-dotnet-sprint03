using ChallangeDotnet.Domain.Entities;

namespace ChallangeDotnet.Domain.Interface
{
    public interface IUnidadeRepository
    {
        Task<PageResultModel<IEnumerable<UnidadeEntity>>> ObterTodosAsync(int Deslocamento = 0, int RegistrosRetornado = 3);
        Task<UnidadeEntity?> ObterUmAsync(int Id);
        Task<UnidadeEntity?> AdicionarAsync(UnidadeEntity entity);
        Task<UnidadeEntity?> EditarAsync(int Id, UnidadeEntity entity);
        Task<UnidadeEntity?> DeletarAsync(int Id);

    }
}
