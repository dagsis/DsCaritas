using AutoMapper;
using Caritas.Insfrastructure.Model;
using Caritas.Web.DTOs;

namespace Caritas.Web.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Cliente, ClienteDto>();
            CreateMap<ClienteDto, Cliente>();
        }
    }
}
