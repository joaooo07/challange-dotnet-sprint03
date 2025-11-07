using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Domain.Entities;

namespace ChallangeDotnet.Application.Mapper
{
    public static class UnidadeMapper
    {
        public static UnidadeEntity ToUnidadeEntity(this UnidadeDto obj)
        {
            return new UnidadeEntity
            {
                Codigo = obj.Codigo,
                Nome = obj.Nome,
                Ativa = obj.Ativa,
                Observacao = obj.Observacao
            };
        }
    }
}
