using EclipseWorksChallange.Core.Models;

namespace EclipseWorksChallange.Application.DTOs.View
{
    public class HistoricoTarefaViewModel
    {

        public DateTime DataAtualizacao { get; set; }
        public string UsuarioNome { get; set; }
        public string Comentario { get; set; }
        public string CampoAlterado { get; set; }
        public string ValorAnterior { get; set; }
        public string NovoValor { get; set; }

        public HistoricoTarefaViewModel(HistoricoTarefa historico)
        {
            DataAtualizacao = historico.DataAtualizacao;
            UsuarioNome = historico.Usuario?.Nome;
            Comentario = historico.Comentario;
            CampoAlterado = historico.CampoAlterado;
            ValorAnterior = historico.ValorAnterior;
            NovoValor = historico.NovoValor;
        }
    }

}
