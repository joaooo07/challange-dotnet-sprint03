using Microsoft.EntityFrameworkCore;
using ChallangeDotnet.Domain.Entities;

namespace ChallangeDotnet.Infraestructure.Data.AppData
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<UsuarioEntity> Usuario { get; set; }
        public DbSet<UnidadeEntity> Unidade { get; set; }
        public DbSet<VagaEntity> Vaga { get; set; }
        public DbSet<MotoEntity> Moto { get; set; }

    }
}

