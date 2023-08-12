using ProEventos.Application.Interfaces;
using ProEventos.Domain.Entities;
using ProEventos.Persistence.Interface;

namespace ProEventos.Application.Services
{
    public class EventoService : IEventoService
    {
        private readonly IProEventosRepository _proEventosRepository;
        private readonly IEventoRepository _eventoRepository;

        public EventoService(IProEventosRepository proEventosRepository, IEventoRepository eventoRepository)
        {
            _proEventosRepository = proEventosRepository;
            _eventoRepository = eventoRepository;
        }

        public async Task<Evento> AddEvento(Evento model)
        {
            try
            {
                _proEventosRepository.Add(model);
                if (await _proEventosRepository.SaveChangesAsync())
                {
                    return await _eventoRepository.GetEventoByIdAsync(model.Id, false);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> UpdateEvento(int idEvento, Evento model)
        {
            try
            {
                // Verificar se existe o evento
                var evento = await _eventoRepository.GetEventoByIdAsync(idEvento, false);
                if (evento is null) { return null; }

                _proEventosRepository.Update<Evento>(model);
                if (await _proEventosRepository.SaveChangesAsync())
                {
                    return await _eventoRepository.GetEventoByIdAsync(idEvento);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int idEvento)
        {
            try
            {
                // Verificar se existe o evento
                var evento = await _eventoRepository.GetEventoByIdAsync(idEvento, false);
                if (evento is null) { throw new Exception("Evento não encontrado."); }

                _proEventosRepository.Delete<Evento>(evento);
                return await _proEventosRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            return await _eventoRepository.GetAllEventosAsync(includePalestrantes);
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            return await _eventoRepository.GetAllEventosByTemaAsync(tema, includePalestrantes);
        }

        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            return await _eventoRepository.GetEventoByIdAsync(eventoId, includePalestrantes);
        }
    }
}