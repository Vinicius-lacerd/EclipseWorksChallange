using EclipseWorksChallange.Application.DTOs.Create;
using EclipseWorksChallange.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorksChallange.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProjetosController : Controller
    {
        private readonly IProjetosService _projetosService;

        public ProjetosController(IProjetosService projetosService)
        {
            _projetosService = projetosService;
        }

        [HttpGet("{userId}")]
        public ActionResult GetProjectsByUserID(int userId)
        {
            try
            {
                var response = _projetosService.GetProjectsByUserID(userId);

                if (response == null) return NotFound();

                return Ok(response);
            }
            catch (Exception)
            {
                // log
                return StatusCode(500);
            }
        }

        [HttpPost]
        public ActionResult Create([FromBody] CriarProjetoDTO projetoDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _projetosService.CreateNewProject(projetoDTO);

                if (response == null) return BadRequest();

                return Ok(response);
            }
            catch (Exception)
            {
                // log
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var response = _projetosService.DeleteProject(id);

                if (response.Item1)
                    return Ok();
                else
                    return BadRequest(response.Item2);
            }
            catch (Exception)
            {
                // log
                return StatusCode(500);
            }
        }
    }
}
