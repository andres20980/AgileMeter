using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using everisapi.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using everisapi.API.Services;
using System.IO;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace everisapi.API
{
  public class Startup
  {
    //Almacenaremos la configuración de las direcciones
    public static IConfiguration DireccionesConf { get; private set; }

    public Startup(IConfiguration configuracion)
    {
      DireccionesConf = configuracion;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      //Añadimos unos parametros para validar el token
      var tokenParams = new TokenValidationParameters()
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = DireccionesConf["JWT:issuer"],
        ValidAudience = DireccionesConf["JWT:audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(DireccionesConf["JWT:key"]))
      };

      //Incluimos seguridad JWT
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(jwtconfig =>
      {
        jwtconfig.TokenValidationParameters = tokenParams;
      });

      //MVC
      services.AddMvc();

      //Conexión con DB
      var ConexionActualBD = Startup.DireccionesConf["connectionStrings:DBConnectionString"];
      services.AddDbContext<AsignacionInfoContext>(options =>
      options.UseMySql(ConexionActualBD));

      //Incluimos estas lineas para que al utilizar un controlador con estas interfaces
      //no de fallo al utilizar estos componentes
      services.AddScoped<IAsignacionInfoRepository, AsignacionInfoRepository>();
      services.AddScoped<IUsersInfoRepository, UsersInfoRespository>();
      services.AddScoped<ISectionsInfoRepository, SectionsInfoRepository>();
      services.AddScoped<IRespuestasInfoRepository, RespuestasInfoRepository>();
      services.AddScoped<IEvaluacionInfoRepository, EvaluacionInfoRepository>();
      services.AddScoped<IOficinaInfoRepository, OficinaInfoRepository>();   
      services.AddScoped<IUnidadInfoRepository, UnidadInfoRepository>();  
      services.AddScoped<ILineaInfoRepository, LineaInfoRepository>();

      services.AddScoped<IPuntuacionSectionRepository, PuntuacionSectionRepository>();  

      //Incluimos swagger
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "AgileMeter API", Description = "Swagger para visualizar todos los metodos de la API" });
      });

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
        AsignacionInfoContext asignacionInfoContext)
    {
      //Registra eventos en la consola
      //loggerFactory.AddConsole();

      //loggerFactory.AddDebug();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/error");
      }

      //Creamos la estructura de la base de datos con sus datos
      asignacionInfoContext.EnsureSeedDataForContext();

      //La aplicación utiliza los codigos para redirigirnos a diferentes vistas
      app.UseStatusCodePages();

      //Realizamos un mapeo de nuestros Dto's y entidades
      AutoMapper.Mapper.Initialize(cfg =>
      {
        cfg.CreateMap<AsignacionEntity, Models.AsignacionSinPreguntasDto>();        
        cfg.CreateMap<AsignacionEntity, Models.AsignacionCreateUpdateDto>();        
        cfg.CreateMap<EvaluacionEntity, Models.EvaluacionCreateUpdateDto>();        
        cfg.CreateMap<EvaluacionEntity, Models.EvaluacionesWithoutRespuestasDto>();
        cfg.CreateMap<EvaluacionEntity, Models.EvaluacionDto>();
        cfg.CreateMap<AsignacionEntity, Models.AsignacionDto>();
        cfg.CreateMap<PreguntaEntity, Models.PreguntaDto>();
        cfg.CreateMap<PreguntaEntity, Models.PreguntaWithOneRespuestasDto>();
        cfg.CreateMap<PreguntaEntity, Models.PreguntaCreateDto>();
        cfg.CreateMap<PreguntaEntity, Models.PreguntaUpdateDto>();
        cfg.CreateMap<UserEntity, Models.UsersDto>();
        cfg.CreateMap<UserEntity, Models.UsersWithRolesDto>();
        cfg.CreateMap<UserEntity, Models.UsersSinProyectosDto>();
        cfg.CreateMap<ProyectoEntity, Models.ProyectoDto>();
        cfg.CreateMap<ProyectoEntity, Models.ProyectoCreateUpdateDto>();
        cfg.CreateMap<RoleEntity, Models.RoleDto>();
        cfg.CreateMap<SectionEntity, Models.SectionWithoutAreaDto>();
        cfg.CreateMap<SectionEntity, Models.SectionDto>();
        cfg.CreateMap<RespuestaEntity, Models.RespuestaDto>();
        cfg.CreateMap<UserProyectoEntity, Models.UserProyectoDto>();
        cfg.CreateMap<OficinaEntity, Models.Oficina>();
        cfg.CreateMap<UnidadEntity, Models.Unidad>();
        cfg.CreateMap<LineaEntity, Models.Linea>();        
        cfg.CreateMap<ProyectoEntity, Models.Equipos>();
        cfg.CreateMap<PuntuacionSectionEntity, Models.PuntuacionSection>();

        cfg.CreateMap<Models.EvaluacionCreateUpdateDto, EvaluacionEntity>();
        cfg.CreateMap<Models.PreguntaCreateDto, PreguntaEntity>();
        cfg.CreateMap<Models.PreguntaUpdateDto, PreguntaEntity>();
        
      });

      //Incluimos todos los cors
      app.UseCors(builder => builder
      .AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials());

      //Utilizamos Swagger
      app.UseSwagger();
      app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgileMeter Core API"));

      //Utilizamos la validación de tokens
      app.UseAuthentication();

      //Utiliza el modelo MVC
      //app.UseMvc();

      app.Use(async (context, next) =>
      {
        await next();
        if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value) &&
           !context.Request.Path.Value.StartsWith("/api/"))
        {
          context.Request.Path = "/index.html";
          await next();
        }
      });

      app.UseMvcWithDefaultRoute();

      app.UseDefaultFiles();
      app.UseStaticFiles();

      //app.Run((context) =>
      //{
      //   throw new Exception("Example exception");
      //});
      // app.Run(async (context) =>
      // {
      //     await context.Response.WriteAsync("Hello World!");
      // });
    }
  }
}
