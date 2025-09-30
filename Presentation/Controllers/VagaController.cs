using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Application.Interface;
using ChallangeDotnet.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
// using ChallangeDotnet.Doc.Samples; 

namespace ChallangeDotnet.Presentation.Controllers
{
    [Route("api/v1/vaga")]
    [ApiController]
    public class VagaController : ControllerBase
    {
        private readonly IVagaUseCase _vagaUseCase;

        public VagaController(IVagaUseCase vagaUseCase)
        {
            _vagaUseCase = vagaUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
           Summary = "Lista vagas",
           Description = "Retorna a lista paginada de vagas cadastradas."
        )]
        [SwaggerResponse(statusCode: 200, description: "Lista retornada com sucesso", type: typeof(IEnumerable<VagaEntity>))]
        [SwaggerResponse(statusCode: 201, description: "Não possui dados para a vaga")]
        // [SwaggerResponseExample(statusCode: 200, typeof(VagaResponseListSample))] // se tiver samples
        [EnableRateLimiting("rateLimitePolicy")]
        public async Task<IActionResult> Get(int Deslocamento = 0, int RegistrosRetornado = 3)
        {
            var result = await _vagaUseCase.ObterTodasVagasAsync(Deslocamento, RegistrosRetornado);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            var hateaos = new
            {
                data = result.Value?.Data.Select(v => new
                {
                    v.Id,
                    v.Codigo,
                    v.Coberta,
                    v.Ocupada,
                    v.Observacao,
                    links = new
                    {
                        self = Url.Action(nameof(Get), "Vaga", new { id = v.Id }, Request.Scheme),
                        put = Url.Action(nameof(Put), "Vaga", new { id = v.Id }, Request.Scheme),
                        delete = Url.Action(nameof(Delete), "Vaga", new { id = v.Id }, Request.Scheme),
                    }
                }),
                links = new
                {
                    self = Url.Action(nameof(Get), "Vaga", null, Request.Scheme),
                    create = Url.Action(nameof(Post), "Vaga", null, Request.Scheme),
                },
                pagina = new
                {
                    result.Value?.Deslocamento,
                    result.Value?.RegistrosRetornado,
                    result.Value?.TotalRegistros
                }
            };

            return StatusCode(result.StatusCode, hateaos);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obtém vaga por ID",
            Description = "Retorna a vaga correspondente ao ID informado."
        )]
        [SwaggerResponse(statusCode: 200, description: "Vaga encontrada", type: typeof(VagaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Vaga não encontrada")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _vagaUseCase.ObterUmaVagaAsync(id);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            var hateaos = new
            {
                data = result.Value,
                links = new
                {
                    self = Url.Action(nameof(Get), "Vaga", new { id }),
                    get = Url.Action(nameof(Get), "Vaga", null),
                    put = Url.Action(nameof(Put), "Vaga", new { id }),
                    delete = Url.Action(nameof(Delete), "Vaga", new { id }),
                }
            };

            return StatusCode(result.StatusCode, hateaos);
        }

        [HttpPost]
        // [SwaggerRequestExample(typeof(VagaDto), typeof(VagaRequestSample))]
        [SwaggerResponse(statusCode: 200, description: "Vaga salva com sucesso", type: typeof(VagaEntity))]
        // [SwaggerResponseExample(statusCode: 200, typeof(VagaResponseSample))]
        public async Task<IActionResult> Post(VagaDto entity)
        {
            var result = await _vagaUseCase.AdicionarVagaAsync(entity);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            return StatusCode(result.StatusCode, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, VagaDto entity)
        {
            var result = await _vagaUseCase.EditarVagaAsync(id, entity);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            return StatusCode(result.StatusCode, result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _vagaUseCase.DeletarVagaAsync(id);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            return StatusCode(result.StatusCode, result.Value);
        }
    }
}
