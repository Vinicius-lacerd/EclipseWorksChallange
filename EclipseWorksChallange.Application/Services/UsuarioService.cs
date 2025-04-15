using EclipseWorksChallange.Application.DTOs.View;
using EclipseWorksChallange.Core.Models;
using EclipseWorksChallange.Infraestructure.Persistence.Repositories;

namespace EclipseWorksChallange.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public UsuarioViewModel GetUsuarioByID(int id)
        {
            Usuario usuario = _usuarioRepository.GetByID(id);

            if (usuario == null) return null;

            return new UsuarioViewModel(usuario);
        }

        public Usuario ViewModelToUsuario(UsuarioViewModel usuarioViewModel)
        {
            if (usuarioViewModel is null) throw new ArgumentNullException();

            var usuario = new Usuario() 
            {
                ID = usuarioViewModel.ID,
                Nome = usuarioViewModel.Nome,
                Projetos = usuarioViewModel.Projetos,
                NivelID = usuarioViewModel.NivelID
            };
            
            return usuario;
        }
    }
}
