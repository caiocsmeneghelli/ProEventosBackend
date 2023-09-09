using System.Diagnostics.Tracing;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;

namespace ProEventos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _eventoService;

        public EventoController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var evento = await _eventoService.GetAllEventosAsync(true);
                if (evento is null)
                    return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    string.Format("Erro ao tentar recuperar eventos. Erro: ", ex.Message));
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(id, true);
                if (evento is null)
                    return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    string.Format("Erro ao tentar recuperar eventos. Erro: ", ex.Message));
            }
        }

        [HttpGet]
        [Route("tema/{tema}")]
        public async Task<IActionResult> GetAllByTema(string tema)
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosByTemaAsync(tema, true);
                if (eventos is null)
                    return NoContent();

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    string.Format("Erro ao tentar recuperar eventos. Erro: ", ex.Message));
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = await _eventoService.AddEvento(model);
                if (evento is null)
                    return BadRequest("Erro ao tentar adicionar evento.");
                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    string.Format("Erro ao tentar adicionar evento. Erro: ", ex.Message));
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                var evento = await _eventoService.UpdateEvento(id, model);
                if (evento is null) { return BadRequest("Erro ao tentar atualizar evento."); }
                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    string.Format("Erro ao tentar atualizar evento. Erro: ", ex.Message));
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return await _eventoService.DeleteEvento(id) ?
                    Ok() :
                    BadRequest("Falha ao deletar evento.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    string.Format("Erro ao tentar excluir evento. Erro: ", ex.Message));
            }
        }
    }
}