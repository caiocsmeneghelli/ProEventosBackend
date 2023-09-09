using AutoMapper;
using Microsoft.Extensions.Logging;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Entities;
using ProEventos.Persistence.Interface;

namespace ProEventos.Application.Services
{
    public class EventoService : IEventoService
    {
        private readonly IProEventosRepository _proEventosRepository;
        private readonly IEventoRepository _eventoRepository;
        private readonly IMapper _mapper;

        public EventoService(IProEventosRepository proEventosRepository, IEventoRepository eventoRepository, IMapper mapper)
        {
            _proEventosRepository = proEventosRepository;
            _eventoRepository = eventoRepository;
            _mapper = mapper;
        }

        public async Task<EventoDto> AddEvento(EventoDto model)
        {
            return null;
            //try
            //{
            //    _proEventosRepository.Add(model);
            //    if (await _proEventosRepository.SaveChangesAsync())
            //    {
            //        return await _eventoRepository.GetEventoByIdAsync(model.Id, false);
            //    }
            //    return null;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
        }

        public async Task<EventoDto> UpdateEvento(int idEvento, EventoDto model)
        {
            return null;
            //try
            //{
            //    // Verificar se existe o evento
            //    var evento = await _eventoRepository.GetEventoByIdAsync(idEvento, false);
            //    if (evento is null) { return null; }

            //    _proEventosRepository.Update<Evento>(model);
            //    if (await _proEventosRepository.SaveChangesAsync())
            //    {
            //        return await _eventoRepository.GetEventoByIdAsync(idEvento);
            //    }
            //    return null;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
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

        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            var eventos = await _eventoRepository.GetAllEventosAsync(includePalestrantes);
            var eventosDto = _mapper.Map<EventoDto[]>(eventos);
            return eventosDto;
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            var eventos = await _eventoRepository.GetAllEventosByTemaAsync(tema, includePalestrantes);
            var eventosDto = _mapper.Map<EventoDto[]>(eventos);
            return eventosDto;
        }

        public async Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            var evento = await _eventoRepository.GetEventoByIdAsync(eventoId, includePalestrantes);
            var eventoDto = _mapper.Map<EventoDto>(evento);
            return eventoDto;
        }
    }
}