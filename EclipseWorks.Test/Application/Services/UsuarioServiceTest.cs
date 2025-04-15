using EclipseWorksChallange.Application.DTOs.View;
using EclipseWorksChallange.Application.Services;
using EclipseWorksChallange.Core.Models;
using EclipseWorksChallange.Infraestructure.Persistence.Repositories;
using Moq;

namespace EclipseWorks.Test.Application.Services
{
    public class UsuarioServiceTest
    {
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepo;
        private readonly UsuarioService _service;

        public UsuarioServiceTest()
        {
            _mockUsuarioRepo = new Mock<IUsuarioRepository>();
            _service = new UsuarioService(_mockUsuarioRepo.Object);
        }

        [Fact]
        public void GetUsuarioByID_QuandoUsuarioExiste_RetornaViewModel()
        {            
            var usuario = new Usuario
            {
                ID = 1,
                Nome = "teste",
                NivelID = 1
            };

            _mockUsuarioRepo.Setup(x => x.GetByID(1)).Returns(usuario);
            
            var result = _service.GetUsuarioByID(1);
            
            Assert.NotNull(result);
            Assert.Equal(1, result.ID);
            Assert.Equal("teste", result.Nome);
            Assert.Equal(1, result.NivelID);
        }

        [Fact]
        public void GetUsuarioByID_QuandoUsuarioNaoExiste_RetornaNull()
        {            
            _mockUsuarioRepo.Setup(x => x.GetByID(1)).Returns((Usuario)null);
            
            var result = _service.GetUsuarioByID(1);
            
            Assert.Null(result);
        }

        [Fact]
        public void ViewModelToUsuario_MapearCorretamente()
        {            
            var usuario = new Usuario {
                ID = 1,
                Nome = "teste",
                NivelID = 2,
                Projetos = new List<Projeto>()
            };

            var viewModel = new UsuarioViewModel(usuario);

            
            var result = _service.ViewModelToUsuario(viewModel);
            
            Assert.Equal(1, result.ID);
            Assert.Equal("teste", result.Nome);
            Assert.Equal(2, result.NivelID);
            Assert.NotNull(result.Projetos);
        }

        [Fact]
        public void ViewModelToUsuario_QuandoViewModelNull_LancarExcecao()
        {
            UsuarioViewModel viewModel = null;

            Assert.Throws<ArgumentNullException>(() => _service.ViewModelToUsuario(viewModel));
        }
    }
}
