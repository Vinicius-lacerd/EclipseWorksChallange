using EclipseWorksChallange.Application.DTOs.View;
using EclipseWorksChallange.Core.Models;

namespace EclipseWorksChallange.Application.Services
{
    public interface IUsuarioService
    {
        UsuarioViewModel GetUsuarioByID(int id);
        Usuario ViewModelToUsuario(UsuarioViewModel usuarioViewModel);
    }
}