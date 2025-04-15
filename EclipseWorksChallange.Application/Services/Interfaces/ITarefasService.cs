using EclipseWorksChallange.Application.DTOs.Create;
using EclipseWorksChallange.Application.DTOs.Update;
using EclipseWorksChallange.Application.DTOs.View;

namespace EclipseWorksChallange.Application.Services
{
    public interface ITarefasService
    {
        (TarefaViewModel, string) CreateNewTask(CriarTarefaDTO tarefaDTO);
        bool DeleteTaskById(int id);
        List<TarefaViewModel> GetTasksByProjectID(int projectID);
        TarefaViewModel UpdateTask(AtualizarTarefaDTO tarefaDTO);
    }
}