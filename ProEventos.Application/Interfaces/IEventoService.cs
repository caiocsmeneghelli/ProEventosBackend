using ProEventos.Application.Dtos;
using ProEventos.Domain.Entities;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<EventoDto> AddEvento(int userId, EventoDto model);
        Task<EventoDto> UpdateEvento(int userId, int idEvento, EventoDto model);
        Task<bool> DeleteEvento(int userId, int idEvento);
        Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes = false);
        Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false);
        Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
        
    }
}