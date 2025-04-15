using EclipseWorksChallange.Application.DTOs.Create;
using EclipseWorksChallange.Application.DTOs.Update;
using EclipseWorksChallange.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorksChallange.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefasController : Controller
    {

        private readonly ITarefasService _tarefasService;
        private readonly IHistoricoTarefaService _historicotarefasService;

        public TarefasController(ITarefasService tarefasService, IHistoricoTarefaService historicotarefasService)
        {
            _tarefasService = tarefasService;
            _historicotarefasService = historicotarefasService;
        }
        [HttpGet("{projectID}")]
        public ActionResult GetTasksByProjectID(int projectID)
        {
            var tarefas = _tarefasService.GetTasksByProjectID(projectID);

            if (tarefas is null) return NotFound();

            return Ok(tarefas);
        }

        [HttpPost]
        public ActionResult Create([FromBody] CriarTarefaDTO tarefaDTO)
        {
            try
            {
                var response = _tarefasService.CreateNewTask(tarefaDTO);

                if(response.Item1 is null)
                    return BadRequest(response.Item2);

                return Ok(response);
            }
            catch (Exception)
            {
                // log
                return StatusCode(500);
            }
        }

        [HttpPut]
        public ActionResult AtualizaTarefa([FromBody] AtualizarTarefaDTO tarefaDTO)
        {
            try
            {
                var response = _tarefasService.UpdateTask(tarefaDTO);

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
        public ActionResult DeleteTarefa(int id)
        {
            try
            {
                var response = _tarefasService.DeleteTaskById(id);

                if (response)
                    return Ok(response);
                else
                    return BadRequest(500);
            }
            catch (Exception)
            {
                // log
                return StatusCode(500);
            }
        }

        [HttpPost("Comentarios")]
        public ActionResult CreateComentario([FromBody] CriarComentarioDTO comentarioDTO)
        {
            try
            {
                var response = _historicotarefasService.CreateNewComment(comentarioDTO);

                if (!response)
                    return BadRequest();

                return Ok();
            }
            catch (Exception)
            {
                // log
                return StatusCode(500);
            }
        }
    }
}