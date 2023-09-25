using ProEventos.Application.Dtos;
using ProEventos.Domain.Entities;

namespace ProEventos.Application.Interfaces
{
    public interface ILoteService
    {
        Task<LoteDto[]> GetAllLotesAsync();
        Task<LoteDto[]> GetAllLotesByEventoAsync(int idEvento);
        Task<LoteDto> AddLote(LoteDto model);
        Task<LoteDto> UpdateLotes(int idEvento, LoteDto[] models);
        Task<bool> DeleteLote(int idLote);
    }
}