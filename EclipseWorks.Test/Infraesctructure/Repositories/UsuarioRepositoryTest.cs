using EclipseWorksChallange.Core.Models;
using EclipseWorksChallange.Infraestructure.Persistence;
using EclipseWorksChallange.Infraestructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EclipseWorks.Test.Infraesctructure.Repositories
{
    public class UsuarioRepositoryTest
    {
        private readonly Mock<EclipseWorksSQLServerContext> _mockContext;
        private readonly UsuarioRepository _repository;
        private readonly Mock<DbSet<Usuario>> _mockUsuarioSet;
        private readonly Mock<DbSet<Tarefa>> _mockTarefaSet;

        public UsuarioRepositoryTest()
        {
            _mockContext = new Mock<EclipseWorksSQLServerContext>();
            _mockUsuarioSet = new Mock<DbSet<Usuario>>();
            _mockTarefaSet = new Mock<DbSet<Tarefa>>();
            _repository = new UsuarioRepository(_mockContext.Object);
        }

        [Fact]
        public void GetByID_RetornarUsuario()
        {           
            var usuarioId = 1;
            var usuarioMock = new Usuario { ID = usuarioId, Nome = "usurio teste" };

            var data = new List<Usuario> { usuarioMock }.AsQueryable();

            SetupMockDbSet(_mockUsuarioSet, data);
            _mockContext.Setup(x => x.Usuario).Returns(_mockUsuarioSet.Object);
              
            var result = _repository.GetByID(usuarioId);
            
            Assert.NotNull(result);
            Assert.Equal(usuarioId, result.ID);
        }


        [Fact]
        public void GetByID_QuandoNaoExiste_RetornaNull()
        {
            var usuarioId = 99;
            var data = new List<Usuario>().AsQueryable();

            SetupMockDbSet(_mockUsuarioSet, data);
            _mockContext.Setup(x => x.Usuario).Returns(_mockUsuarioSet.Object);
            
            var result = _repository.GetByID(usuarioId);            
            Assert.Null(result);
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
