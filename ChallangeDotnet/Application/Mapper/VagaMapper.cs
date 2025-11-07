using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Domain.Entities;

namespace ChallangeDotnet.Application.Mapper
{
    public static class VagaMapper
    {
        public static VagaEntity ToVagaEntity(this VagaDto obj)
        {
            return new VagaEntity
            {
                Codigo = obj.Codigo,
                Coberta = obj.Coberta,
                Ocupada = obj.Ocupada,
                Observacao = obj.Observacao
            };
        }
    }
}
