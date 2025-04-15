using EclipseWorksChallange.Application.DTOs.Create;
using EclipseWorksChallange.Application.DTOs.View;
using EclipseWorksChallange.Application.Services;
using EclipseWorksChallange.Core.Enums;
using EclipseWorksChallange.Core.Models;
using EclipseWorksChallange.Infraestructure.Persistence.Repositories;
using Moq;

namespace EclipseWorks.Test.Application.Services
{
    public class ProjetosServiceTest
    {
        private readonly Mock<IProjetosRepository> _mockProjetoRepo;
        private readonly Mock<IUsuarioService> _mockUsuarioService;
        private readonly ProjetosService _service;

        public ProjetosServiceTest()
        {
            _mockProjetoRepo = new Mock<IProjetosRepository>();
            _mockUsuarioService = new Mock<IUsuarioService>();
            _service = new ProjetosService(_mockProjetoRepo.Object, _mockUsuarioService.Object);
        }

        [Fact]
        public void CreateNewProject_QuandoUsuarioNaoExiste_RetornaNull()
        {
            var projetoDTO = new CriarProjetoDTO { UsuarioId = 1 };
            _mockUsuarioService.Setup(x => x.GetUsuarioByID(1)).Returns((UsuarioViewModel)null);
            
            var result = _service.CreateNewProject(projetoDTO);
            
            Assert.Null(result);
            _mockProjetoRepo.Verify(x => x.Add(It.IsAny<Projeto>()), Times.Never);
        }

        [Fact]
        public void CreateNewProject_QuandoUsuarioExiste_CriarProjeto()
        {            
            var projetoDTO = new CriarProjetoDTO
            {
                UsuarioId = 1,
                Nome = "projeto teste",
                DataCriacao = DateTime.Now,
                Descricao = "descricao"
            };

            var usuario = new Usuario { ID = 1 , Nome = "user teste"};
            var usuarioViewModel = new UsuarioViewModel (usuario);

            _mockUsuarioService.Setup(x => x.GetUsuarioByID(1)).Returns(usuarioViewModel);
            _mockProjetoRepo.Setup(x => x.Add(It.IsAny<Projeto>()));
            
            var result = _service.CreateNewProject(projetoDTO);
            
            Assert.NotNull(result);
            Assert.Equal("projeto teste", result.Nome);

            _mockProjetoRepo.Verify(x => x.Add(It.Is<Projeto>(p =>
                p.Nome == "projeto teste" &&
                p.UsuarioId == 1 &&
                p.StatusProjeto == (int)StatusTarefaEnum.Pendente)), Times.Once);
        }

        [Fact]
        public void CreateNewProject_QuandoExcecao_RetornaNull()
        {
            var projetoDTO = new CriarProjetoDTO { UsuarioId = 1 };
            _mockUsuarioService.Setup(x => x.GetUsuarioByID(1)).Throws(new Exception());
            
            var result = _service.CreateNewProject(projetoDTO);
            
            Assert.Null(result);
        }

        [Fact]
        public void DeleteProject_QuandoProjetoNaoExiste_RetornaFalso()
        {
            _mockProjetoRepo.Setup(x => x.GetByID(1, true)).Returns((Projeto)null);
            
            var (success, message) = _service.DeleteProject(1);
            
            Assert.False(success);
            Assert.Equal("Projeto Invalido!", message);
        }

        [Fact]
        public void DeleteProject_QuandoTarefasPendentes_RetornaFalso()
        {            
            var projeto = new Projeto
            {
                Tarefas = new List<Tarefa>
                {
                    new Tarefa { StatusTarefa = (int)StatusTarefaEnum.Pendente }
                }
            };

            _mockProjetoRepo.Setup(x => x.GetByID(1, true)).Returns(projeto);
            
            var (success, message) = _service.DeleteProject(1);
            
            Assert.False(success);
            Assert.Equal("Projeto com tarefas Pendentes!", message);
        }

        [Fact]
        public void DeleteProject_QuandoSemTarefasPendentes_RetornaVerdadeiro()
        {
            var projeto = new Projeto
            {
                Tarefas = new List<Tarefa>
                {
                    new Tarefa { StatusTarefa = (int)StatusTarefaEnum.Concluida }
                }
            };
            _mockProjetoRepo.Setup(x => x.GetByID(1, true)).Returns(projeto);
            _mockProjetoRepo.Setup(x => x.Delete(projeto));
            
            var (success, message) = _service.DeleteProject(1);
            
            Assert.True(success);
            Assert.Equal("", message);
        }

        [Fact]
        public void DeleteProject_QuandoExcecao_RetornaFalso()
        {            
            var projeto = new Projeto { Tarefas = new List<Tarefa>() };
            _mockProjetoRepo.Setup(x => x.GetByID(1, true)).Returns(projeto);
            _mockProjetoRepo.Setup(x => x.Delete(projeto)).Throws(new Exception());
            
            var (success, message) = _service.DeleteProject(1);
            
            Assert.False(success);
            Assert.Equal("", message);
        }

        [Fact]
        public void GetProjectByID_QuandoExiste_RetornaViewModel()
        {
            var projeto = new Projeto { ID = 1, Nome = "projeto teste" };
            _mockProjetoRepo.Setup(x => x.GetByID(1, false)).Returns(projeto);
            
            var result = _service.GetProjectByID(1);
            
            Assert.NotNull(result);
            Assert.Equal(1, result.ID);
        }

        [Fact]
        public void GetProjectByID_QuandoNaoExiste_RetornaNull()
        {            
            _mockProjetoRepo.Setup(x => x.GetByID(1, false)).Returns((Projeto)null);
            
            var result = _service.GetProjectByID(1);
            
            Assert.Null(result);
        }

        [Fact]
        public void GetProjectsByUserID_QuandoExistem_RetornaListaViewModels()
        {            
            var projetos = new List<Projeto>
            {
                new Projeto { ID = 1, Nome = "Projeto 1" },
                new Projeto { ID = 2, Nome = "Projeto 2" }
            };
            _mockProjetoRepo.Setup(x => x.GetAll(1)).Returns(projetos);

            var result = _service.GetProjectsByUserID(1);
            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Projeto 1", result[0].Nome);
            Assert.Equal("Projeto 2", result[1].Nome);
        }

        [Fact]
        public void GetProjectsByUserID_QuandoNaoExiste_RetornaNull()
        {            
            _mockProjetoRepo.Setup(x => x.GetAll(1)).Returns(new List<Projeto>());
            
            var result = _service.GetProjectsByUserID(1);
   
            Assert.Null(result);
        }
    }
}
