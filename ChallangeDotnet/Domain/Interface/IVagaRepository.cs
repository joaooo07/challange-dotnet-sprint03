using ChallangeDotnet.Domain.Entities;

namespace ChallangeDotnet.Domain.Interface
{
    public interface IVagaRepository
    {
        Task<PageResultModel<IEnumerable<VagaEntity>>> ObterTodosAsync(int Deslocamento = 0, int RegistrosRetornado = 3);
        Task<VagaEntity?> ObterUmAsync(int Id);
        Task<VagaEntity?> AdicionarAsync(VagaEntity entity);
        Task<VagaEntity?> EditarAsync(int Id, VagaEntity entity);
        Task<VagaEntity?> DeletarAsync(int Id);
    }
}
