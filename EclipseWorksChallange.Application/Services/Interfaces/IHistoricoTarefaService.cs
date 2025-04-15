using EclipseWorksChallange.Application.DTOs.Create;
using EclipseWorksChallange.Application.DTOs.Update;
using EclipseWorksChallange.Core.Models;

namespace EclipseWorksChallange.Application.Services
{
    public interface IHistoricoTarefaService
    {
        bool CreateNewComment(CriarComentarioDTO comentarioDTO);
        void Delete(ICollection<HistoricoTarefa> historicos);
        void RegistrarHistorico(AtualizarTarefaDTO tarefaDTO, List<AtualizaHistoricoTarefaDTO> listAtualizacao);
    }
}