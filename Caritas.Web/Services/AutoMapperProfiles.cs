using AutoMapper;
using Caritas.Insfrastructure.Model;
using Caritas.Insfrastructure.Models;
using Caritas.Web.DTOs;

namespace Caritas.Web.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Cliente, ClienteDto>();
            CreateMap<ClienteDto, Cliente>();

            CreateMap<Calendario, CalendarioDto>();
            CreateMap<CalendarioDto, Calendario>();

            CreateMap<PlantillaEmail, PlantillaEmailDto>();
            CreateMap<PlantillaEmailDto, PlantillaEmail>();

            CreateMap<ResultadoDto, Resultado>();
            CreateMap<Resultado, ResultadoDto>();
        }
    }
}
