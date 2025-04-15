using System.ComponentModel.DataAnnotations.Schema;

namespace EclipseWorksChallange.Core.Models
{
    public class HistoricoTarefa
    {
        public int Id { get; set; }
        [ForeignKey("Tarefa")]
        public int TarefaId { get; set; }
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public string Comentario { get; set; }
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;
        public string CampoAlterado { get; set; }
        public string ValorAnterior { get; set; }
        public string NovoValor { get; set; }

        public Tarefa Tarefa { get; set; }
        public Usuario Usuario { get; set; }
    }
}
