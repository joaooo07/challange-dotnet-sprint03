using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChallangeDotnet.Domain.Entities
{
    [Table("tb_unidade")]
    [Index(nameof(Codigo), IsUnique = true)]
    public class UnidadeEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo nome é obrigatório")]
        [StringLength(120, ErrorMessage = "Nome não pode ter mais que 120 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo código é obrigatório")]
        [StringLength(20, ErrorMessage = "Código não pode ter mais que 20 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Column(TypeName = "NUMBER(1)")]
        public bool Ativa { get; set; } = true;

        [StringLength(255, ErrorMessage = "Observação não pode ter mais que 255 caracteres")]
        public string? Observacao { get; set; }
    }
}
