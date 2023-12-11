using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Entities;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Helpers;
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

        public async Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestranteEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking()
                .Where(e => e.Tema.ToLower().Contains(pageParams.Term.ToLower()))
                .Where(e => e.UserId == userId)
                .OrderBy(e => e.Id);


            return await PageList<Evento>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
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

            query = query
                .Where(e => e.Id == eventoId)
                .Where(e => e.UserId == userId);

            return await query.SingleOrDefaultAsync();
        }

    }
}