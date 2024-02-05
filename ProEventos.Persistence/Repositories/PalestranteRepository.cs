using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Entities;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Helpers;
using ProEventos.Persistence.Interface;

namespace ProEventos.Persistence.Repositories
{
    public class PalestranteRepository : ProEventosRepository, IPalestranteRepository
    {
        private readonly ProEventosContext _context;

        public PalestranteRepository(ProEventosContext context) : base(context)
        {
            _context = context;
        }


        public async Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.RedesSociais)
                .Include(p => p.User)
                .AsNoTracking();
            if (includeEventos)
            {
                query = query.Include(p => p.PalestranteEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            query = query.Where(p => (p.MiniCurriculo.ToLower().Contains(pageParams.Term.ToLower()) ||
                                      p.User.PrimeiroNome.ToLower().Contains(pageParams.Term.ToLower()) ||
                                      p.User.UltimoNome.ToLower().Contains(pageParams.Term.ToLower())) &&
                                      p.User.Funcao == Domain.Enums.Funcao.Palestrante)
                .OrderBy(p => p.Id);
            return await PageList<Palestrante>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.User)
                .Include(p => p.RedesSociais)
                .AsNoTracking();
            if (includeEventos)
            {
                query = query.Include(e => e.PalestranteEventos)
                    .ThenInclude(pe => pe.Evento);
            }
            query = query.Where(p => p.UserId == userId);
            return await query.FirstOrDefaultAsync();
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }
    }
}