using AutoMapper;
using Microsoft.Extensions.Logging;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Entities;
using ProEventos.Persistence.Helpers;
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

        public async Task<PageList<EventoDto>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false)
        {
            var eventos = await _eventoRepository.GetAllEventosAsync(userId, pageParams, includePalestrantes);
            var resultado = _mapper.Map<PageList<EventoDto>>(eventos);

            resultado.CurrentPage = eventos.CurrentPage;
            resultado.TotalPages = eventos.TotalPages;
            resultado.PageSize = eventos.PageSize;
            resultado.TotalCount = eventos.TotalCount;

            return resultado;
        }

        public async Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
        {
            var evento = await _eventoRepository.GetEventoByIdAsync(userId, eventoId, includePalestrantes);
            var eventoDto = _mapper.Map<EventoDto>(evento);
            return eventoDto;
        }
    }
}