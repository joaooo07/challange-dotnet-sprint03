using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Domain.Entities;

namespace ChallangeDotnet.Application.Interface
{
    public interface IUsuarioUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<UsuarioEntity>>>> ObterTodosUsuariosAsync(int Deslocamento = 0, int RegistrosRetornado = 3);
        Task<OperationResult<UsuarioEntity?>> ObterUmUsuarioAsync(int Id);
        Task<OperationResult<UsuarioEntity?>> AdicionarUsuarioAsync(UsuarioDto entity);
        Task<OperationResult<UsuarioEntity?>> EditarUsuarioAsync(int Id, UsuarioDto entity);
        Task<OperationResult<UsuarioEntity?>> DeletarUsuarioAsync(int Id);
        Task<UsuarioEntity?> ObterPorEmailAsync(string email);

    }
}
