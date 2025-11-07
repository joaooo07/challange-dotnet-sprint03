using ChallangeDotnet.Domain.Entities;
using ChallangeDotnet.Domain.Interface;
using ChallangeDotnet.Infraestructure.Data.AppData;
using Microsoft.EntityFrameworkCore;

namespace ChallangeDotnet.Infrastructure.Data.Repositories
{
    public class MotoRepository : IMotoRepository
    {
        private readonly ApplicationContext _context;

        public MotoRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<MotoEntity?> AdicionarAsync(MotoEntity entity)
        {
            _context.Moto.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<MotoEntity?> DeletarAsync(int Id)
        {
            var result = await _context.Moto.FindAsync(Id);

            if (result is not null)
            {
                _context.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }

            return null;
        }

        public async Task<MotoEntity?> EditarAsync(int Id, MotoEntity entity)
        {
            var result = await _context
                .Moto
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (result is not null)
            {
                result.Modelo = entity.Modelo;
                result.Marca = entity.Marca;
                result.Ano = entity.Ano;

                _context.Update(result);
                await _context.SaveChangesAsync();
                return result;
            }

            return null;
        }

        public async Task<PageResultModel<IEnumerable<MotoEntity>>> ObterTodosAsync(int Deslocamento = 0, int RegistrosRetornado = 3)
        {
            var totalRegistros = await _context.Moto.CountAsync();

            var result = await _context
                .Moto
                .OrderBy(x => x.Id)
                .Skip(Deslocamento)
                .Take(RegistrosRetornado)
                .ToListAsync();

            return new PageResultModel<IEnumerable<MotoEntity>>
            {
                Data = result,
                Deslocamento = Deslocamento,
                RegistrosRetornado = RegistrosRetornado,
                TotalRegistros = totalRegistros
            };
        }

        public async Task<MotoEntity?> ObterUmAsync(int Id)
        {
            var result = await _context
                .Moto
                .FirstOrDefaultAsync(x => x.Id == Id);

            return result;
        }
    }
}
