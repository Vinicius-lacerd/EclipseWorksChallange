namespace EclipseWorksChallange.Core.Models
{
    public class Usuario
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public ICollection<Projeto> Projetos { get; set; }

        public int NivelID { get; set; }
        public NiveisUsuario Nivel { get; set; }
    }
}
