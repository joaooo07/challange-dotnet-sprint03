using ChallangeDotnet.Domain.Entities;
using ChallangeDotnet.Domain.Interface;
using ChallangeDotnet.Infraestructure.Data.AppData;

using Microsoft.EntityFrameworkCore;

namespace ChallangeDotnet.Infrastructure.Data.Repositories
{
    public class VagaRepository : IVagaRepository
    {
        private readonly ApplicationContext _context;

        public VagaRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<VagaEntity?> AdicionarAsync(VagaEntity entity)
        {
            _context.Vaga.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<VagaEntity?> DeletarAsync(int Id)
        {
            var result = await _context.Vaga.FindAsync(Id);

            if (result is not null)
            {
                _context.Remove(result);
                _context.SaveChanges();
                return result;
            }

            return null;
        }

        public async Task<VagaEntity?> EditarAsync(int Id, VagaEntity entity)
        {
            var result = await _context
                .Vaga
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (result is not null)
            {
                result.Codigo = entity.Codigo;
                result.Coberta = entity.Coberta;
                result.Ocupada = entity.Ocupada;
                result.Observacao = entity.Observacao;

                _context.Update(result);
                _context.SaveChanges();
                return result;
            }

            return null;
        }

        public async Task<PageResultModel<IEnumerable<VagaEntity>>> ObterTodosAsync(int Deslocamento = 0, int RegistrosRetornado = 3)
        {
            var totalRegistros = await _context.Vaga.CountAsync();

            var result = await _context
                .Vaga
                .OrderBy(x => x.Id)
                .Skip(Deslocamento)
                .Take(RegistrosRetornado)
                .ToListAsync();

            return new PageResultModel<IEnumerable<VagaEntity>>
            {
                Data = result,
                Deslocamento = Deslocamento,
                RegistrosRetornado = RegistrosRetornado,
                TotalRegistros = totalRegistros
            };
        }

        public async Task<VagaEntity?> ObterUmAsync(int Id)
        {
            var result = await _context
                .Vaga
                .FirstOrDefaultAsync(x => x.Id == Id);

            return result;
        }
    }
}
