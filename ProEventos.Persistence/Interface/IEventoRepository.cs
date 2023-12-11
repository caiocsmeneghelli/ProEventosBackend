using ProEventos.Domain.Entities;
using ProEventos.Persistence.Helpers;

namespace ProEventos.Persistence.Interface
{
    public interface IEventoRepository
    {
        Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
    }
}