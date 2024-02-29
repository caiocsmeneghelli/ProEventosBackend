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

        public async Task AddRedeSocial(int id, RedeSocialDto model, bool isEvento)
        {
            try
            {
                var redeSocial = _mapper.Map<RedeSocial>(model);

                if (isEvento)
                {
                    redeSocial.EventoId = id;
                    redeSocial.PalestranteId = null;
                }
                else
                {
                    redeSocial.EventoId = null;
                    redeSocial.PalestranteId = id;
                }


                _redeSocialRepository.Add<RedeSocial>(redeSocial);
                await _redeSocialRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if (redeSocial == null)
                    throw new Exception("Rede Social não encontrado para remoção.");

                _redeSocialRepository.Delete<RedeSocial>(redeSocial);
                return await _redeSocialRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
                if (redeSocial == null)
                    throw new Exception("Rede Social não encontrado para remoção.");

                _redeSocialRepository.Delete<RedeSocial>(redeSocial);
                return await _redeSocialRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialEventoByIdAsync(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if (redeSocial == null)
                    return null;

                var resultado = _mapper.Map<RedeSocialDto>(redeSocial);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialPalestranteByIdAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
                if (redeSocial == null)
                    return null;

                var resultado = _mapper.Map<RedeSocialDto>(redeSocial);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByEventoIdAsync(int eventoId)
        {
            try
            {
                var redeSociais = await _redeSocialRepository.GetAllByEventoIdAsync(eventoId);
                if (redeSociais == null)
                    return null;

                var resultado = _mapper.Map<RedeSocialDto[]>(redeSociais);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            try
            {
                var redeSociais = await _redeSocialRepository.GetAllByPalestranteIdAsync(palestranteId);
                if (redeSociais == null)
                    return null;

                var resultado = _mapper.Map<RedeSocialDto[]>(redeSociais);
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
                        await AddRedeSocial(eventoId, model, true);
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
        public async Task<RedeSocialDto[]> SaveByPalestrante(int palestranteId, RedeSocialDto[] models)
        {
            try
            {
                var redeSociais = await _redeSocialRepository.GetAllByPalestranteIdAsync(palestranteId);
                if (redeSociais == null)
                    return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddRedeSocial(palestranteId, model, false);
                    }
                    else
                    {
                        var redeSocial = redeSociais.FirstOrDefault(redeSocial => redeSocial.Id == model.Id);
                        model.PalestranteId = palestranteId;

                        _mapper.Map(model, redeSocial);

                        _redeSocialRepository.Update<RedeSocial>(redeSocial);
                        await _redeSocialRepository.SaveChangesAsync();
                    }
                }

                var redeSociaisRetorno = await _redeSocialRepository.GetAllByPalestranteIdAsync(palestranteId);
                return _mapper.Map<RedeSocialDto[]>(redeSociaisRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}