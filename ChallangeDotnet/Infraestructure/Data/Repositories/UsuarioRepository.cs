using ChallangeDotnet.Domain.Entities;
using ChallangeDotnet.Domain.Interface;
using ChallangeDotnet.Infraestructure.Data.AppData;
using Microsoft.EntityFrameworkCore;

namespace ChallangeDotnet.Infrastructure.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationContext _context;

        public UsuarioRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<UsuarioEntity?> AdicionarAsync(UsuarioEntity entity)
        {
            _context.Usuario.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<UsuarioEntity?> DeletarAsync(int Id)
        {
            var result = await _context.Usuario.FindAsync(Id);

            if (result is not null)
            {
                _context.Remove(result);
                _context.SaveChanges();
                return result;
            }

            return null;
        }

        public async Task<UsuarioEntity?> EditarAsync(int Id, UsuarioEntity entity)
        {
            var result = await _context
                .Usuario
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (result is not null)
            {
                result.Nome = entity.Nome;
                result.Email = entity.Email;
                result.Senha = entity.Senha;
                result.Ativo = entity.Ativo;

                _context.Update(result);
                _context.SaveChanges();
                return result;
            }

            return null;
        }

        public async Task<PageResultModel<IEnumerable<UsuarioEntity>>> ObterTodosAsync(int Deslocamento = 0, int RegistrosRetornado = 3)
        {
            var totalRegistros = await _context.Usuario.CountAsync();

            var result = await _context
                .Usuario
                .OrderBy(x => x.Id)
                .Skip(Deslocamento)
                .Take(RegistrosRetornado)
                .ToListAsync();

            return new PageResultModel<IEnumerable<UsuarioEntity>>
            {
                Data = result,
                Deslocamento = Deslocamento,
                RegistrosRetornado = RegistrosRetornado,
                TotalRegistros = totalRegistros
            };
        }

        public async Task<UsuarioEntity?> ObterUmAsync(int Id)
        {
            var result = await _context
                .Usuario
                .FirstOrDefaultAsync(x => x.Id == Id);

            return result;
        }

        public async Task<UsuarioEntity?> ObterPorEmailAsync(string email)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Email == email);
        }

    }
}
