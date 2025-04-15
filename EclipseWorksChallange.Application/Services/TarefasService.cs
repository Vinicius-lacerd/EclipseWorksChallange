
using EclipseWorksChallange.Application.DTOs.Create;
using EclipseWorksChallange.Application.DTOs.Update;
using EclipseWorksChallange.Application.DTOs.View;
using EclipseWorksChallange.Core.Enums;
using EclipseWorksChallange.Core.Models;
using EclipseWorksChallange.Infraestructure.Persistence.Repositories;

namespace EclipseWorksChallange.Application.Services
{
    public class TarefasService : ITarefasService
    {
        private readonly ITarefasRepository _tarefasRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly IProjetosService _projetosService;
        private readonly IHistoricoTarefaService _historicoTarefaService;

        public TarefasService(ITarefasRepository tarefasRepository, IProjetosService projetosService, IUsuarioService usuarioService, IHistoricoTarefaService historicoTarefaService)
        {
            _tarefasRepository = tarefasRepository;
            _projetosService = projetosService;
            _usuarioService = usuarioService;
            _historicoTarefaService = historicoTarefaService;
        }

        public (TarefaViewModel, string) CreateNewTask(CriarTarefaDTO tarefaDTO)
        {
            try
            {
                var usuario = _usuarioService.GetUsuarioByID(tarefaDTO.UsuarioResponsavelId);

                if (usuario is null) return (null, "Usuario Invalido!");

                var projeto = _projetosService.GetProjectByID(tarefaDTO.ProjetoId);

                if (projeto is null) return (null, "Projeto Invalido");
                if (projeto.Tarefas?.Count >= 20) return (null, "Projeto atingiu limite maximo de Tarefas!");


                Tarefa tarefa = new Tarefa()
                {
                    DataVencimento = tarefaDTO.DataVencimento,
                    Nome = tarefaDTO.Nome,
                    Descricao = tarefaDTO.Descricao,
                    Prioridade = tarefaDTO.Prioridade,
                    StatusTarefa = 0,
                    UsuarioResponsavelId = tarefaDTO.UsuarioResponsavelId,
                    //UsuarioResponsavel = _usuarioService.ViewModelToUsuario(usuario),
                    //Projeto = _projetosService.ViewModelToProject(projeto),
                    ProjetoId = tarefaDTO.ProjetoId
                };

                _tarefasRepository.Add(tarefa);

                return (new TarefaViewModel(tarefa), "");
            }
            catch (Exception)
            {
                //log
                //tratar a excecao tbm
                return (null, "");
            }
        }

        public bool DeleteTaskById(int id)
        {
            try
            {
                var tarefa = _tarefasRepository.GetById(id);

                if (tarefa is null) return false;

                if (tarefa.Historicos?.Count > 0)
                    _historicoTarefaService.Delete(tarefa.Historicos);

                _tarefasRepository.Delete(tarefa);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<TarefaViewModel> GetTasksByProjectID(int projectID)
        {
            List<Tarefa> tarefas = _tarefasRepository.GetAllByProject(projectID);

            //if (tarefas == null) return null;
            //if (tarefas.Count == 0) return null;

            List<TarefaViewModel> tarefasViewModel = new List<TarefaViewModel>();
            foreach (Tarefa tarefa in tarefas)
                tarefasViewModel.Add(new TarefaViewModel(tarefa));

            return tarefasViewModel;
        }

        public TarefaViewModel UpdateTask(AtualizarTarefaDTO tarefaDTO)
        {
            try
            {
                var tarefaExistente = _tarefasRepository.GetById(tarefaDTO.Id);

                if (tarefaExistente == null)
                    return null;

                //deveria validar se esta concluida ou cancelada ?


                var usuario = _usuarioService.GetUsuarioByID(tarefaDTO.UsuarioAlteracaoId);

                if (usuario is null) return null;

                List<AtualizaHistoricoTarefaDTO> listAtualizacao = VerificaAlteracoesTarefaDTO(tarefaDTO, tarefaExistente);

                if(tarefaDTO.StatusTarefa == (int)StatusTarefaEnum.Concluida)
                    tarefaExistente.DataConclusao = DateTime.Now;

                if (listAtualizacao.Any())
                {
                    _historicoTarefaService.RegistrarHistorico(tarefaDTO, listAtualizacao);
                    tarefaExistente = _tarefasRepository.Update(tarefaExistente);
                }

                return new TarefaViewModel(tarefaExistente);
            }
            catch (Exception)
            {
                //log e tratamento
                return null;
            }
        }

        private List<AtualizaHistoricoTarefaDTO> VerificaAlteracoesTarefaDTO(AtualizarTarefaDTO tarefaDTO, Tarefa tarefaExistente)
        {
            List<AtualizaHistoricoTarefaDTO> listAtualizacao = new List<AtualizaHistoricoTarefaDTO>();

            if (tarefaExistente.Nome != tarefaDTO.Nome)
            {
                listAtualizacao.Add(
                    new AtualizaHistoricoTarefaDTO() { CampoAlterado = nameof(tarefaDTO.Nome), NovoValor = tarefaDTO.Nome, ValorAnterior = tarefaExistente.Nome });
                tarefaExistente.Nome = tarefaDTO.Nome;
            }

            if (tarefaExistente.Descricao != tarefaDTO.Descricao)
            {
                listAtualizacao.Add(
                    new AtualizaHistoricoTarefaDTO() { CampoAlterado = nameof(tarefaDTO.Descricao), NovoValor = tarefaDTO.Descricao, ValorAnterior = tarefaExistente.Descricao });
                tarefaExistente.Descricao = tarefaDTO.Descricao;
            }

            if (tarefaExistente.StatusTarefa != tarefaDTO.StatusTarefa)
            {
                listAtualizacao.Add(
                    new AtualizaHistoricoTarefaDTO()
                    {
                        CampoAlterado = nameof(tarefaDTO.StatusTarefa),
                        NovoValor = tarefaDTO.StatusTarefa.ToString(),
                        ValorAnterior = tarefaExistente.StatusTarefa.ToString()
                    });
                tarefaExistente.StatusTarefa = tarefaDTO.StatusTarefa;
            }

            if (tarefaExistente.DataVencimento != tarefaDTO.DataVencimento)
            {
                listAtualizacao.Add(
                    new AtualizaHistoricoTarefaDTO()
                    {
                        CampoAlterado = nameof(tarefaDTO.DataVencimento),
                        NovoValor = tarefaDTO.DataVencimento?.ToString(),
                        ValorAnterior = tarefaExistente.DataVencimento?.ToString()
                    });
                tarefaExistente.DataVencimento = tarefaDTO.DataVencimento;
            }
            listAtualizacao.ForEach(x =>
            {
                x.TarefaId = tarefaDTO.Id;
                x.UsuarioId = tarefaDTO.UsuarioAlteracaoId;
                x.Comentario = tarefaDTO.Comentario;
            });
            return listAtualizacao;
        }
    }
}
