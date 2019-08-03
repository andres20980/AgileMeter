using AutoMapper;

namespace everisapiTest
{
    class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<everisapi.API.Entities.UnidadEntity, everisapi.API.Models.Unidad>();
                cfg.CreateMap<everisapi.API.Entities.UserEntity, everisapi.API.Models.UsersSinProyectosDto>();
                cfg.CreateMap<everisapi.API.Entities.UserEntity, everisapi.API.Models.UsersDto>();
                cfg.CreateMap<everisapi.API.Entities.UserEntity, everisapi.API.Models.UsersWithRolesDto>();
                cfg.CreateMap<everisapi.API.Entities.RoleEntity, everisapi.API.Models.RoleDto>();
                cfg.CreateMap<everisapi.API.Entities.LineaEntity, everisapi.API.Models.Linea>();
                cfg.CreateMap<everisapi.API.Entities.OficinaEntity, everisapi.API.Models.Oficina>();
                cfg.CreateMap<everisapi.API.Entities.RespuestaEntity, everisapi.API.Models.RespuestaDto>();
                cfg.CreateMap<everisapi.API.Entities.PreguntaEntity, everisapi.API.Models.PreguntaUpdateDto>();
                cfg.CreateMap<everisapi.API.Entities.ProyectoEntity, everisapi.API.Models.ProyectoDto>();
                cfg.CreateMap<everisapi.API.Entities.SectionEntity, everisapi.API.Models.SectionWithoutAreaDto>();
                cfg.CreateMap<everisapi.API.Entities.SectionEntity, everisapi.API.Models.SectionWithoutAreaDto>();
                cfg.CreateMap<everisapi.API.Entities.AsignacionEntity, everisapi.API.Models.SectionWithoutAreaDto>();
                cfg.CreateMap<everisapi.API.Entities.PreguntaEntity, everisapi.API.Models.PreguntaDto>();
                cfg.CreateMap<everisapi.API.Entities.AsignacionEntity, everisapi.API.Models.AsignacionSinPreguntasDto>();
                cfg.CreateMap<everisapi.API.Entities.SectionEntity, everisapi.API.Models.SectionDto>();
                cfg.CreateMap<everisapi.API.Entities.AsignacionEntity, everisapi.API.Models.AsignacionDto>();

                cfg.CreateMap<everisapi.API.Models.EvaluacionCreateUpdateDto, everisapi.API.Entities.EvaluacionEntity>();
                cfg.CreateMap<everisapi.API.Models.PreguntaCreateDto, everisapi.API.Entities.PreguntaEntity>();
                cfg.CreateMap<everisapi.API.Models.PreguntaUpdateDto, everisapi.API.Entities.PreguntaEntity>();
                cfg.CreateMap<everisapi.API.Models.AsignacionCreateUpdateDto, everisapi.API.Entities.AsignacionEntity>();
                cfg.CreateMap<everisapi.API.Models.SectionWithoutAreaDto, everisapi.API.Entities.SectionEntity>();
            });
        }
    }
}
