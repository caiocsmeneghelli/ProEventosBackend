using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Extensions;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;

namespace ProEventos.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class RedeSocialController : ControllerBase
    {
        private readonly IRedeSocialService _redeSocialService;
        private readonly IEventoService _eventoService;
        private readonly IPalestranteService _palestranteService;
        public RedeSocialController(IRedeSocialService redeSocialService, 
                                    IEventoService eventoService, 
                                    IPalestranteService palestranteService)
        {
            _redeSocialService = redeSocialService;
            _eventoService = eventoService;
            _palestranteService = palestranteService;
        }


        [HttpGet]
        [Route("evento/{eventoId}")]
        public async Task<IActionResult> GetByEventoId(int eventoId)
        {
            try
            {
                if(!(await AutorEvento(eventoId)))
                    return Unauthorized();

                var redeSociais = await _redeSocialService.GetAllByEventoIdAsync(eventoId);
                if (redeSociais == null)
                    return NoContent();

                return Ok(redeSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar redeSociais: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("palestrante")]
        public async Task<IActionResult> GetByPalestrante()
        {
            try
            {
                var userId = User.GetUserId();
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(userId, false);
                if(palestrante is null)
                    return Unauthorized();

                var redeSociais = await _redeSocialService.GetAllByPalestranteIdAsync(palestrante.Id);
                if (redeSociais == null)
                    return NoContent();

                return Ok(redeSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar lotes: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("evento/{eventoId}")]
        public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDto[] redeSocial)
        {
            try
            {
                if(!(await AutorEvento(eventoId)))
                    return Unauthorized();

                var redeSocialRetorno = _redeSocialService.SaveByEvento(eventoId, redeSocial);
                if (redeSocialRetorno == null)
                    return NoContent();

                return Ok(redeSocialRetorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar salvar rede social: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("palestrante")]
        public async Task<IActionResult> SaveByPalestrante(RedeSocialDto[] redeSocial)
        {
            try
            {
                var userId = User.GetUserId();
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(userId);
                if (palestrante == null)
                    return Unauthorized();

                var redeSocialRetorno = _redeSocialService.SaveByPalestrante(palestrante.Id, redeSocial);
                if (redeSocialRetorno == null)
                    return NoContent();

                return Ok(redeSocialRetorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar salvar rede social: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("evento/{eventoId}/{redeSocialId}")]
        public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                if (!(await AutorEvento(eventoId)))
                    return Unauthorized();

                var redeSocial = await _redeSocialService.GetRedeSocialEventoByIdAsync(eventoId, redeSocialId);
                if (redeSocial == null)
                    return NoContent();

                return await _redeSocialService.DeleteByEvento(eventoId, redeSocialId)
                    ? Ok(new { message = "Deletado" })
                    : throw new Exception("Ocorreu um problema não específico ao tentar deletar Lote.");
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar rede social. Erro: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("palestrante/{redeSocialId}")]
        public async Task<IActionResult> DeleteByPalestrante( int redeSocialId)
        {
            try
            {
                var userId = User.GetUserId();
                var palestrante = _palestranteService.GetPalestranteByUserIdAsync(userId);
                if (palestrante == null)
                    return Unauthorized();

                var redeSocial = await _redeSocialService.GetRedeSocialPalestranteByIdAsync(palestrante.Id, redeSocialId);
                if (redeSocial == null)
                    return NoContent();

                return await _redeSocialService.DeleteByPalestrante(palestrante.Id, redeSocialId)
                    ? Ok(new { message = "Deletado" })
                    : throw new Exception("Ocorreu um problema não específico ao tentar deletar Lote.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar rede social. Erro: {ex.Message}");
            }
        }

        [NonAction]
        private async Task<bool> AutorEvento(int eventoId){
            var userId = User.GetUserId();
            var evento = await _eventoService.GetEventoByIdAsync(userId, eventoId, false);

            if(evento is null)
                return false;

            return true;
        }
    }
}