using AutoMapper;
using ProEventos.Api.Dtos;
using ProEventos.Domain.Entities;

namespace ProEventos.Api.Helpers
{
    public class ProEventosProfile : Profile
    {
        public ProEventosProfile()
        {
            CreateMap<Evento, EventoDto>();
        }
    }
}
