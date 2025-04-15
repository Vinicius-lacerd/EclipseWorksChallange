using EclipseWorksChallange.Application.DTOs.View;
using EclipseWorksChallange.Core.Enums;
using EclipseWorksChallange.Core.Models;
using EclipseWorksChallange.Infraestructure.Persistence.Repositories;

namespace EclipseWorksChallange.Application.Services
{
    public class RelatoriosService : IRelatoriosService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public RelatoriosService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<RelatorioMediaTarefasConcluidasViewModel> MediaTarefasConcluidasUsuarios(int userId, DateTime dataLimite)
        {
            //minimo gerente
            if (_usuarioRepository.GetByID(userId).NivelID <= (int)NiveisUsuarioEnum.Gerente) return null;

            List<UsuarioTarefas> usuarios = await _usuarioRepository.RelatorioMediaTarefasConcluidasUsuario(dataLimite);

            var resultados = usuarios.Select(x => new UsuarioTarefasDTO
            {
                MediaTarefasConcluidas = x.MediaTarefasConcluidas,
                NomeUsuario = x.NomeUsuario,
                UsuarioId = x.UsuarioId
            }).ToList();

            var relatorio = new RelatorioMediaTarefasConcluidasViewModel
            {
                Nome = "Relatorio Media Tarefas Concluidas",
                DataGeracao = DateTime.Now,
                Resultados = resultados
            };

            return relatorio;
        }
    }

}
