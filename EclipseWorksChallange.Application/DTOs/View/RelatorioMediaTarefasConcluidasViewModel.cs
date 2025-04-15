
namespace EclipseWorksChallange.Application.DTOs.View
{
    public class RelatorioMediaTarefasConcluidasViewModel
    {
        public string Nome { get; set; }
        public DateTime DataGeracao { get; set; }
        public List<UsuarioTarefasDTO> Resultados { get; set; }
    }

    public class UsuarioTarefasDTO
    {
        public int UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
        public double MediaTarefasConcluidas { get; set; }
    }
}
