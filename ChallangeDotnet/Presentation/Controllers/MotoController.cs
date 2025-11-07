using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Application.Interface;
using ChallangeDotnet.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;

namespace ChallangeDotnet.Presentation.Controllers
{
    [Route("api/v1/moto")]
    [ApiController]
    public class MotoController : ControllerBase
    {
        private readonly IMotoUseCase _motoUseCase;

        public MotoController(IMotoUseCase motoUseCase)
        {
            _motoUseCase = motoUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
           Summary = "Lista motos",
           Description = "Retorna a lista paginada de motos cadastradas."
        )]
        [SwaggerResponse(statusCode: 200, description: "Lista retornada com sucesso", type: typeof(IEnumerable<MotoEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Não possui dados para motos")]
        [EnableRateLimiting("rateLimitePolicy")]
        public async Task<IActionResult> Get(int Deslocamento = 0, int RegistrosRetornado = 3)
        {
            var result = await _motoUseCase.ObterTodasMotosAsync(Deslocamento, RegistrosRetornado);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            var hateaos = new
            {
                data = result.Value?.Data.Select(m => new
                {
                    m.Id,
                    m.Modelo,
                    m.Marca,
                    m.Ano,
                    links = new
                    {
                        self = Url.Action(nameof(Get), "Moto", new { id = m.Id }, Request.Scheme),
                        put = Url.Action(nameof(Put), "Moto", new { id = m.Id }, Request.Scheme),
                        delete = Url.Action(nameof(Delete), "Moto", new { id = m.Id }, Request.Scheme),
                    }
                }),
                links = new
                {
                    self = Url.Action(nameof(Get), "Moto", null, Request.Scheme),
                    create = Url.Action(nameof(Post), "Moto", null, Request.Scheme),
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
            Summary = "Obtém moto por ID",
            Description = "Retorna a moto correspondente ao ID informado."
        )]
        [SwaggerResponse(statusCode: 200, description: "Moto encontrada", type: typeof(MotoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Moto não encontrada")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _motoUseCase.ObterUmaMotoAsync(id);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            var hateaos = new
            {
                data = result.Value,
                links = new
                {
                    self = Url.Action(nameof(Get), "Moto", new { id }),
                    get = Url.Action(nameof(Get), "Moto", null),
                    put = Url.Action(nameof(Put), "Moto", new { id }),
                    delete = Url.Action(nameof(Delete), "Moto", new { id }),
                }
            };

            return StatusCode(result.StatusCode, hateaos);
        }

        [HttpPost]
        [SwaggerResponse(statusCode: 200, description: "Moto salva com sucesso", type: typeof(MotoEntity))]
        public async Task<IActionResult> Post(MotoDto entity)
        {
            var result = await _motoUseCase.AdicionarMotoAsync(entity);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            return StatusCode(result.StatusCode, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, MotoDto entity)
        {
            var result = await _motoUseCase.EditarMotoAsync(id, entity);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            return StatusCode(result.StatusCode, result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _motoUseCase.DeletarMotoAsync(id);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            return StatusCode(result.StatusCode, result.Value);
        }
    }
}
