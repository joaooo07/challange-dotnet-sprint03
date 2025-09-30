using ChallangeDotnet.Domain.Entities;

namespace ChallangeDotnet.Domain.Interface
{
    public interface IUsuarioRepository
    {
        Task<PageResultModel<IEnumerable<UsuarioEntity>>> ObterTodosAsync(int Deslocamento = 0, int RegistrosRetornado = 3);
        Task<UsuarioEntity?> ObterUmAsync(int Id);
        Task<UsuarioEntity?> AdicionarAsync(UsuarioEntity entity);
        Task<UsuarioEntity?> EditarAsync(int Id, UsuarioEntity entity);
        Task<UsuarioEntity?> DeletarAsync(int Id);
        Task<UsuarioEntity?> ObterPorEmailAsync(string email);



    }
}
