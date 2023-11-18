using ProEventos.Domain.Entities;

namespace ProEventos.Persistence.Interface
{
    public interface IEventoRepository
    {
        Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
    }
}