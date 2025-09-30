using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace ChallangeDotnet.Presentation.Examples
{
    // Exemplo de request (POST/PUT)
    public class MotoRequestExample : IExamplesProvider<MotoDto>
    {
        public MotoDto GetExamples()
        {
            return new MotoDto(
                Modelo: "CB 500F",
                Marca: "Honda",
                Ano: 2022
            );
        }
    }

    // Exemplo de response (GET)
    public class MotoResponseExample : IExamplesProvider<MotoEntity>
    {
        public MotoEntity GetExamples()
        {
            return new MotoEntity
            {
                Id = 1,
                Modelo = "MT-07",
                Marca = "Yamaha",
                Ano = 2023
            };
        }
    }
}
