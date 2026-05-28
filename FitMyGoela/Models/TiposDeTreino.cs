using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitMyGoela.Models
{
    public class TiposDeTreino
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do treino é obrigatório")]
        [StringLength(100)]
        public string? Nome { get; set; }

        [Required]
        public string? Descricao { get; set; }

        [Range(1, 300, ErrorMessage = "A duração deve ser entre 1 e 300 minutos")]
        public int DuracaoMinutos { get; set; }

        public string? UserId { get; set; }

        public List<Exercicio>? Exercicios { get; set; }
    }
}
