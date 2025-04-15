using EclipseWorksChallange.Core.Models;

namespace EclipseWorksChallange.Application.DTOs.View
{
    public class ProjetoViewModel
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int UsuarioId { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataFinalizacao { get; set; }
        public int StatusProjeto { get; set; } = 0;
        public string NomeUsuario { get; set; }
        public ICollection<TarefaViewModel> Tarefas { get; set; }

        public ProjetoViewModel(Projeto projeto)
        {
            ID = projeto.ID;
            Nome = projeto.Nome;
            Descricao = projeto.Descricao;
            UsuarioId = projeto.UsuarioId;
            DataCriacao = projeto.DataCriacao;
            DataFinalizacao = projeto.DataFinalizacao;
            StatusProjeto = projeto.StatusProjeto;
            NomeUsuario = projeto.Usuario?.Nome;

            Tarefas = projeto.Tarefas?
               .Select(h => new TarefaViewModel(h))
               .ToList();
        }
    }
}
