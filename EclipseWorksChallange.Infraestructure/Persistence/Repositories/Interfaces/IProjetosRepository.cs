using EclipseWorksChallange.Core.Models;

namespace EclipseWorksChallange.Infraestructure.Persistence.Repositories
{
    public interface IProjetosRepository
    {
        void Add(Projeto projeto);
        void Delete(Projeto projeto);
        List<Projeto> GetAll(int userID);
        Projeto GetByID(int id, bool incluirTarefas = false);
    }
}