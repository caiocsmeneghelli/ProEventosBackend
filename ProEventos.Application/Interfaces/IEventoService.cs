using ProEventos.Application.Dtos;
using ProEventos.Domain.Entities;
using ProEventos.Persistence.Helpers;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<EventoDto> AddEvento(int userId, EventoDto model);
        Task<EventoDto> UpdateEvento(int userId, int idEvento, EventoDto model);
        Task<bool> DeleteEvento(int userId, int idEvento);
        Task<PageList<EventoDto>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false);
        Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
        
    }
}