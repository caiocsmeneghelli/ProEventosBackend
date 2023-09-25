using ProEventos.Domain.Entities;

namespace ProEventos.Persistence.Interface{
    public interface ILoteRepository
    {
        Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);
        Task<Lote[]> GetLoteByIdsAsync(int loteId, int eventoId);
    }
}