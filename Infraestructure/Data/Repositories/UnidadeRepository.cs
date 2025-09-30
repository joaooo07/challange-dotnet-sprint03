using ChallangeDotnet.Domain.Entities;
using ChallangeDotnet.Domain.Interface;
using ChallangeDotnet.Infraestructure.Data.AppData;
using Microsoft.EntityFrameworkCore;

namespace ChallangeDotnet.Infrastructure.Data.Repositories
{
    public class UnidadeRepository : IUnidadeRepository
    {
        private readonly ApplicationContext _context;

        public UnidadeRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<UnidadeEntity?> AdicionarAsync(UnidadeEntity entity)
        {
            _context.Unidade.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<UnidadeEntity?> DeletarAsync(int Id)
        {
            var result = await _context.Unidade.FindAsync(Id);

            if (result is not null)
            {
                _context.Remove(result);
                _context.SaveChanges();
                return result;
            }

            return null;
        }

        public async Task<UnidadeEntity?> EditarAsync(int Id, UnidadeEntity entity)
        {
            var result = await _context
                .Unidade
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (result is not null)
            {
                result.Nome = entity.Nome;
                result.Codigo = entity.Codigo;
                result.Ativa = entity.Ativa;
                result.Observacao = entity.Observacao;

                _context.Update(result);
                _context.SaveChanges();
                return result;
            }

            return null;
        }

        public async Task<PageResultModel<IEnumerable<UnidadeEntity>>> ObterTodosAsync(int Deslocamento = 0, int RegistrosRetornado = 3)
        {
            var totalRegistros = await _context.Unidade.CountAsync();

            var result = await _context
                .Unidade
                .OrderBy(x => x.Id)
                .Skip(Deslocamento)
                .Take(RegistrosRetornado)
                .ToListAsync();

            return new PageResultModel<IEnumerable<UnidadeEntity>>
            {
                Data = result,
                Deslocamento = Deslocamento,
                RegistrosRetornado = RegistrosRetornado,
                TotalRegistros = totalRegistros
            };
        }

        public async Task<UnidadeEntity?> ObterUmAsync(int Id)
        {
            var result = await _context
                .Unidade
                .FirstOrDefaultAsync(x => x.Id == Id);

            return result;
        }
    }
}
