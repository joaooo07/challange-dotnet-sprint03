using ChallangeDotnet.Application.Dtos;
using ChallangeDotnet.Domain.Entities;

namespace ChallangeDotnet.Application.Mapper
{
    public static class UsuarioMapper
    {
        public static UsuarioEntity ToUsuarioEntity(this UsuarioDto obj)
        {
            return new UsuarioEntity
            {
                Nome = obj.Nome,
                Email = obj.Email,
                Senha = obj.Senha, 
                Ativo = obj.Ativo
            };
        }
    }
}
