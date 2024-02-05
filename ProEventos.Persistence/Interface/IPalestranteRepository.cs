using ProEventos.Domain.Entities;
using ProEventos.Persistence.Helpers;

namespace ProEventos.Persistence.Interface
{
    public interface IPalestranteRepository : IProEventosRepository
    {
        Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false);
        Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);
    }
}