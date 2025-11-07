using Asp.Versioning;
using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Application.Interface;
using ChallangeDotnet.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// using ChallangeDotnet.Doc.Samples; 

namespace ChallangeDotnet.Presentation.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioUseCase _usuarioUseCase;
        private readonly IConfiguration _configuration;

        public UsuarioController(IUsuarioUseCase usuarioUseCase, IConfiguration configuration)
        {
            _usuarioUseCase = usuarioUseCase;
            _configuration = configuration;
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
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var usuario = await _usuarioUseCase.ObterPorEmailAsync(loginDto.Email);

            if (usuario == null)
                return Unauthorized("Usuário não encontrado.");

            if (usuario.Senha != loginDto.Senha)
                return Unauthorized("Senha inválida.");

            // 🔑 Gerar token JWT
            var secret = _configuration["Secretkey"];
            if (string.IsNullOrEmpty(secret) || secret.Length < 32)
                return StatusCode(500, "Chave JWT inválida ou muito curta.");

            var key = Encoding.UTF8.GetBytes(secret); 


            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email)
        }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // 🔙 Retornar token + dados do usuário
            return Ok(new
            {
                message = "Login bem-sucedido!",
                token = tokenString,
                usuario = new
                {
                    usuario.Id,
                    usuario.Nome,
                    usuario.Email,
                    usuario.Ativo
                }
            });
        }


    }
}
