using EclipseWorksChallange.Application.DTOs.View;
using EclipseWorksChallange.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorksChallange.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : Controller
    {
        private readonly IRelatoriosService _relatorioServic;
        public RelatoriosController(IRelatoriosService relatoriosService)
        {
            _relatorioServic = relatoriosService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<RelatorioMediaTarefasConcluidasViewModel>> GetMediaTarefasConcluidas(int userId)
        {
            try
            {
                var dataLimite = DateTime.Now.AddDays(-30);

                var resultado = await _relatorioServic.MediaTarefasConcluidasUsuarios(userId ,dataLimite);

                if (resultado == null) return BadRequest();

                return Ok(resultado);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao processar o relatório");
            }
        }

    }
}
