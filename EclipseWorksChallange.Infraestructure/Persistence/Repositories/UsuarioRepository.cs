using EclipseWorksChallange.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EclipseWorksChallange.Infraestructure.Persistence.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly EclipseWorksSQLServerContext _context;
        public UsuarioRepository(EclipseWorksSQLServerContext context)
        {
            _context = context;
        }

        public Usuario GetByID(int id)
        {
            return _context.Usuario
                .Include(x => x.Nivel)
                .FirstOrDefault(x => x.ID == id);
        }

        public Task<List<UsuarioTarefas>> RelatorioMediaTarefasConcluidasUsuario(DateTime dataLimite)
        {
            return _context.Usuario
                     .Select(u => new UsuarioTarefas
                     {
                         UsuarioId = u.ID,
                         NomeUsuario = u.Nome,
                         MediaTarefasConcluidas = _context.Tarefa
                             .Count(t => t.UsuarioResponsavelId == u.ID &&
                                         t.StatusTarefa == 2 && //  concluida
                                         t.DataConclusao >= dataLimite)
                     })
                     .OrderByDescending(x => x.MediaTarefasConcluidas)
                     .ToListAsync();
        }
    }
}
