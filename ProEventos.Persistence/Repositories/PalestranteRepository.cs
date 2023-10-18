using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Entities;
using ProEventos.Persistence.Context;

namespace ProEventos.Persistence.Repositories
{
    public class PalestranteRepository
    {
        private readonly ProEventosContext _context;

        public PalestranteRepository(ProEventosContext context)
        {
            _context = context;
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.RedesSociais)
                .AsNoTracking();
            if (includeEventos)
            {
                query = query.Include(p => p.PalestranteEventos)
                    .ThenInclude(pe => pe.Evento);
            }
            return await query.ToArrayAsync();
        }
        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.RedesSociais)
                .AsNoTracking();
            if (includeEventos)
            {
                query = query.Include(p => p.PalestranteEventos)
                    .ThenInclude(pe => pe.Evento);
            }
            query = query.Where(p => p.User.NomeCompleto.ToLower().Contains(nome.ToLower()));
            return await query.ToArrayAsync();
        }
        public async Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.RedesSociais)
                .AsNoTracking();
            if (includeEventos)
            {
                query = query.Include(e => e.PalestranteEventos)
                    .ThenInclude(pe => pe.Evento);
            }
            query = query.Where(p => p.Id == palestranteId);
            return await query.FirstOrDefaultAsync();
        }

    }
}