namespace EclipseWorksChallange.Application.DTOs.Create
{
    public class CriarTarefaDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public int Prioridade { get; set; }
        public int ProjetoId { get; set; }
        public int UsuarioResponsavelId { get; set; }

    }
}
