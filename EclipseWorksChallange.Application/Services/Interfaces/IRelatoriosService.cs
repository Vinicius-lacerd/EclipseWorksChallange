using EclipseWorksChallange.Application.DTOs.View;

namespace EclipseWorksChallange.Application.Services
{
    public interface IRelatoriosService
    {
        Task<RelatorioMediaTarefasConcluidasViewModel> MediaTarefasConcluidasUsuarios(int userId, DateTime dataLimite);
    }
}