using EclipseWorksChallange.Core.Models;

namespace EclipseWorksChallange.Infraestructure.Persistence.Repositories
{
    public interface IUsuarioRepository
    {
        Usuario GetByID(int id);
        Task<List<UsuarioTarefas>> RelatorioMediaTarefasConcluidasUsuario(DateTime dataLimite);
    }
}