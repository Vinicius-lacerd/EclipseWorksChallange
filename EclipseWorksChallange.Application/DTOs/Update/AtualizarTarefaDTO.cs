namespace EclipseWorksChallange.Application.DTOs.Update
{
    public class AtualizarTarefaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Comentario { get; set; }
        public DateTime? DataVencimento { get; set; }
        public int StatusTarefa { get; set; }
        public int UsuarioAlteracaoId { get; set; }
    }
}
