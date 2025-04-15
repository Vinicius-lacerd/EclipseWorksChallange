using System.ComponentModel.DataAnnotations.Schema;

namespace EclipseWorksChallange.Core.Models
{
    public class Projeto
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataFinalizacao { get; set; }
        public int StatusProjeto { get; set; } = 0;
        public Usuario Usuario { get; set; }
        public ICollection<Tarefa> Tarefas { get; set; }

    }
}
