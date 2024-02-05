using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Entities;
using ProEventos.Persistence.Helpers;
using ProEventos.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Services
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IProEventosRepository _proEventosRepository;
        private readonly IPalestranteRepository _palestranteRepository;
        private readonly IMapper _mapper;

        public PalestranteService(IProEventosRepository proEventosRepository, 
            IPalestranteRepository palestranteRepository,
            IMapper mapper)
        {
            _proEventosRepository = proEventosRepository;
            _palestranteRepository = palestranteRepository;
            _mapper = mapper;
        }

        public async Task<PalestranteDto> AddPalestrante(int userId, PalestranteAddDto model)
        {
            try
            {
                var palestrante = _mapper.Map<Palestrante>(model);
                palestrante.UserId = userId;

                _palestranteRepository.Add<Palestrante>(palestrante);

                if(await _palestranteRepository.SaveChangesAsync())
                {
                    var palestranteRetorno = await _palestranteRepository.GetPalestranteByUserIdAsync(palestrante.UserId, false);

                    return _mapper.Map<PalestranteDto>(palestranteRetorno);
                }

                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = true)
        {
            try
            {
                var palestrantes = await _palestranteRepository.GetAllPalestrantesAsync(pageParams, false);
                if (palestrantes == null) return null;

                var resultado = _mapper.Map<PageList<PalestranteDto>>(palestrantes);

                resultado.CurrentPage = palestrantes.CurrentPage;
                resultado.TotalPages = palestrantes.TotalPages;
                resultado.PageSize = palestrantes.PageSize;
                resultado.TotalCount = palestrantes.TotalCount;

                return resultado;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, bool includeEventos = true)
        {
            var palestrante = await _palestranteRepository.GetPalestranteByUserIdAsync(userId, false);
            if (palestrante == null) return null;

            return _mapper.Map<PalestranteDto>(palestrante);
        }

        public async Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model)
        {
            try
            {
                var palestrante = await _palestranteRepository.GetPalestranteByUserIdAsync(userId, false);
                if (palestrante == null) return null;

                model.Id = palestrante.Id;
                model.UserId = userId;

                _mapper.Map(model, palestrante);
                _palestranteRepository.Update<Palestrante>(palestrante);

                if (await _palestranteRepository.SaveChangesAsync())
                {
                    var palestranteRetorno = await _palestranteRepository.GetPalestranteByUserIdAsync(palestrante.UserId, false);

                    return _mapper.Map<PalestranteDto>(palestranteRetorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
