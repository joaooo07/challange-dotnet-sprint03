using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChallangeDotnet.Domain.Entities
{
    [Table("tb_vaga")]
    [Index(nameof(Codigo), IsUnique = true)]
    public class VagaEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo código é obrigatório")]
        [StringLength(20, ErrorMessage = "Código não pode ter mais que 20 caracteres")]
        public string Codigo { get; set; } = string.Empty; // ex.: "A-01"

        [Column(TypeName = "NUMBER(1)")]
        public bool Coberta { get; set; }

        [Column(TypeName = "NUMBER(1)")]
        public bool Ocupada { get; set; }

        [StringLength(255, ErrorMessage = "Observação não pode ter mais que 255 caracteres")]
        public string? Observacao { get; set; }
    }
}
