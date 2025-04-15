using EclipseWorksChallange.Core.Models;

namespace EclipseWorksChallange.Infraestructure.Persistence.Repositories
{
    public interface IHistoricoTarefaRepository
    {
        void Add(HistoricoTarefa historicoTarefa);
        void Delete(ICollection<HistoricoTarefa> historicos);
        List<HistoricoTarefa> GetAll(int tarefaId);
    }
}