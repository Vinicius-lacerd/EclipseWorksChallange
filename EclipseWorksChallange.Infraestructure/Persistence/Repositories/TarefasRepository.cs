using EclipseWorksChallange.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EclipseWorksChallange.Infraestructure.Persistence.Repositories
{
    public class TarefasRepository : ITarefasRepository
    {
        private readonly EclipseWorksSQLServerContext _context;

        public TarefasRepository(EclipseWorksSQLServerContext context)
        {
            _context = context;
        }

        public Tarefa GetById(int id)
        {
            return _context.Tarefa
                //.Include(x => x.UsuarioResponsavel)
                //.Include(x => x.Projeto)
                .Include(x => x.Historicos)
                .FirstOrDefault(x => x.Id == id);
        }

        public List<Tarefa> GetAllByProject(int projetoId)
        {
            return _context.Tarefa.Where(x => x.ProjetoId == projetoId)
                .Include(x => x.Projeto)
                .Include(x => x.UsuarioResponsavel)
                .Include(x => x.Historicos)
                .ToList();
        }

        public void Add(Tarefa tarefa)
        {
            _context.Tarefa.Add(tarefa);
            _context.SaveChanges();
        }

        public bool Delete(Tarefa tarefa)
        {
            try
            {
                _context.Tarefa.Remove(tarefa);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
        public Tarefa Update(Tarefa tarefa)
        {
            try
            {
                _context.Update(tarefa);
                _context.SaveChanges();

                return tarefa;
            }
            catch (DbUpdateException)
            {
                return tarefa;
            }
        }
    }
}
