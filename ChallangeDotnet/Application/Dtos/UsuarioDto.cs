namespace ChallangeDotnet.Application.Dtos
{
   public record UsuarioDto(
           string Nome,
           string Email,
           string Senha,
           bool Ativo
    );
}
