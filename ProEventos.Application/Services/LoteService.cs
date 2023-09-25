using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Entities;
using ProEventos.Persistence.Interface;

namespace ProEventos.Application.Services
{
    public class LoteService : ILoteService
    {
        private readonly IProEventosRepository _proEventosRepository;
        private readonly ILoteRepository _loteRepository;
        private readonly IMapper _mapper;

        public LoteService(IProEventosRepository proEventosRepository, ILoteRepository loteRepository, IMapper mapper)
        {
            _proEventosRepository = proEventosRepository;
            _loteRepository = loteRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteLote(int loteId, int eventoId)
        {
            try
            {
                var lote = await _loteRepository.GetLoteByIdsAsync(loteId, eventoId);
                if (lote == null)
                    throw new Exception("Lote não encontrado para remoção.");

                _proEventosRepository.Delete<Lote>(lote);
                return await _proEventosRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            try
            {
                var lote = await _loteRepository.GetLoteByIdsAsync(loteId, eventoId);
                if (lote == null)
                    return null;

                var resultado = _mapper.Map<LoteDto>(lote);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await _loteRepository.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null)
                    return null;

                var resultado = _mapper.Map<LoteDto[]>(lotes);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<LoteDto> SaveLotes(int eventoId, LoteDto[] models)
        {
            throw new NotImplementedException();
        }
    }
}