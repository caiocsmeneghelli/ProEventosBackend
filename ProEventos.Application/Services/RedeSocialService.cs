using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Entities;
using ProEventos.Persistence.Interface;

namespace ProEventos.Application.Services
{
    public class RedeSocialService : IRedeSocialService
    {
        private readonly IRedeSocialRepository _redeSocialRepository;
        private readonly IMapper _mapper;

        public RedeSocialService(IRedeSocialRepository redeSocialRepository, IMapper mapper)
        {
            _redeSocialRepository = redeSocialRepository;
            _mapper = mapper;
        }

        public async Task AddRedeSocial(int eventoId, RedeSocialDto model)
        {
            try
            {
                var redeSocial = _mapper.Map<RedeSocial>(model);
                redeSocial.EventoId = eventoId;

                _redeSocialRepository.Add<RedeSocial>(redeSocial);
                await _redeSocialRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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

        public async Task<RedeSocialDto[]> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                var redeSociais = await _redeSocialRepository.GetAllByEventoIdAsync(eventoId);
                if (redeSociais == null)
                    return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddRedeSocial(eventoId, model);
                    }
                    else
                    {
                        var redeSocial = redeSociais.FirstOrDefault(redeSocial => redeSocial.Id == model.Id);
                        model.EventoId = eventoId;

                        _mapper.Map(model, redeSocial);

                        _redeSocialRepository.Update<RedeSocial>(redeSocial);
                        await _redeSocialRepository.SaveChangesAsync();
                    }
                }

                var redeSociaisRetorno = await _redeSocialRepository.GetAllByEventoIdAsync(eventoId);
                return _mapper.Map<RedeSocialDto[]>(redeSociaisRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}