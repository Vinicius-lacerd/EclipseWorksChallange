using EclipseWorksChallange.Application.DTOs.Create;
using EclipseWorksChallange.Application.DTOs.Update;
using EclipseWorksChallange.Application.DTOs.View;
using EclipseWorksChallange.Application.Services;
using EclipseWorksChallange.Core.Models;
using EclipseWorksChallange.Infraestructure.Persistence.Repositories;
using Moq;

namespace EclipseWorks.Test.Application.Services
{
    public class HistoricoTarefaServiceTest
    {
        private readonly Mock<IHistoricoTarefaRepository> _mockHistoricoRepo;
        private readonly Mock<IUsuarioService> _mockUsuarioService;
        private readonly Mock<ITarefasRepository> _mockTarefasRepo;
        private readonly HistoricoTarefaService _service;

        public HistoricoTarefaServiceTest()
        {
            _mockHistoricoRepo = new Mock<IHistoricoTarefaRepository>();
            _mockUsuarioService = new Mock<IUsuarioService>();
            _mockTarefasRepo = new Mock<ITarefasRepository>();
            _service = new HistoricoTarefaService(
                _mockHistoricoRepo.Object,
                _mockUsuarioService.Object,
                _mockTarefasRepo.Object);
        }

        [Fact]
        public void CreateNewComment_QuandoTarefaNaoExste_RetornaFalse()
        {
            
            var comentarioDTO = new CriarComentarioDTO { TarefaId = 1, UsuarioId = 1 };
            _mockTarefasRepo.Setup(x => x.GetById(1)).Returns((Tarefa)null);
            
            var result = _service.CreateNewComment(comentarioDTO);
            
            Assert.False(result);
        }

        [Fact]
        public void CreateNewComment_QuandoUsuarioNaoExiste_RetornaFalse()
        {
            
            var comentarioDTO = new CriarComentarioDTO { TarefaId = 1, UsuarioId = 1 };
            _mockTarefasRepo.Setup(x => x.GetById(1)).Returns(new Tarefa());
            _mockUsuarioService.Setup(x => x.GetUsuarioByID(1)).Returns((UsuarioViewModel)null);
            
            var result = _service.CreateNewComment(comentarioDTO);

            Assert.False(result);
        }

        [Fact]
        public void CreateNewComment_QuandoTarefaEUsuarioExistem_CriaHistoricoERetornaTrue()
        {            
            var comentarioDTO = new CriarComentarioDTO
            {
                TarefaId = 1,
                UsuarioId = 1,
                Comentario = "teste"
            };

            var usuario = new Usuario()
            {
                ID = 1,
                Nome = "teste"
            };

            _mockTarefasRepo.Setup(x => x.GetById(1)).Returns(new Tarefa());
            _mockUsuarioService.Setup(x => x.GetUsuarioByID(1)).Returns(new UsuarioViewModel(usuario));
            _mockHistoricoRepo.Setup(x => x.Add(It.IsAny<HistoricoTarefa>()));

            
            var result = _service.CreateNewComment(comentarioDTO);

            
            Assert.True(result);
            _mockHistoricoRepo.Verify(x => x.Add(It.Is<HistoricoTarefa>(h =>
                h.TarefaId == 1 &&
                h.UsuarioId == 1 &&
                h.Comentario == "teste")), Times.Once);
        }

        [Fact]
        public void Delete_ChamarRepositorioDelete()
        {
            
            var historicos = new List<HistoricoTarefa>
        {
            new HistoricoTarefa { Id = 1 },
            new HistoricoTarefa { Id = 2 }
        };

            
            _service.Delete(historicos);

            
            _mockHistoricoRepo.Verify(x => x.Delete(historicos), Times.Once);
        }

        [Fact]
        public void RegistrarHistorico_AdicionarUmHistoricoParaCadaAtualizacao()
        {
            
            var tarefaDTO = new AtualizarTarefaDTO
            {
                Id = 1,
                UsuarioAlteracaoId = 1
            };

            var atualizacoes = new List<AtualizaHistoricoTarefaDTO>
        {
            new AtualizaHistoricoTarefaDTO
            {
                CampoAlterado = "Status",
                Comentario = "Status alterado",
                NovoValor = "2",
                ValorAnterior = "1"
            },
            new AtualizaHistoricoTarefaDTO
            {
                CampoAlterado = "Prioridade",
                Comentario = "Prioridade alterada",
                NovoValor = "Alta",
                ValorAnterior = "Média"
            }
        };

            _mockHistoricoRepo.Setup(x => x.Add(It.IsAny<HistoricoTarefa>()));

            
            _service.RegistrarHistorico(tarefaDTO, atualizacoes);

            
            _mockHistoricoRepo.Verify(x => x.Add(It.Is<HistoricoTarefa>(h =>
                h.TarefaId == 1 &&
                h.UsuarioId == 1 &&
                h.CampoAlterado == "Status" &&
                h.NovoValor == "2")), Times.Once);

            _mockHistoricoRepo.Verify(x => x.Add(It.Is<HistoricoTarefa>(h =>
                h.TarefaId == 1 &&
                h.UsuarioId == 1 &&
                h.CampoAlterado == "Prioridade" &&
                h.NovoValor == "Alta")), Times.Once);

            _mockHistoricoRepo.Verify(x => x.Add(It.IsAny<HistoricoTarefa>()), Times.Exactly(2));
        }

        [Fact]
        public void RegistrarHistorico_QuandoListaVazia_NaoAdicionarNada()
        {
            
            var tarefaDTO = new AtualizarTarefaDTO { Id = 1 };
            var atualizacoes = new List<AtualizaHistoricoTarefaDTO>();

            
            _service.RegistrarHistorico(tarefaDTO, atualizacoes);

            
            _mockHistoricoRepo.Verify(x => x.Add(It.IsAny<HistoricoTarefa>()), Times.Never);
        }
    }
}
