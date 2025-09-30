using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChallangeDotnet.Domain.Entities
{
    [Table("tb_moto")]
    public class MotoEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo modelo é obrigatório")]
        [StringLength(100, ErrorMessage = "Modelo não pode ter mais que 100 caracteres")]
        public string Modelo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo marca é obrigatório")]
        [StringLength(100, ErrorMessage = "Marca não pode ter mais que 100 caracteres")]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo ano é obrigatório")]
        public int Ano { get; set; }

    }
}
