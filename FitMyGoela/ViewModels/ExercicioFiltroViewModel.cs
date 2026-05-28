using FitMyGoela.Models;
namespace FitMyGoela.ViewModels
{
    public class ExercicioFiltroViewModel
    {
        public List<Exercicio>? Exercicios { get; set; }
        public List<TiposDeTreino>? TiposDeTreinos { get; set; }
        public string? Pesquisa { get; set; }
        public int? TiposDeTreinoId { get; set; }
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
    }
}
