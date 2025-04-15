using EclipseWorksChallange.Core.Models;
using EclipseWorksChallange.Infraestructure.Persistence;
using EclipseWorksChallange.Infraestructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EclipseWorks.Test.Infraesctructure.Repositories
{
    public class ProjetosRepositoryTest
    {
        private readonly Mock<EclipseWorksSQLServerContext> _mockContext;
        private readonly ProjetosRepository _repository;
        private readonly Mock<DbSet<Projeto>> _mockProjetoSet;

        public ProjetosRepositoryTest()
        {
            _mockContext = new Mock<EclipseWorksSQLServerContext>();
            _mockProjetoSet = new Mock<DbSet<Projeto>>();
            _repository = new ProjetosRepository(_mockContext.Object);
        }

        [Fact]
        public void GetByID_RetornarProjeto()
        {            
            var projetoId = 1;
            var projetoMock = new Projeto { ID = projetoId, Nome = "teste" };

            var data = new List<Projeto> { projetoMock }.AsQueryable();

            SetupMockDbSet(_mockProjetoSet, data);
            _mockContext.Setup(x => x.Projeto).Returns(_mockProjetoSet.Object);
            
            var result = _repository.GetByID(projetoId);
            
            Assert.NotNull(result);
            Assert.Equal(projetoId, result.ID);
        }


        [Fact]
        public void GetByID_QuandoNaoExiste_RetornaNull()
        {            
            var projetoId = 99;
            var data = new List<Projeto>().AsQueryable();

            SetupMockDbSet(_mockProjetoSet, data);
            _mockContext.Setup(x => x.Projeto).Returns(_mockProjetoSet.Object);
            
            var result = _repository.GetByID(projetoId);
            
            Assert.Null(result);
        }

        [Fact]
        public void GetAll_RetornarProjetosPorUsuario()
        {            
            var userId = 1;
            var data = new List<Projeto>
            {
                new Projeto { ID = 1, UsuarioId = userId },
                new Projeto { ID = 2, UsuarioId = userId },
                new Projeto { ID = 3, UsuarioId = 2 }
            }.AsQueryable();

            SetupMockDbSet(_mockProjetoSet, data);
            _mockContext.Setup(x => x.Projeto).Returns(_mockProjetoSet.Object);
            
            var result = _repository.GetAll(userId);
            
            Assert.Equal(2, result.Count);
            Assert.All(result, p => Assert.Equal(userId, p.UsuarioId));
        }

        [Fact]
        public void GetAll_QuandoNaoExisteProjeto_RetornaListaVazia()
        {
            var userId = 99;
            var data = new List<Projeto>().AsQueryable();

            SetupMockDbSet(_mockProjetoSet, data);
            _mockContext.Setup(x => x.Projeto).Returns(_mockProjetoSet.Object);
            
            var result = _repository.GetAll(userId);            
            Assert.Empty(result);
        }

        [Fact]
        public void Add_AdicionarProjetoEChamarSaveChanges()
        {            
            var projeto = new Projeto { ID = 1 };
            _mockContext.Setup(x => x.Projeto.Add(projeto));
            _mockContext.Setup(x => x.SaveChanges()).Returns(1);
            
            _repository.Add(projeto);
            
            _mockContext.Verify(x => x.Projeto.Add(projeto), Times.Once());
            _mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        [Fact]
        public void Delete_RemoverProjetoEChamarSaveChanges()
        {
            var projeto = new Projeto { ID = 1 };
            _mockContext.Setup(x => x.Projeto.Remove(projeto));
            _mockContext.Setup(x => x.SaveChanges()).Returns(1);
            
            _repository.Delete(projeto);
            
            _mockContext.Verify(x => x.Projeto.Remove(projeto), Times.Once());
            _mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        [Fact]
        public void Delete_QuandoSaveChangesFalhar_LancarExcecao()
        {

            var projeto = new Projeto { ID = 1 };
            _mockContext.Setup(x => x.Projeto.Remove(projeto));
            _mockContext.Setup(x => x.SaveChanges()).Throws(new DbUpdateException());

            Assert.Throws<DbUpdateException>(() => _repository.Delete(projeto));
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
