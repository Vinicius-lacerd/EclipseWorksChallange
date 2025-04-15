using EclipseWorksChallange.Application.DTOs.Create;
using EclipseWorksChallange.Application.DTOs.Update;
using EclipseWorksChallange.Core.Models;
using EclipseWorksChallange.Infraestructure.Persistence.Repositories;

namespace EclipseWorksChallange.Application.Services
{
    public class HistoricoTarefaService : IHistoricoTarefaService
    {

        private readonly IHistoricoTarefaRepository _historicoTarefaRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly ITarefasRepository _tarefasRepository;


        public HistoricoTarefaService(IHistoricoTarefaRepository historicoTarefaRepository, IUsuarioService usuarioService, ITarefasRepository tarefasRepository)
        {
            _historicoTarefaRepository = historicoTarefaRepository;
            _usuarioService = usuarioService;
            _tarefasRepository = tarefasRepository;
        }

        public bool CreateNewComment(CriarComentarioDTO comentarioDTO)
        {
            var tarefa = _tarefasRepository.GetById(comentarioDTO.TarefaId);

            if(tarefa is  null ) return false;

            var usuario = _usuarioService.GetUsuarioByID(comentarioDTO.UsuarioId);

            if (usuario == null) return false;

            HistoricoTarefa historicoTarefa = new HistoricoTarefa()
            {
                Comentario = comentarioDTO.Comentario,
                TarefaId = comentarioDTO.TarefaId,
                UsuarioId = comentarioDTO.UsuarioId,
            };

            _historicoTarefaRepository.Add(historicoTarefa);
            return true;
        }

        public void Delete(ICollection<HistoricoTarefa> historicos)
        {
            _historicoTarefaRepository.Delete(historicos);
        }

        public void RegistrarHistorico(AtualizarTarefaDTO tarefaDTO, List<AtualizaHistoricoTarefaDTO> listAtualizacao)
        {
            foreach (var item in listAtualizacao)
            {
                _historicoTarefaRepository.Add(new HistoricoTarefa()
                {
                    CampoAlterado = item.CampoAlterado,
                    Comentario = item.Comentario,
                    DataAtualizacao = DateTime.Now,
                    NovoValor = item.NovoValor, 
                    TarefaId = tarefaDTO.Id,
                    UsuarioId = tarefaDTO.UsuarioAlteracaoId,
                    ValorAnterior = item.ValorAnterior,
                });
            }
        }
    }
}
