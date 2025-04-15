using EclipseWorksChallange.Core.Models;

namespace EclipseWorksChallange.Infraestructure.Persistence.Repositories
{
    public interface ITarefasRepository
    {
        void Add(Tarefa tarefa);
        bool Delete(Tarefa tarefa);
        List<Tarefa> GetAllByProject(int projetoId);
        Tarefa GetById(int id);
        Tarefa Update(Tarefa tarefa);
    }
}