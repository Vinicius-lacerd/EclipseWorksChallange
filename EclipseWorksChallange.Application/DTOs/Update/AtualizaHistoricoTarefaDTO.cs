namespace EclipseWorksChallange.Application.DTOs.Update
{
    public class AtualizaHistoricoTarefaDTO
    {
        public int Id { get; set; }
        public int TarefaId { get; set; }
        public int UsuarioId { get; set; }
        public string Comentario { get; set; }
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;
        public string CampoAlterado { get; set; }
        public string ValorAnterior { get; set; }
        public string NovoValor { get; set; }

    }
}
