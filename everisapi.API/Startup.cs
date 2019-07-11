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
            services.AddScoped<IIdiomasInfoRepository, IdiomasInfoRepository>();
            services.AddScoped<IPreguntasIdiomasInfoRepository, PreguntasIdiomasInfoRepository>();
            services.AddScoped<IAsignacionesIdiomasInfoRepository, AsignacionesIdiomasInfoRepository>();
            services.AddScoped<ISectionsIdiomasInfoRepository, SectionsIdiomasInfoRepository>();


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
                cfg.CreateMap<Entities.AsignacionEntity, Models.AsignacionSinPreguntasDto>();

                cfg.CreateMap<Entities.AsignacionEntity, Models.AsignacionCreateUpdateDto>();

                cfg.CreateMap<Entities.EvaluacionEntity, Models.EvaluacionCreateUpdateDto>();
                cfg.CreateMap<Models.EvaluacionCreateUpdateDto, Entities.EvaluacionEntity>();
                cfg.CreateMap<Entities.EvaluacionEntity, Models.EvaluacionesWithoutRespuestasDto>();
                cfg.CreateMap<Entities.EvaluacionEntity, Models.EvaluacionDto>();
                cfg.CreateMap<Entities.AsignacionEntity, Models.AsignacionDto>();
                cfg.CreateMap<Entities.PreguntaEntity, Models.PreguntaDto>();
                cfg.CreateMap<Entities.PreguntaEntity, Models.PreguntaWithOneRespuestasDto>();
                cfg.CreateMap<Entities.PreguntaEntity, Models.PreguntaCreateDto>();
                cfg.CreateMap<Entities.PreguntaEntity, Models.PreguntaUpdateDto>();
                cfg.CreateMap<Entities.UserEntity, Models.UsersDto>();
                cfg.CreateMap<Entities.UserEntity, Models.UsersWithRolesDto>();
                cfg.CreateMap<Entities.UserEntity, Models.UsersSinProyectosDto>();
                cfg.CreateMap<Entities.ProyectoEntity, Models.ProyectoDto>();
                cfg.CreateMap<Entities.ProyectoEntity, Models.ProyectoCreateUpdateDto>();
                cfg.CreateMap<Entities.RoleEntity, Models.RoleDto>();
                cfg.CreateMap<Entities.SectionEntity, Models.SectionWithoutAreaDto>();
                cfg.CreateMap<Entities.SectionEntity, Models.SectionDto>();
                cfg.CreateMap<Entities.RespuestaEntity, Models.RespuestaDto>();
                cfg.CreateMap<Entities.UserProyectoEntity, Models.UserProyectoDto>();
                cfg.CreateMap<Entities.OficinaEntity, Models.Oficina>();
                cfg.CreateMap<Entities.UnidadEntity, Models.Unidad>();
                cfg.CreateMap<Entities.LineaEntity, Models.Linea>();
                cfg.CreateMap<Entities.ProyectoEntity, Models.Equipos>();
                cfg.CreateMap<Entities.IdiomasEntity, Models.Idiomas>();
                cfg.CreateMap<Entities.PreguntasIdiomasEntity, Models.PreguntasIdiomas>();
                cfg.CreateMap<Entities.AsignacionesIdiomasEntity, Models.AsignacionesIdiomas>();
                cfg.CreateMap<Entities.SectionsIdiomasEntity, Models.SectionsIdiomas>();

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
