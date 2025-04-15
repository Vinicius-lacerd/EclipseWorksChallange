using EclipseWorksChallange.Core.Models;

namespace EclipseWorksChallange.Application.DTOs.Create
{
    public class CriarProjetoDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int UsuarioId { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataFinalizacao { get; set; }  
    }
}
