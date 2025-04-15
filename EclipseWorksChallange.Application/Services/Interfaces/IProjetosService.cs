using EclipseWorksChallange.Application.DTOs.Create;
using EclipseWorksChallange.Application.DTOs.View;
using EclipseWorksChallange.Core.Models;

namespace EclipseWorksChallange.Application.Services
{
    public interface IProjetosService
    {
        ProjetoViewModel CreateNewProject(CriarProjetoDTO projetoDTO);
        ProjetoViewModel GetProjectByID(int projetoId);
        List<ProjetoViewModel> GetProjectsByUserID(int userID);

        (bool,string) DeleteProject(int projetoId);
    }
}