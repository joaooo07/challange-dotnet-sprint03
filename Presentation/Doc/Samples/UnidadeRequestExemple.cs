using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace ChallangeDotnet.Presentation.Examples
{
    // Exemplo de request (POST/PUT)
    public class UnidadeRequestExample : IExamplesProvider<UnidadeDto>
    {
        public UnidadeDto GetExamples()
        {
            return new UnidadeDto(
                Codigo: "UN001",
                Nome: "Unidade Central",
                Ativa: true,
                Observacao: "Aberta em horário comercial"
            );
        }

        public class UnidadeResponseExample : IExamplesProvider<UnidadeEntity>
        {
            public UnidadeEntity GetExamples()
            {
                return new UnidadeEntity
                {
                    Id = 1,
                    Codigo = "UN001",
                    Nome = "Unidade Central",
                    Ativa = true,
                    Observacao = "Aberta em horário comercial"
                };
            }
        }
    }
}
}
