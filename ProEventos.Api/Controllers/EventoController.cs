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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EventoController(IEventoService eventoService, IWebHostEnvironment webHostEnvironment)
        {
            _eventoService = eventoService;
            _webHostEnvironment = webHostEnvironment;
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

        [HttpPost]
        [Route("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(eventoId);
                if (evento is null)
                    return BadRequest("Erro ao tentar adicionar evento.");

                var file = Request.Form.Files[0];
                if(file.Length > 0)
                {
                    DeleteImage(evento.ImagemURL);
                    evento.ImagemURL = await SaveImage(file);
                }

                var eventoRetorno = await _eventoService.UpdateEvento(eventoId, evento);
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
                    Ok(new { message = "Deletado" }) :
                    BadRequest("Falha ao deletar evento.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    string.Format("Erro ao tentar excluir evento. Erro: ", ex.Message));
            }
        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagemPath = Path.Combine(_webHostEnvironment.ContentRootPath, @"Resources/Images", imageName);
            if (System.IO.File.Exists(imagemPath))
            {
                System.IO.File.Delete(imagemPath);
            }
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName)
                .Take(10)
                .ToArray())
                .Replace(' ', '-');
            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

            var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, @"Resources/Images", imageName);

            using(var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;
        }
    }
}