using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitMyGoela.Models
{
    public class Exercicio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do exercício é obrigatório")]
        [StringLength(80)]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "A descrição é necessária")]
        public string? Descricao { get; set; }

        public int Series { get; set; }

        public int Repeticoes { get; set; }

        public string? LinkVideoTutorial { get; set; }

        public string? UserId { get; set; }

        public int TiposDeTreinoId { get; set; }
        public TiposDeTreino? TiposDeTreino { get; set; }
        public string? Imagem { get; internal set; }
    }
}
