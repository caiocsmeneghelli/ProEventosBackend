using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Entities;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Interface;

namespace ProEventos.Persistence.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        private readonly ProEventosContext _context;

        public EventoRepository(ProEventosContext context)
        {
            _context = context;
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais)
                .AsNoTracking()
                .OrderBy(e => e.Id);
            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestranteEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }
            return await query.ToArrayAsync();
        }
        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais)
                .AsNoTracking()
                .OrderBy(e => e.Id);
            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestranteEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }
            query = query.Where(e => e.Tema.ToLower().Contains(tema.ToLower()));
            return await query.ToArrayAsync();
        }
        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais)
                .AsNoTracking();
            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestranteEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }
            query = query.Where(e => e.Id == eventoId);
            return await query.SingleOrDefaultAsync();
        }

    }
}