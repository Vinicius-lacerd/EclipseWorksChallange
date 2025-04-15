using EclipseWorksChallange.Application.DTOs.Create;
using EclipseWorksChallange.Application.DTOs.View;
using EclipseWorksChallange.Core.Enums;
using EclipseWorksChallange.Core.Models;
using EclipseWorksChallange.Infraestructure.Persistence.Repositories;

namespace EclipseWorksChallange.Application.Services
{
    public class ProjetosService : IProjetosService
    {
        private readonly IProjetosRepository _projetoRepository;
        private readonly IUsuarioService _usuarioService;

        public ProjetosService(IProjetosRepository projestosRepository, IUsuarioService usuarioRepository)
        {
            _projetoRepository = projestosRepository;
            _usuarioService = usuarioRepository;
        }

        public ProjetoViewModel CreateNewProject(CriarProjetoDTO projetoDTO)
        {
            try
            {
                var usuario = _usuarioService.GetUsuarioByID(projetoDTO.UsuarioId);

                if (usuario is null) return null;

                Projeto projeto = new Projeto()
                {
                    Nome = projetoDTO.Nome,
                    StatusProjeto = (int)StatusTarefaEnum.Pendente,
                    DataCriacao = projetoDTO.DataCriacao,
                    DataFinalizacao = projetoDTO.DataFinalizacao,
                    Descricao = projetoDTO.Descricao,
                    UsuarioId = projetoDTO.UsuarioId
                };

                _projetoRepository.Add(projeto);

                projeto.Usuario = _usuarioService.ViewModelToUsuario(usuario);

                return new ProjetoViewModel(projeto);
            }
            catch (Exception)
            {
                //log
                //tratar a excecao tbm
                return null;
            }
        }

        public (bool, string) DeleteProject(int projetoId)
        {
            try
            {
                Projeto projeto = _projetoRepository.GetByID(projetoId, true);

                if (projeto is null) return (false, "Projeto Invalido!");
                if (projeto.Tarefas.Any(x => x.StatusTarefa == (int)StatusTarefaEnum.Pendente)) return (false, "Projeto com tarefas Pendentes!");

                _projetoRepository.Delete(projeto);
                return (true, "");
            }
            catch (Exception)
            {
                //log and tratar
                return (false, "");
            }
        }

        public ProjetoViewModel GetProjectByID(int projetoId)
        {
            Projeto projeto = _projetoRepository.GetByID(projetoId);

            if (projeto is null) return null;

            return new ProjetoViewModel(projeto);
        }

        public List<ProjetoViewModel> GetProjectsByUserID(int userID)
        {
            List<Projeto> projetos = _projetoRepository.GetAll(userID);

            if (projetos == null) return null;
            if (projetos.Count == 0) return null;

            List<ProjetoViewModel> projetosView = new List<ProjetoViewModel>();
            foreach (Projeto projeto in projetos)
                projetosView.Add(new ProjetoViewModel(projeto));

            return projetosView;
        }

        
    }
}
