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
            try{
                var lotes = await _loteService.GetAllLotesByEventoAsync(eventoId);
                if(lotes == null)
                    return NoContent();
                
                return Ok(lotes);
            }
            catch(Exception ex){
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar lotes: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post()
        {
            return Ok();
        }

        [HttpPut]
        [Route("{idEvento}")]
        public async Task<IActionResult> Put(int eventoId, LoteDto[] models)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            return Ok();
        }
    }
}