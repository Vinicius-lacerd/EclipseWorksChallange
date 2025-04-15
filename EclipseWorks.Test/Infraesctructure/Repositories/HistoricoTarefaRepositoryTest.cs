using EclipseWorksChallange.Core.Models;
using EclipseWorksChallange.Infraestructure.Persistence;
using EclipseWorksChallange.Infraestructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EclipseWorks.Test.Infraesctructure.Repositories
{
    public class HistoricoTarefaRepositoryTest
    {
        private readonly Mock<EclipseWorksSQLServerContext> _mockContext;
        private readonly HistoricoTarefaRepository _repository;
        private readonly Mock<DbSet<HistoricoTarefa>> _mockSet;

        public HistoricoTarefaRepositoryTest()
        {
            _mockContext = new Mock<EclipseWorksSQLServerContext>();
            _mockSet = new Mock<DbSet<HistoricoTarefa>>();

            var data = new List<HistoricoTarefa>().AsQueryable();
            SetupMockDbSet(_mockSet, data);

            _mockContext.Setup(x => x.HistoricoTarefa).Returns(_mockSet.Object);

            _repository = new HistoricoTarefaRepository(_mockContext.Object);
        }

        [Fact]
        public void Add_AdicionarHistoricoEChamarSaveChanges()
        {            
            var historico = new HistoricoTarefa { Id = 1, TarefaId = 1, Comentario = "teste" };
            _mockContext.Setup(x => x.HistoricoTarefa.Add(historico));
            _mockContext.Setup(x => x.SaveChanges()).Returns(1);            
            _repository.Add(historico);
            
            _mockContext.Verify(x => x.HistoricoTarefa.Add(historico), Times.Once());
            _mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        [Fact]
        public void Delete_RemoverTodosHistoricosEChamarSaveChanges()
        {
            var historicos = new List<HistoricoTarefa>
            {
                new HistoricoTarefa { Id = 1 },
                new HistoricoTarefa { Id = 2 }
            };

            _mockContext.Setup(x => x.HistoricoTarefa).Returns(_mockSet.Object);
            _mockContext.Setup(x => x.SaveChanges()).Returns(1);            
            _repository.Delete(historicos);
            
            foreach (var historico in historicos)
            {
                _mockSet.Verify(x => x.Remove(historico), Times.Once());
            }
            _mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        [Fact]
        public void GetAll_RetornarHistoricosDaTarefa()
        {            
            var tarefaId = 1;
            var data = new List<HistoricoTarefa>
            {
                new HistoricoTarefa { Id = 1, TarefaId = 1 },
                new HistoricoTarefa { Id = 2, TarefaId = 1 },
                new HistoricoTarefa { Id = 3, TarefaId = 2 }
            }.AsQueryable();

            SetupMockDbSet(_mockSet, data);
            _mockContext.Setup(x => x.HistoricoTarefa).Returns(_mockSet.Object);            
            var result = _repository.GetAll(tarefaId);
            
            Assert.Equal(2, result.Count);
            Assert.All(result, x => Assert.Equal(tarefaId, x.TarefaId));
        }

        [Fact]
        public void GetAll_QuandoNaoExisteHistorico_RetornaListaVazia()
        {
            var tarefaId = 99;
            var data = new List<HistoricoTarefa>().AsQueryable();

            //_mockSet.As<IQueryable<HistoricoTarefa>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            SetupMockDbSet(_mockSet, data);
            _mockContext.Setup(x => x.HistoricoTarefa).Returns(_mockSet.Object);
            
            var result = _repository.GetAll(tarefaId);
            
            Assert.Empty(result);
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
