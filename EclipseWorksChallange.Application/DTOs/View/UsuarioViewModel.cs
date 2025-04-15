using EclipseWorksChallange.Core.Models;
using System.Collections.ObjectModel;

namespace EclipseWorksChallange.Application.DTOs.View
{
    public class UsuarioViewModel
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public ICollection<Projeto> Projetos { get; set; }
        public int NivelID { get; set; }

        public UsuarioViewModel(Usuario usuario)
        {
            ID = usuario.ID;
            Nome = usuario.Nome;
            Projetos= usuario.Projetos is null ? new Collection<Projeto>() : usuario.Projetos;
            NivelID = usuario.NivelID;
        }
    }
}
