using EclipseWorksChallange.Core.Models;

namespace EclipseWorksChallange.Infraestructure.Persistence.Repositories
{
    public class HistoricoTarefaRepository : IHistoricoTarefaRepository
    {
        private readonly EclipseWorksSQLServerContext _context;

        public HistoricoTarefaRepository(EclipseWorksSQLServerContext context)
        {
            _context = context;
        }


        public void Add(HistoricoTarefa historicoTarefa)
        {
            _context.HistoricoTarefa.Add(historicoTarefa);
            _context.SaveChanges();
        }

        public void Delete(ICollection<HistoricoTarefa> historicos)
        {
            foreach (var item in historicos)
            {
                _context.HistoricoTarefa.Remove(item);
            }
            _context.SaveChanges();
        }

        public List<HistoricoTarefa> GetAll(int tarefaId)
        {
            return _context.HistoricoTarefa.Where(x => x.TarefaId == tarefaId)
                //.Include(x => x.Usuario)
                .ToList();
        }
    }
}
