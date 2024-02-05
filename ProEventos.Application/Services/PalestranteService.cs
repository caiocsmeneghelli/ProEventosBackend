using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Services
{
    public class PalestranteService
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
    }
}
