using EclipseWorksChallange.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EclipseWorksChallange.Infraestructure.Persistence.Repositories
{
    public class ProjetosRepository : IProjetosRepository
    {
        private readonly EclipseWorksSQLServerContext _context;

        public ProjetosRepository(EclipseWorksSQLServerContext context)
        {
            _context = context;
        }
        public Projeto GetByID(int id, bool incluirTarefas = false)
        {
            var query = _context.Projeto
                .Include(p => p.Usuario)
                .AsQueryable();

            if (incluirTarefas)
                query = query.Include(p => p.Tarefas);

            return query.FirstOrDefault(p => p.ID == id);
        }

        public List<Projeto> GetAll(int userID)
        {
            return _context.Projeto.Where(x => x.UsuarioId == userID)
                .Include(p => p.Usuario)
                .Include(p => p.Tarefas).ThenInclude(x => x.Historicos)
                .ToList();
        }

        public void Add(Projeto projeto)
        {
            _context.Projeto.Add(projeto);
            _context.SaveChanges();

        }

        public void Delete(Projeto projeto)
        {
            _context.Projeto.Remove(projeto);
            _context.SaveChanges();

        }
    }
}
