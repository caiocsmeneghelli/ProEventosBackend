using System.Diagnostics.Tracing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Extensions;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Entities;
using ProEventos.Persistence.Helpers;

namespace ProEventos.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PalestranteController : ControllerBase
    {
        private readonly IPalestranteService _palestranteService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserService _userService;

        public PalestranteController(IPalestranteService palestranteService, IWebHostEnvironment webHostEnvironment,
                IUserService userService)
        {
            _palestranteService = palestranteService;
            _webHostEnvironment = webHostEnvironment;
            _userService = userService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
        {
            try
            {
                var palestrante = await _palestranteService.GetAllPalestrantesAsync(pageParams, true);
                if (palestrante is null)
                    return NoContent();

                Response.AddPagination(palestrante.CurrentPage, palestrante.PageSize, 
                    palestrante.TotalCount, palestrante.TotalPages);

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    string.Format("Erro ao tentar recuperar palestrantes. Erro: ", ex.Message));
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetPalestrantes()
        {
            try
            {
                var userId = User.GetUserId();
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(userId, true);
                if (palestrante is null)
                    return NoContent();

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    string.Format("Erro ao tentar recuperar palestrantes. Erro: ", ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(PalestranteAddDto model)
        {
            try
            {
                int userId = User.GetUserId();
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(userId, false);
                if(palestrante == null)
                    palestrante = await _palestranteService.AddPalestrante(userId, model);
                
                if (palestrante is null)
                    return BadRequest("Erro ao tentar adicionar palestrante.");
                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    string.Format("Erro ao tentar adicionar palestrante. Erro: ", ex.Message));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(PalestranteUpdateDto model)
        {
            try
            {
                int userId = User.GetUserId();
                var palestrante = await _palestranteService.UpdatePalestrante(userId, model);
                if (palestrante is null) { return BadRequest("Erro ao tentar atualizar palestrante."); }
                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    string.Format("Erro ao tentar atualizar palestrante. Erro: ", ex.Message));
            }
        }
    }
}