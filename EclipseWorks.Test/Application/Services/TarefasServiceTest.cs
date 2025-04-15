using EclipseWorksChallange.Application.DTOs.Create;
using EclipseWorksChallange.Application.DTOs.Update;
using EclipseWorksChallange.Application.DTOs.View;
using EclipseWorksChallange.Application.Services;
using EclipseWorksChallange.Core.Enums;
using EclipseWorksChallange.Core.Models;
using EclipseWorksChallange.Infraestructure.Persistence.Repositories;
using Moq;

namespace EclipseWorks.Test.Application.Services
{
    public class TarefasServiceTest
    {
        private readonly Mock<ITarefasRepository> _mockTarefasRepo;
        private readonly Mock<IUsuarioService> _mockUsuarioService;
        private readonly Mock<IProjetosService> _mockProjetosService;
        private readonly Mock<IHistoricoTarefaService> _mockHistoricoService;
        private readonly TarefasService _service;

        public TarefasServiceTest()
        {
            _mockTarefasRepo = new Mock<ITarefasRepository>();
            _mockUsuarioService = new Mock<IUsuarioService>();
            _mockProjetosService = new Mock<IProjetosService>();
            _mockHistoricoService = new Mock<IHistoricoTarefaService>();
            _service = new TarefasService(
                _mockTarefasRepo.Object,
                _mockProjetosService.Object,
                _mockUsuarioService.Object,
                _mockHistoricoService.Object);
        }

        [Fact]
        public void CreateNewTask_QuandoUsuarioNull_RetornarErro()
        {
            var tarefaDTO = new CriarTarefaDTO { UsuarioResponsavelId = 1, ProjetoId = 1 };
            _mockUsuarioService.Setup(x => x.GetUsuarioByID(1)).Returns((UsuarioViewModel)null);

            var (result, error) = _service.CreateNewTask(tarefaDTO);

            Assert.Null(result);
            Assert.Equal("Usuario Invalido!", error);
        }

        [Fact]
        public void CreateNewTask_QuandoProjetoNull_RetornarErro()
        {
            var tarefaDTO = new CriarTarefaDTO { UsuarioResponsavelId = 1, ProjetoId = 1 };
            var usuario = new Usuario { ID = 1, Nome = "teste" };

            _mockUsuarioService.Setup(x => x.GetUsuarioByID(1)).Returns(new UsuarioViewModel(usuario));
            _mockProjetosService.Setup(x => x.GetProjectByID(1)).Returns((ProjetoViewModel)null);

            var (result, error) = _service.CreateNewTask(tarefaDTO);

            Assert.Null(result);
            Assert.Equal("Projeto Invalido", error);
        }

        [Fact]
        public void CreateNewTask_QuandoLimiteTarefasAtingido_RetornarErro()
        {
            var tarefaDTO = new CriarTarefaDTO { UsuarioResponsavelId = 1, ProjetoId = 1 };


            var tarefas = new List<Tarefa>();

            for (int i = 0; i < 20; i++)
            {
                tarefas.Add(new Tarefa { Id = i, Nome = $"tarefa{i}" });
            }
            var projeto = new Projeto { Tarefas = tarefas };

            var projetoVM = new ProjetoViewModel(projeto);
            var usuario = new Usuario { ID = 1, Nome = "teste" };

            _mockUsuarioService.Setup(x => x.GetUsuarioByID(1)).Returns(new UsuarioViewModel(usuario));
            _mockProjetosService.Setup(x => x.GetProjectByID(1)).Returns(projetoVM);

            var (result, error) = _service.CreateNewTask(tarefaDTO);

            Assert.Null(result);
            Assert.Equal("Projeto atingiu limite maximo de Tarefas!", error);
        }

        [Fact]
        public void CreateNewTask_QuandoDadosOK_CriarTarefa()
        {
            var tarefaDTO = new CriarTarefaDTO
            {
                UsuarioResponsavelId = 1,
                ProjetoId = 1,
                Nome = "teste",
                Descricao = "descricao",
                DataVencimento = DateTime.Now.AddDays(7),
                Prioridade = 1
            };
            var usuario = new Usuario { ID = 1, Nome = "teste" };
            var projeto = new Projeto { ID = 1, Nome = "teste" };

            _mockUsuarioService.Setup(x => x.GetUsuarioByID(1)).Returns(new UsuarioViewModel(usuario));
            _mockProjetosService.Setup(x => x.GetProjectByID(1)).Returns(new ProjetoViewModel(projeto));
            _mockTarefasRepo.Setup(x => x.Add(It.IsAny<Tarefa>()));


            var (result, error) = _service.CreateNewTask(tarefaDTO);

            Assert.NotNull(result);
            Assert.Equal("", error);
            Assert.Equal("teste", result.Nome);
        }

        [Fact]
        public void DeleteTaskById_QuandoTarefaNaoExiste_RetornarFalse()
        {
            _mockTarefasRepo.Setup(x => x.GetById(1)).Returns((Tarefa)null);

            var result = _service.DeleteTaskById(1);

            Assert.False(result);
        }

        [Fact]
        public void DeleteTaskById_QuandoTarefaComHistorico_Deletar()
        {
            var historicos = new List<HistoricoTarefa> { new HistoricoTarefa() };
            var tarefa = new Tarefa { Id = 1, Historicos = historicos };

            _mockTarefasRepo.Setup(x => x.GetById(1)).Returns(tarefa);
            _mockTarefasRepo.Setup(x => x.Delete(tarefa));
            _mockHistoricoService.Setup(x => x.Delete(historicos));

            var result = _service.DeleteTaskById(1);

            Assert.True(result);
        }

        [Fact]
        public void GetTasksByProjectID_RetornaListaDeTarefas()
        {

            var tarefas = new List<Tarefa>
            {
                new Tarefa { Id = 1, Nome = "Tarefa 1" },
                new Tarefa { Id = 2, Nome = "Tarefa 2" }
            };
            _mockTarefasRepo.Setup(x => x.GetAllByProject(1)).Returns(tarefas);

            var result = _service.GetTasksByProjectID(1);

            Assert.Equal(2, result.Count);
            Assert.Equal("Tarefa 1", result[0].Nome);
            Assert.Equal("Tarefa 2", result[1].Nome);
        }

        [Fact]
        public void UpdateTask_QuandoTarefaNaoExiste_RetornaNull()
        {
            var tarefaDTO = new AtualizarTarefaDTO { Id = 1 };
            _mockTarefasRepo.Setup(x => x.GetById(1)).Returns((Tarefa)null);

            var result = _service.UpdateTask(tarefaDTO);

            Assert.Null(result);
        }

        [Fact]
        public void UpdateTask_QuandoUsuarioInvalido_RetornaNull()
        {
            var tarefaDTO = new AtualizarTarefaDTO { Id = 1, UsuarioAlteracaoId = 1 };
            _mockTarefasRepo.Setup(x => x.GetById(1)).Returns(new Tarefa());
            _mockUsuarioService.Setup(x => x.GetUsuarioByID(1)).Returns((UsuarioViewModel)null);

            var result = _service.UpdateTask(tarefaDTO);

            Assert.Null(result);
        }

        [Fact]
        public void UpdateTask_QuandoStatusConcluido_AtualizarDataConclusao()
        {
            var tarefaDTO = new AtualizarTarefaDTO
            {
                Id = 1,
                UsuarioAlteracaoId = 1,
                StatusTarefa = (int)StatusTarefaEnum.Concluida
            };
            var usuario = new Usuario { ID = 1, Nome = "teste" };

            var tarefaExistente = new Tarefa { Id = 1 };

            _mockUsuarioService.Setup(x => x.GetUsuarioByID(1)).Returns(new UsuarioViewModel(usuario));
            _mockTarefasRepo.Setup(x => x.GetById(1)).Returns(tarefaExistente);
            _mockTarefasRepo.Setup(x => x.Update(It.IsAny<Tarefa>())).Returns(tarefaExistente);

            var result = _service.UpdateTask(tarefaDTO);

            Assert.NotNull(result);
            Assert.NotNull(tarefaExistente.DataConclusao);
        }

        [Fact]
        public void UpdateTask_QuandoNaoHaAlteracoes_NaoRegistrarHistorico()
        {
            var tarefaDTO = new AtualizarTarefaDTO
            {
                Id = 1,
                UsuarioAlteracaoId = 1,
                Nome = "Tarefa Existente",
                StatusTarefa = 0
            };

            var tarefaExistente = new Tarefa
            {
                Id = 1,
                Nome = "Tarefa Existente",
                StatusTarefa = 0
            };
            var usuario = new Usuario { ID = 1, Nome = "teste" };

            _mockUsuarioService.Setup(x => x.GetUsuarioByID(1)).Returns(new UsuarioViewModel(usuario));
            _mockTarefasRepo.Setup(x => x.GetById(1)).Returns(tarefaExistente);

            var result = _service.UpdateTask(tarefaDTO);

            Assert.NotNull(result);
        }
    }
}
