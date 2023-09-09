using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Entities;

namespace ProEventos.Api.Helpers
{
    public class ProEventosProfile : Profile
    {
        public ProEventosProfile()
        {
            CreateMap<Evento, EventoDto>().ReverseMap();
        }
    }
}
