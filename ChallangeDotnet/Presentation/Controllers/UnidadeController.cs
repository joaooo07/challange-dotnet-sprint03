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
    [Route("api/v1/unidade")]
    [ApiController]
    public class UnidadeController : ControllerBase
    {
        private readonly IUnidadeUseCase _unidadeUseCase;

        public UnidadeController(IUnidadeUseCase unidadeUseCase)
        {
            _unidadeUseCase = unidadeUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
           Summary = "Lista unidades",
           Description = "Retorna a lista paginada de unidades cadastradas."
        )]
        [SwaggerResponse(statusCode: 200, description: "Lista retornada com sucesso", type: typeof(IEnumerable<UnidadeEntity>))]
        [SwaggerResponse(statusCode: 201, description: "Não possui dados para unidades")]
        // [SwaggerResponseExample(statusCode: 200, typeof(UnidadeResponseListSample))]
        [EnableRateLimiting("rateLimitePolicy")]
        public async Task<IActionResult> Get(int Deslocamento = 0, int RegistrosRetornado = 3)
        {
            var result = await _unidadeUseCase.ObterTodasUnidadesAsync(Deslocamento, RegistrosRetornado);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            var hateaos = new
            {
                data = result.Value?.Data.Select(u => new
                {
                    u.Id,
                    u.Nome,
                    u.Codigo,
                    u.Ativa,
                    u.Observacao,
                    links = new
                    {
                        self = Url.Action(nameof(Get), "Unidade", new { id = u.Id }, Request.Scheme),
                        put = Url.Action(nameof(Put), "Unidade", new { id = u.Id }, Request.Scheme),
                        delete = Url.Action(nameof(Delete), "Unidade", new { id = u.Id }, Request.Scheme),
                    }
                }),
                links = new
                {
                    self = Url.Action(nameof(Get), "Unidade", null, Request.Scheme),
                    create = Url.Action(nameof(Post), "Unidade", null, Request.Scheme),
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
            Summary = "Obtém unidade por ID",
            Description = "Retorna a unidade correspondente ao ID informado."
        )]
        [SwaggerResponse(statusCode: 200, description: "Unidade encontrada", type: typeof(UnidadeEntity))]
        [SwaggerResponse(statusCode: 404, description: "Unidade não encontrada")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _unidadeUseCase.ObterUmaUnidadeAsync(id);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            var hateaos = new
            {
                data = result.Value,
                links = new
                {
                    self = Url.Action(nameof(Get), "Unidade", new { id }),
                    get = Url.Action(nameof(Get), "Unidade", null),
                    put = Url.Action(nameof(Put), "Unidade", new { id }),
                    delete = Url.Action(nameof(Delete), "Unidade", new { id }),
                }
            };

            return StatusCode(result.StatusCode, hateaos);
        }

        [HttpPost]
        // [SwaggerRequestExample(typeof(UnidadeDto), typeof(UnidadeRequestSample))]
        [SwaggerResponse(statusCode: 200, description: "Unidade salva com sucesso", type: typeof(UnidadeEntity))]
        // [SwaggerResponseExample(statusCode: 200, typeof(UnidadeResponseSample))]
        public async Task<IActionResult> Post(UnidadeDto entity)
        {
            var result = await _unidadeUseCase.AdicionarUnidadeAsync(entity);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            return StatusCode(result.StatusCode, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UnidadeDto entity)
        {
            var result = await _unidadeUseCase.EditarUnidadeAsync(id, entity);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            return StatusCode(result.StatusCode, result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _unidadeUseCase.DeletarUnidadeAsync(id);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            return StatusCode(result.StatusCode, result.Value);
        }
    }
}
