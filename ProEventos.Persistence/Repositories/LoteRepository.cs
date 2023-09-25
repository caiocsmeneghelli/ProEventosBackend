using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Entities;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Interface;

namespace ProEventos.Persistence.Repositories
{
    public class LoteRepository : ILoteRepository
    {
        private readonly ProEventosContext _context;
        public LoteRepository(ProEventosContext context)
        {
            _context = context;
        }
        public async Task<Lote> GetLoteByIdsAsync(int loteId, int eventoId)
        {
            IQueryable<Lote> query = _context.Lotes;
            query = query.AsNoTracking()
                .Where(lote => lote.EventoID == eventoId)
                .Where(lote => lote.Id == loteId);

            return await query.SingleOrDefaultAsync();
        }

        public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId)
        {
            IQueryable<Lote> query = _context.Lotes;
            query = query.AsNoTracking()
                .Where(lote => lote.EventoID == eventoId);

            return await query.ToArrayAsync();
        }
    }
}