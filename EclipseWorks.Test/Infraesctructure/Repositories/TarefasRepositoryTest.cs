using EclipseWorksChallange.Core.Models;
using EclipseWorksChallange.Infraestructure.Persistence;
using EclipseWorksChallange.Infraestructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EclipseWorks.Test.Infraesctructure.Repositories
{
    public class TarefasRepositoryTest
    {
        private readonly Mock<EclipseWorksSQLServerContext> _mockContext;
        private readonly TarefasRepository _repository;
        private readonly Mock<DbSet<Tarefa>> _mockTarefaSet;

        public TarefasRepositoryTest()
        {
            _mockContext = new Mock<EclipseWorksSQLServerContext>();
            _mockTarefaSet = new Mock<DbSet<Tarefa>>();
            _repository = new TarefasRepository(_mockContext.Object);
        }

        [Fact]
        public void GetById_RetornarTarefa()
        {            
            var tarefaId = 1;
            var tarefaMock = new Tarefa { Id = tarefaId, Nome = "teste" };

            var data = new List<Tarefa> { tarefaMock }.AsQueryable();

            SetupMockDbSet(_mockTarefaSet, data);
            _mockContext.Setup(x => x.Tarefa).Returns(_mockTarefaSet.Object);
            
            var result = _repository.GetById(tarefaId);
            
            Assert.NotNull(result);
            Assert.Equal(tarefaId, result.Id);
        }

        [Fact]
        public void GetById_IncluirHistoricos()
        {            
            var tarefaId = 1;
            var tarefaMock = new Tarefa
            {
                Id = tarefaId,
                Historicos = new List<HistoricoTarefa> { new HistoricoTarefa() }
            };

            var data = new List<Tarefa> { tarefaMock }.AsQueryable();

            SetupMockDbSet(_mockTarefaSet, data);
            _mockContext.Setup(x => x.Tarefa).Returns(_mockTarefaSet.Object);
            
            var result = _repository.GetById(tarefaId);
            
            Assert.NotNull(result.Historicos);
        }

        [Fact]
        public void GetById_QuandoNaoExiste_RetornaNull()
        {            
            var tarefaId = 99;
            var data = new List<Tarefa>().AsQueryable();

            SetupMockDbSet(_mockTarefaSet, data);
            _mockContext.Setup(x => x.Tarefa).Returns(_mockTarefaSet.Object);
            
            var result = _repository.GetById(tarefaId);            
            Assert.Null(result);
        }

        [Fact]
        public void GetAllByProject_RetornarTarefasProjetoPorId()
        {            
            var projetoId = 1;
            var data = new List<Tarefa>
            {
            new Tarefa { Id = 1, ProjetoId = projetoId },
                new Tarefa { Id = 2, ProjetoId = projetoId },
                new Tarefa { Id = 3, ProjetoId = 2 }
            }.AsQueryable();

            SetupMockDbSet(_mockTarefaSet, data);
            _mockContext.Setup(x => x.Tarefa).Returns(_mockTarefaSet.Object);
            
            var result = _repository.GetAllByProject(projetoId);
            
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetAllByProject_IncluirProjetoUsuarioHistorico()
        {
            var projetoId = 1;
            var tarefaMock = new Tarefa
            {
                Id = 1,
                ProjetoId = projetoId,
                Projeto = new Projeto(),
                UsuarioResponsavel = new Usuario(),
                Historicos = new List<HistoricoTarefa>()
            };

            var data = new List<Tarefa> { tarefaMock }.AsQueryable();

            SetupMockDbSet(_mockTarefaSet, data);
            _mockContext.Setup(x => x.Tarefa).Returns(_mockTarefaSet.Object);            
            var result = _repository.GetAllByProject(projetoId);
            
            Assert.NotNull(result.First().Projeto);
            Assert.NotNull(result.First().UsuarioResponsavel);
            Assert.NotNull(result.First().Historicos);
        }

        [Fact]
        public void Add_AdicionarTarefaEChamarSaveChanges()
        {
            var tarefa = new Tarefa { Id = 1 };
            _mockContext.Setup(x => x.Tarefa.Add(tarefa));
            _mockContext.Setup(x => x.SaveChanges()).Returns(1);
            _repository.Add(tarefa);
            
            _mockContext.Verify(x => x.Tarefa.Add(tarefa), Times.Once());
            _mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        [Fact]
        public void Delete_QuandoSucesso_RetornaTrue()
        {            
            var tarefa = new Tarefa { Id = 1 };
            _mockContext.Setup(x => x.Tarefa.Remove(tarefa));
            
            var result = _repository.Delete(tarefa);

            Assert.True(result);
        }

        [Fact]
        public void Delete_QuandoErro_RetornaFalse()
        {
            var tarefa = new Tarefa { Id = 1 };
            _mockContext.Setup(x => x.Tarefa.Remove(tarefa));
            _mockContext.Setup(x => x.SaveChanges()).Throws(new DbUpdateException());
            
            var result = _repository.Delete(tarefa);
            
            Assert.False(result);
        }

        [Fact]
        public void Update_QuandoSucesso_RetornaTarefa()
        {            
            var tarefa = new Tarefa { Id = 1, Nome = "tarefa" };
            _mockContext.Setup(x => x.Update(tarefa));
            _mockContext.Setup(x => x.SaveChanges()).Returns(1);
            
            var result = _repository.Update(tarefa);
            
            Assert.Equal(tarefa, result);
        }

        private void SetupMockDbSet<T>(Mock<DbSet<T>> mockSet, IQueryable<T> data) where T : class
        {
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }
    }
}
