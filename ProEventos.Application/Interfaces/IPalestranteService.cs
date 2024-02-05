using ProEventos.Application.Dtos;
using ProEventos.Persistence.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Interfaces
{
    public interface IPalestranteService
    {
        Task<PalestranteDto> AddPalestrante(int userId, PalestranteAddDto addPalestranteDto);
        Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto updatePalestranteDto);
        Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = true);
        Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, bool includeEventos = true);
    }
}
