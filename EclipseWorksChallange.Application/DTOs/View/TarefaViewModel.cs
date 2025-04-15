using EclipseWorksChallange.Core.Enums;
using EclipseWorksChallange.Core.Models;

namespace EclipseWorksChallange.Application.DTOs.View
{
    public class TarefaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public DateTime? DataConclusao { get; set; }
        public string Status { get; set; }
        public string Prioridade { get; set; }
        public int ProjetoId { get; set; }
        public string ProjetoNome { get; set; }
        public int UsuarioResponsavelId { get; set; }
        public string UsuarioResponsavelNome { get; set; }
        public List<HistoricoTarefaViewModel> Historicos { get; set; }

        public TarefaViewModel(Tarefa tarefa)
        {
            Id = tarefa.Id;
            Nome = tarefa.Nome;
            Descricao = tarefa.Descricao;
            DataCriacao = tarefa.DataCriacao;
            DataVencimento = tarefa.DataVencimento;
            DataConclusao = tarefa.DataConclusao;
            Status = ((StatusTarefaEnum)tarefa.StatusTarefa).ToString();
            Prioridade = ((PrioridadeTarefaEnum)tarefa.Prioridade).ToString();
            ProjetoId = tarefa.ProjetoId;
            ProjetoNome = tarefa.Projeto?.Nome;
            UsuarioResponsavelId = tarefa.UsuarioResponsavelId;
            UsuarioResponsavelNome = tarefa.UsuarioResponsavel?.Nome;

            Historicos = tarefa.Historicos?
               .Select(h => new HistoricoTarefaViewModel(h))
               .ToList();
        }
    }
}
