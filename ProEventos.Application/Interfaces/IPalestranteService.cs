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
        Task<PalestranteDto> AddPalestrante(int userId, AddPalestranteDto addPalestranteDto);
        Task<PalestranteDto> UpdatePalestrante(int userId, UpdatePalestranteDto updatePalestranteDto);
        Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(int userId, PageParams pageParams, bool includeEventos = true);
        Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, bool includeEventos = true);
    }
}
