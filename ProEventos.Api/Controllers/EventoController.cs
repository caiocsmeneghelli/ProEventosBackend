using System.Diagnostics.Tracing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Extensions;
using ProEventos.Api.Helpers;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Entities;
using ProEventos.Persistence.Helpers;

namespace ProEventos.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IUtil _util;
        private readonly IUserService _userService;

        private readonly string _destino = "Images";

        public EventoController(IEventoService eventoService, IUtil util, IUserService userService)
        {
            _eventoService = eventoService;
            _util = util;
            _userService = userService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
        {
            try
            {
                int userId = User.GetUserId();
                var evento = await _eventoService.GetAllEventosAsync(userId, pageParams, true);
                if (evento is null)
                    return NoContent();

                Response.AddPagination(evento.CurrentPage, evento.PageSize, 
                    evento.TotalCount, evento.TotalPages);

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
                var userId = User.GetUserId();
                var evento = await _eventoService.GetEventoByIdAsync(userId, id, true);
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

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                int userId = User.GetUserId();
                var evento = await _eventoService.AddEvento(userId, model);
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

        [HttpPost]
        [Route("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                int userId = User.GetUserId();
                var evento = await _eventoService.GetEventoByIdAsync(userId, eventoId);
                if (evento is null)
                    return BadRequest("Erro ao tentar adicionar evento.");

                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    _util.DeleteImage(evento.ImagemURL, _destino);
                    evento.ImagemURL = await _util.SaveImage(file, _destino);
                }

                var eventoRetorno = await _eventoService.UpdateEvento(userId, eventoId, evento);
                return Ok(eventoRetorno);
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
                int userId = User.GetUserId();
                var evento = await _eventoService.UpdateEvento(userId, id, model);
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
                int userId = User.GetUserId();
                var evento = await _eventoService.GetEventoByIdAsync(userId, id);
                if (evento is null) { throw new Exception("Evento não encontrado."); }

                if (await _eventoService.DeleteEvento(userId, id))
                {
                    _util.DeleteImage(evento.ImagemURL, _destino);
                    return Ok(new { message = "Deletado" });
                }
                else
                {
                    return BadRequest("Falha ao deletar evento.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    string.Format("Erro ao tentar excluir evento. Erro: ", ex.Message));
            }
        }

    }
}