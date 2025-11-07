using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChallangeDotnet.Domain.Entities
{
    [Table("tb_usuario")]               
    [Index(nameof(Email), IsUnique = true)]
    public class UsuarioEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo nome é obrigatório")]
        [StringLength(150, ErrorMessage = "Nome não pode ter mais que 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo e-mail é obrigatório")]
        [StringLength(150, ErrorMessage = "E-mail não pode ter mais que 150 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo senha é obrigatório")]
        [StringLength(255, ErrorMessage = "Senha não pode ter mais que 255 caracteres")]
        public string Senha { get; set; } = string.Empty;

        [Column(TypeName = "NUMBER(1)")]
        public bool Ativo { get; set; } = true;
    }
}
