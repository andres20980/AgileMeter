using AutoMapper;

namespace everisapiTest
{
    class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            Mapper.Initialize(cfg =>
            {
                CreateMap<everisapi.API.Entities.UnidadEntity, everisapi.API.Models.Unidad>();
                CreateMap<everisapi.API.Entities.UserEntity, everisapi.API.Models.UsersSinProyectosDto>();
                CreateMap<everisapi.API.Entities.UserEntity, everisapi.API.Models.UsersDto>();
                CreateMap<everisapi.API.Entities.UserEntity, everisapi.API.Models.UsersWithRolesDto>();
                CreateMap<everisapi.API.Entities.RoleEntity, everisapi.API.Models.RoleDto>();
                CreateMap<everisapi.API.Entities.LineaEntity, everisapi.API.Models.Linea>();
                CreateMap<everisapi.API.Entities.OficinaEntity, everisapi.API.Models.Oficina>();
                CreateMap<everisapi.API.Entities.RespuestaEntity, everisapi.API.Models.RespuestaDto>();
            });
        }
    }
}
