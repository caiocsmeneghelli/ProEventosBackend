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

        public async Task<EventoDto> AddEvento(int userId, EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                evento.UserId = userId;

                _proEventosRepository.Add(evento);
                if (await _proEventosRepository.SaveChangesAsync())
                {
                    var eventoRep = await _eventoRepository.GetEventoByIdAsync(userId, evento.Id, false);
                    return _mapper.Map<EventoDto>(eventoRep);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int userId, int idEvento, EventoDto model)
        {
            try
            {
                // Verificar se existe o evento
                var evento = await _eventoRepository.GetEventoByIdAsync(userId, idEvento, false);
                if (evento is null) { return null; }

                model.Id = evento.Id;
                model.UserId = userId;

                _mapper.Map(model, evento);

                _proEventosRepository.Update<Evento>(evento);
                if (await _proEventosRepository.SaveChangesAsync())
                {
                    var eventoRep = await _eventoRepository.GetEventoByIdAsync(userId, idEvento, false);
                    return _mapper.Map<EventoDto>(eventoRep);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int userId, int idEvento)
        {
            try
            {
                // Verificar se existe o evento
                var evento = await _eventoRepository.GetEventoByIdAsync(userId, idEvento, false);
                if (evento is null) { throw new Exception("Evento não encontrado."); }

                _proEventosRepository.Delete<Evento>(evento);
                return await _proEventosRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
        {
            var eventos = await _eventoRepository.GetAllEventosAsync(userId, includePalestrantes);
            var eventosDto = _mapper.Map<EventoDto[]>(eventos);
            return eventosDto;
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
        {
            var eventos = await _eventoRepository.GetAllEventosByTemaAsync(userId, tema, includePalestrantes);
            var eventosDto = _mapper.Map<EventoDto[]>(eventos);
            return eventosDto;
        }

        public async Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
        {
            var evento = await _eventoRepository.GetEventoByIdAsync(userId, eventoId, includePalestrantes);
            var eventoDto = _mapper.Map<EventoDto>(evento);
            return eventoDto;
        }
    }
}