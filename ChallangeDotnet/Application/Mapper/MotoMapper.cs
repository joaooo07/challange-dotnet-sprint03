using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Domain.Entities;

namespace ChallangeDotnet.Application.Mapper
{
    public static class MotoMapper
    {
        public static MotoEntity ToMotoEntity(this MotoDto obj)
        {
            return new MotoEntity
            {
                Modelo = obj.Modelo,
                Marca = obj.Marca,
                Ano = obj.Ano
            };
        }
    }
}
