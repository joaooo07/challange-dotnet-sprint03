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
    [Route("api/v1/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioUseCase _usuarioUseCase;

        public UsuarioController(IUsuarioUseCase usuarioUseCase)
        {
            _usuarioUseCase = usuarioUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
           Summary = "Lista usuários",
           Description = "Retorna a lista paginada de usuários cadastrados."
        )]
        [SwaggerResponse(statusCode: 200, description: "Lista retornada com sucesso", type: typeof(IEnumerable<UsuarioEntity>))]
        [SwaggerResponse(statusCode: 201, description: "Não possui dados para usuários")]
        // [SwaggerResponseExample(statusCode: 200, typeof(UsuarioResponseListSample))]
        [EnableRateLimiting("rateLimitePolicy")]
        public async Task<IActionResult> Get(int Deslocamento = 0, int RegistrosRetornado = 3)
        {
            var result = await _usuarioUseCase.ObterTodosUsuariosAsync(Deslocamento, RegistrosRetornado);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            var hateaos = new
            {
                data = result.Value?.Data.Select(u => new
                {
                    u.Id,
                    u.Nome,
                    u.Email,
                    u.Ativo,
                    links = new
                    {
                        self = Url.Action(nameof(Get), "Usuario", new { id = u.Id }, Request.Scheme),
                        put = Url.Action(nameof(Put), "Usuario", new { id = u.Id }, Request.Scheme),
                        delete = Url.Action(nameof(Delete), "Usuario", new { id = u.Id }, Request.Scheme),
                    }
                }),
                links = new
                {
                    self = Url.Action(nameof(Get), "Usuario", null, Request.Scheme),
                    create = Url.Action(nameof(Post), "Usuario", null, Request.Scheme),
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
            Summary = "Obtém usuário por ID",
            Description = "Retorna o usuário correspondente ao ID informado."
        )]
        [SwaggerResponse(statusCode: 200, description: "Usuário encontrado", type: typeof(UsuarioEntity))]
        [SwaggerResponse(statusCode: 404, description: "Usuário não encontrado")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _usuarioUseCase.ObterUmUsuarioAsync(id);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            var hateaos = new
            {
                data = result.Value,
                links = new
                {
                    self = Url.Action(nameof(Get), "Usuario", new { id }),
                    get = Url.Action(nameof(Get), "Usuario", null),
                    put = Url.Action(nameof(Put), "Usuario", new { id }),
                    delete = Url.Action(nameof(Delete), "Usuario", new { id }),
                }
            };

            return StatusCode(result.StatusCode, hateaos);
        }

        [HttpPost]
        // [SwaggerRequestExample(typeof(UsuarioDto), typeof(UsuarioRequestSample))]
        [SwaggerResponse(statusCode: 200, description: "Usuário salvo com sucesso", type: typeof(UsuarioEntity))]
        // [SwaggerResponseExample(statusCode: 200, typeof(UsuarioResponseSample))]
        public async Task<IActionResult> Post(UsuarioDto entity)
        {
            var result = await _usuarioUseCase.AdicionarUsuarioAsync(entity);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            return StatusCode(result.StatusCode, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UsuarioDto entity)
        {
            var result = await _usuarioUseCase.EditarUsuarioAsync(id, entity);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            return StatusCode(result.StatusCode, result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _usuarioUseCase.DeletarUsuarioAsync(id);

            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            return StatusCode(result.StatusCode, result.Value);
        }

        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "Login de usuário",
            Description = "Valida email e senha e retorna os dados do usuário (sem token por enquanto)."
        )]
        [SwaggerResponse(statusCode: 200, description: "Login bem-sucedido", type: typeof(UsuarioEntity))]
        [SwaggerResponse(statusCode: 401, description: "Credenciais inválidas")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var usuario = await _usuarioUseCase.ObterPorEmailAsync(loginDto.Email);

            if (usuario == null)
                return Unauthorized("Usuário não encontrado.");

            if (usuario.Senha != loginDto.Senha) // ⚠️ Sem hash, só comparação direta
                return Unauthorized("Senha inválida.");

            var usuarioSemSenha = new
            {
                usuario.Id,
                usuario.Nome,
                usuario.Email,
                usuario.Ativo
            };

            return Ok(usuarioSemSenha);
        }

    }
}
