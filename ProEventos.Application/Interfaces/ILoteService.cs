using ProEventos.Application.Dtos;

namespace ProEventos.Application.Interfaces
{
    public interface ILoteService
    {
        Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId);
        Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId);
        Task<LoteDto> SaveLotes(int eventoId, LoteDto[] models);
        Task<bool> DeleteLote(int loteId, int eventoId);
    }
}