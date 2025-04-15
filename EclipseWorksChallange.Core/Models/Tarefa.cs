using System.ComponentModel.DataAnnotations.Schema;

namespace EclipseWorksChallange.Core.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataVencimento { get; set; }
        public DateTime? DataConclusao { get; set; }
        
        public int StatusTarefa { get; set; } = 0;
        public int Prioridade { get; set; }
        [ForeignKey("Projeto")]
        public int ProjetoId { get; set; }
        [ForeignKey("Usuario")]
        public int UsuarioResponsavelId { get; set; }
        public ICollection<HistoricoTarefa> Historicos { get; set; }

        public Projeto Projeto { get; set; }
        public Usuario UsuarioResponsavel { get; set; }
    }
}
