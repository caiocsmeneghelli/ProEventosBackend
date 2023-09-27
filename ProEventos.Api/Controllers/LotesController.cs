using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;

namespace ProEventos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly ILoteService _loteService;
        public LotesController(ILoteService loteService)
        {
            _loteService = loteService;
        }


        [HttpGet]
        [Route("{eventoId}")]
        public async Task<IActionResult> GetByEventoId(int eventoId)
        {
            try
            {
                var lotes = await _loteService.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null)
                    return NoContent();

                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar lotes: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("{eventoId}")]
        public async Task<IActionResult> Put(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = _loteService.SaveLotes(eventoId, models);
                if (lotes == null)
                    return NoContent();

                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar salvar lotes: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                var lote = await _loteService.GetLoteByIdsAsync(eventoId, loteId);
                if (lote == null)
                    return NoContent();

                return await _loteService.DeleteLote(loteId, eventoId)
                    ? Ok(new { message = "Deletado" })
                    : throw new Exception("Ocorreu um problema não específico ao tentar deletar Lote.");
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar lote. Erro: {ex.Message}");
            }
        }
    }
}