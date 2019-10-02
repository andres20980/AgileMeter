using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using everisapi.API.Entities;
using everisapi.API.Models;
using Microsoft.EntityFrameworkCore;

namespace everisapi.API.Services
{
    public class UsersInfoRespository : IUsersInfoRepository
    {

        private AsignacionInfoContext _context;

        //Le damos un contexto en el constructor
        public UsersInfoRespository(AsignacionInfoContext context)
        {
            _context = context;
        }

        //Devuelve un solo proyecto de un usuario
        public ProyectoDto GetOneProyecto(string userNombre, int proyectoId)
        {
            var proyectoDeUsuario = _context.Proyectos.Where(p => p.UserNombre == userNombre && p.Id == proyectoId).FirstOrDefault();

            //Creamos un proyecto nuevo con los  datos estrictamente necesarios
            var ProyectoEncontrado = new everisapi.API.Models.ProyectoDto
            {
                Id = proyectoDeUsuario.Id,
                Nombre = proyectoDeUsuario.Nombre,
                Fecha = proyectoDeUsuario.Fecha,
                Proyecto = proyectoDeUsuario.Proyecto,
                numFinishedEvals = _context.Evaluaciones.Where(e => e.ProyectoId == proyectoDeUsuario.Id && e.Estado).Count(),
                numPendingEvals = _context.Evaluaciones.Where(e => e.ProyectoId == proyectoDeUsuario.Id && !e.Estado).Count(),
                TestProject = proyectoDeUsuario.TestProject
            };

            return ProyectoEncontrado;
        }

        //Recoge todos los proyectos de un usuario
        public IEnumerable<ProyectoDto> GetProyectosDeUsuario(string userNombre)
        {
            List<ProyectoDto> proyectos = new List<ProyectoDto>();

            UserEntity usuario = _context.Users.Where(u => u.Nombre == userNombre).FirstOrDefault();

            if (usuario.RoleId != (int)Roles.User)
            {
                var proyectosE = _context.Proyectos.Include(r => r.LineaEntity).Where(p => p.TestProject == false /* || p.UserNombre == userNombre*/ ).OrderBy(p => p.Proyecto).ToList();
                foreach (ProyectoEntity pe in proyectosE)
                {
                    ProyectoDto p = new ProyectoDto();
                    p.Id = pe.Id;
                    //p.Nombre = pe.TestProject ? pe.Nombre : String.Concat(pe.Nombre, " - " , pe.LineaEntity.LineaNombre);
                    p.Nombre = pe.Nombre;
                    p.Fecha = pe.Fecha;
                    p.Proyecto = pe.Proyecto;
                    p.numFinishedEvals = _context.Evaluaciones.Where(e => e.ProyectoId == pe.Id && e.Estado).Count();
                    p.numPendingEvals = _context.Evaluaciones.Where(e => e.ProyectoId == pe.Id && !e.Estado).Count();
                    p.TestProject = pe.TestProject;
                    p.Oficina = pe.Oficina;
                    proyectos.Add(p);
                }
            }
            else
            {
                var ProyectosUsuario = _context.UserProyectos.Where(up => up.UserNombre == userNombre).ToList();

                foreach (UserProyectoEntity userProyecto in ProyectosUsuario)
                {
                    var pe = _context.Proyectos.Where(pr => pr.Id == userProyecto.ProyectoId).FirstOrDefault();
                    ProyectoDto p = new ProyectoDto();
                    p.Id = pe.Id;
                    p.Nombre = pe.Nombre;
                    p.Proyecto = pe.Proyecto;
                    p.Fecha = pe.Fecha;
                    p.numFinishedEvals = _context.Evaluaciones.Where(e => e.ProyectoId == pe.Id && e.Estado).Count();
                    p.numPendingEvals = _context.Evaluaciones.Where(e => e.ProyectoId == pe.Id && !e.Estado).Count();
                    p.TestProject = pe.TestProject;
                    p.Oficina = pe.Oficina;
                    proyectos.Add(p);
                }
                proyectos = proyectos.OrderBy(pro => pro.Proyecto).ToList();
            }

            return proyectos;

        }

        //Recoge todos los proyectos de un usuario atendiendo al idioma
        public IEnumerable<ProyectoDto> GetProyectosDeUsuario(string userNombre, int codigoIdioma)
        {
            List<ProyectoDto> proyectos = new List<ProyectoDto>();

            UserEntity usuario = _context.Users.Where(u => u.Nombre == userNombre).FirstOrDefault();

            if (usuario.RoleId != (int)Roles.User)
            {
                var proyectosE = _context.Proyectos.Include(r => r.LineaEntity).Include(o => o.OficinaEntity).Where(p => p.TestProject == false /* || p.UserNombre == userNombre*/ ).OrderBy(p => p.Proyecto).ToList();
                foreach (ProyectoEntity pe in proyectosE)
                {
                    ProyectoDto p = new ProyectoDto();
                    p.Id = pe.Id;
                    //p.Nombre = pe.TestProject ? pe.Nombre : String.Concat(pe.Nombre, " - " , pe.LineaEntity.LineaNombre);
                    p.Nombre = pe.Nombre;
                    p.Fecha = pe.Fecha;
                    p.Proyecto = pe.Proyecto;
                    p.numFinishedEvals = _context.Evaluaciones.Where(e => e.ProyectoId == pe.Id && e.Estado).Count();
                    p.numPendingEvals = _context.Evaluaciones.Where(e => e.ProyectoId == pe.Id && !e.Estado).Count();
                    p.TestProject = pe.TestProject;
                    //p.Oficina = pe.Oficina;
                    p.Oficina = (string) _context.TraduccionesOficinas
                                    .Where(t => t.OficinaId == pe.OficinaEntity.OficinaId 
                                                &&
                                                t.IdiomaId == codigoIdioma)
                                    .Select(s => s.Traduccion).First();
                    proyectos.Add(p);
                }
            }
            else
            {
                var ProyectosUsuario = _context.UserProyectos.Where(up => up.UserNombre == userNombre).ToList();

                foreach (UserProyectoEntity userProyecto in ProyectosUsuario)
                {
                    var pe = _context.Proyectos.Include(o => o.OficinaEntity).Where(pr => pr.Id == userProyecto.ProyectoId).FirstOrDefault();
                    ProyectoDto p = new ProyectoDto();
                    p.Id = pe.Id;
                    p.Nombre = pe.Nombre;
                    p.Proyecto = pe.Proyecto;
                    p.Fecha = pe.Fecha;
                    p.numFinishedEvals = _context.Evaluaciones.Where(e => e.ProyectoId == pe.Id && e.Estado).Count();
                    p.numPendingEvals = _context.Evaluaciones.Where(e => e.ProyectoId == pe.Id && !e.Estado).Count();
                    p.TestProject = pe.TestProject;
                    //p.Oficina = pe.Oficina;
                    p.Oficina = (string) _context.TraduccionesOficinas
                                    .Where(t => t.OficinaId == pe.OficinaEntity.OficinaId 
                                                &&
                                                t.IdiomaId == codigoIdioma)
                                    .Select(s => s.Traduccion).First();
                    proyectos.Add(p);
                }
                proyectos = proyectos.OrderBy(pro => pro.Proyecto).ToList();
            }

            return proyectos;

        }

        //Recoge todos los proyectos de un usuario con evaluaciones pendientes
        public IEnumerable<ProyectoDto> GetProyectosDeUsuarioConEvaluacionesPendientes(string userNombre)
        {
            var user = this.GetUser(userNombre, false);
            var proyectos = this.GetProyectosDeUsuario(userNombre);
            if (user.RoleId == (int)Roles.User)
            {
                proyectos = proyectos.Where(p => p.numPendingEvals > 0).ToList();
            }
            else
            {
                proyectos = proyectos.Where(p => p.numPendingEvals > 0 && p.TestProject == false).ToList();
            }
            return proyectos;
        }

        //Recoge todos los proyectos de un usuario con evaluaciones finalizadas
        public IEnumerable<ProyectoDto> GetProyectosDeUsuarioConEvaluacionesFinalizadas(string userNombre)
        {
            var user = this.GetUser(userNombre, false);
            var proyectos = this.GetProyectosDeUsuario(userNombre);
            if (user.RoleId != (int)Roles.Admin || user.RoleId != (int)Roles.Evaluator)
            {
                proyectos = proyectos.Where(p => p.numFinishedEvals > 0).ToList();
            }
            else
            {
                proyectos = proyectos.Where(p => p.numFinishedEvals > 0 && p.TestProject == false).ToList();
            }
            return proyectos;
        }

        //Recoge todos los proyectos de un usuario con evaluaciones finalizadas atendiendo al codigoIdioma
        public IEnumerable<ProyectoDto> GetProyectosDeUsuarioConEvaluacionesFinalizadas(string userNombre, int codigoIdioma)
        {
            var user = this.GetUser(userNombre, false);
            var proyectos = this.GetProyectosDeUsuario(userNombre, codigoIdioma);
            if (user.RoleId != (int)Roles.Admin || user.RoleId != (int)Roles.Evaluator)
            {
                proyectos = proyectos.Where(p => p.numFinishedEvals > 0).ToList();
            }
            else
            {
                proyectos = proyectos.Where(p => p.numFinishedEvals > 0 && p.TestProject == false).ToList();
            }

            return proyectos;
        }

        //Recoge todos los proyectos de todos los usuarios que no sean de tipo test
        public IEnumerable<ProyectoDto> GetFullProyectos()
        {
            List<ProyectoDto> proyectos = new List<ProyectoDto>();

            var fullProjects = _context.Proyectos.Where(p => p.TestProject == false).OrderBy(p => p.Proyecto).ToList();

            foreach (ProyectoEntity pe in fullProjects)
            {
                ProyectoDto p = new ProyectoDto();
                p.Id = pe.Id;
                p.Nombre = pe.Nombre;
                p.Fecha = pe.Fecha;
                p.Proyecto = pe.Proyecto;
                p.numFinishedEvals = _context.Evaluaciones.Where(e => e.ProyectoId == pe.Id && e.Estado).Count();
                p.numPendingEvals = _context.Evaluaciones.Where(e => e.ProyectoId == pe.Id && !e.Estado).Count();
                p.TestProject = pe.TestProject;
                proyectos.Add(p);
            }

            return proyectos;
        }

        //Devuelve un listado con todos los proyectos dados de alta en el sistema que no pertenezcan al grupo de pruebas de usuario
        // y la traducción de la oficina se obtiene la primera por defecto.
        public IEnumerable<ProyectoDto> GetAllNotTestProjects()
        {
            var proyectos =  _context.Proyectos
                .Include(r => r.OficinaEntity)
                .Include(r => r.UnidadEntity)
                .Include(r => r.LineaEntity)
                .Include(r => r.Evaluaciones)
                .Where(p => !p.TestProject).OrderBy(p => p.Proyecto).ToList();
            
            var proyectoDtos = Mapper.Map<IEnumerable<ProyectoDto>>(proyectos);

            //Actualizamos las traducciones de las oficinas
            foreach(var item in proyectoDtos)
            {
                item.Oficina = (string) _context.TraduccionesOficinas
                                    .Where(t => t.OficinaId == item.OficinaEntity.OficinaId)
                                    .Select(s => s.Traduccion).First();
            }

            return proyectoDtos;
        }

        //Devuelve un listado con todos los proyectos dados de alta en el sistema que no pertenezcan al grupo de pruebas de usuario
        public IEnumerable<ProyectoDto> GetAllNotTestProjects(int codigoIdioma)
        {
            var proyectos =  _context.Proyectos
                .Include(r => r.OficinaEntity)
                .Include(r => r.UnidadEntity)
                .Include(r => r.LineaEntity)
                .Include(r => r.Evaluaciones)
                .Where(p => !p.TestProject).OrderBy(p => p.Proyecto).ToList();
            
            var proyectoDtos = Mapper.Map<IEnumerable<ProyectoDto>>(proyectos);

            //Actualizamos las traducciones de las oficinas
            foreach(var item in proyectoDtos)
            {
                item.Oficina = (string) _context.TraduccionesOficinas
                                    .Where(t => t.OficinaId == item.OficinaEntity.OficinaId 
                                                &&
                                                t.IdiomaId == codigoIdioma)
                                    .Select(s => s.Traduccion).First();
            }

            return proyectoDtos;
        }

        public IEnumerable<AssessmentEntity> GetAllAssessments()
        {
            return _context.Assessment.OrderBy(a => a.AssessmentName).ToList();
        }

        //Recoge un usuario por su nombre 
        public UserEntity GetUser(string userNombre, bool IncluirProyectos = true)
        {
            if (IncluirProyectos)
            {
                //Si se quiere incluir los proyectos del usuario entrara aquí
                //Incluimos los proyectos del usuario especificada (Include extiende de Microsoft.EntityFrameworkCore)
                return _context.Users.Include(u => u.ProyectosDeUsuario).
                Where(u => u.Nombre == userNombre).FirstOrDefault();
            }
            else
            {
                //Si no es así devolveremos solo el usuario
                return _context.Users.Where(u => u.Nombre == userNombre).FirstOrDefault();
            }
        }

        //Recoge todos los usuarios
        public IEnumerable<UsersWithRolesDto> GetUsers(int codigoIdioma)
        {
            //Devolvemos todos los usuarios activos ordenadas por Nombre 
            //return _context.Users.Include(r => r.Role).Where(u => u.Activo == true).OrderBy(c => c.Nombre).ToList(); 
            var UsersList = Mapper.Map<IEnumerable<UsersWithRolesDto>>(_context.Users.Include(r => r.Role).Where(u => u.Activo == true).ToList());
            var roles = _context.TraduccionesRoles.Where(x => x.IdiomaId == codigoIdioma).ToList();
            foreach (var u in UsersList)
            {
                u.Role.Role = roles.Where(x => x.RoleId == u.Role.Id).Select(x => x.Traduccion).FirstOrDefault();
            }
            return UsersList;
        }

        //Devuelve si el usuario existe
        public bool UserExiste(string userNombre)
        {
            return _context.Users.Any(u => u.Nombre == userNombre);
        }

        //Devuelve si el usuario esta activo
        public bool UserActivo(string userNombre)
        {
            return _context.Users.Any(u => u.Nombre == userNombre && u.Activo);
        }

        //Devuelve todos los roles de usuario
        public RoleEntity GetRolesUsuario(UserEntity usuario)
        {
            RoleEntity RolUsuario = new RoleEntity();

            RolUsuario = _context.Roles.Where(r => r.Id == usuario.RoleId).FirstOrDefault();

            return RolUsuario;
        }

        //Devuelve todos los roles
        public IEnumerable<RoleDto> GetAllRoles(int codigoIdioma)
        {
            var traducciones = _context.TraduccionesRoles.Where(x => x.IdiomaId == codigoIdioma).ToList();
            List<RoleDto> rolesTraducidos = new List<RoleDto>();

            foreach (var rol in traducciones)
            {
                RoleDto rolDto = new RoleDto();
                rolDto.Id = rol.RoleId;
                rolDto.Role = rol.Traduccion;
                rolesTraducidos.Add(rolDto);
            }
            return rolesTraducidos;

        }

        //Devuelve una lista con todos los datos del proyecto por su id
        public ProyectoEntity GetFullProject(int id)
        {
            return _context.Proyectos
                .Include(r => r.OficinaEntity)
                .Include(r => r.UnidadEntity)
                .Include(r => r.LineaEntity)
                .Include(p => p.Evaluaciones)
                .ThenInclude(Evaluacion => Evaluacion.Respuestas)
                .Where(p => p.Id == id).FirstOrDefault();
        }

        //Devuelve si el usuario esta bien logeado o no
        public bool UserAuth(UsersSinProyectosDto UserForAuth)
        {
            //return _context.Users.Any(u => u.Nombre.Equals(UserForAuth.Nombre));
            return _context.Users.Any(u => u.Nombre == UserForAuth.Nombre && u.Password == UserForAuth.Password && u.Activo);
        }

        /*GUARDAR DATOS EN USUARIO*/
        //Aqui introducimos un nuevo usuario
        public bool AddUser(UserEntity usuario)
        {
            usuario.Role = _context.Roles.Where(r => r.Id == usuario.Role.Id).FirstOrDefault();
            usuario.Activo = true;
            _context.Users.Add(usuario);
            this.AddProjectTest(usuario.Nombre);
            return SaveChanges();
        }

        //Este metodo nos permite persistir los cambios en las entidades
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        /*UPDATE USER*/
        //Nos permite modificar un usuario
        public bool AlterUser(UserEntity usuario)
        {
            var UserAlter = _context.Users.Where(u => u.Nombre == usuario.Nombre).FirstOrDefault();
            //UserAlter.Nombre = usuario.Nombre;
            UserAlter.NombreCompleto = usuario.NombreCompleto;
            UserAlter.Activo = usuario.Activo;
            UserAlter.Password = usuario.Password;

            return SaveChanges();
        }

        public bool AlterUserRole(UserEntity usuario)
        {
            var UserAlter = _context.Users.Where(u => u.Nombre == usuario.Nombre).FirstOrDefault();
            //UserAlter.Nombre = usuario.Nombre;
            UserAlter.Activo = usuario.Activo;
            UserAlter.RoleId = usuario.RoleId;
            UserAlter.NombreCompleto = usuario.NombreCompleto;
            UserAlter.Password = usuario.Password;

            return SaveChanges();
        }

        /*ELIMINAR DATOS*/
        //Elimina una pregunta concreta de una asignación
        public bool DeleteUser(UserEntity usuario)
        {
            _context.Users.Remove(usuario);
            return SaveChanges();

        }

        //Elimina todos los proyectos y roles de los que depende el usuario
        public void DeleteRolesOrProjects(UserEntity usuario)
        {
            var Usuario = _context.Users.Include(u => u.ProyectosDeUsuario).ThenInclude(p => p.Evaluaciones).Where(u => u.Nombre == usuario.Nombre).FirstOrDefault();
            if (Usuario.ProyectosDeUsuario.Count != 0)
            {

                foreach (var proyecto in Usuario.ProyectosDeUsuario)
                {
                    foreach (var evaluacion in proyecto.Evaluaciones)
                    {
                        _context.Evaluaciones.Remove(evaluacion);
                    }
                    _context.Proyectos.Remove(proyecto);
                }
                SaveChanges();
            }
        }

        //Aqui introducimos un nuevo proyecto
        public bool AddProj(ProyectoEntity proyecto)
        {
            _context.Proyectos.Add(proyecto);
            return SaveChanges();
        }

        public bool AddProjectTest(string userNombre)
        {
            ProyectoEntity proyecto = new ProyectoEntity();
            proyecto.OficinaEntity = _context.Oficina.Where(o => o.OficinaId == 1).FirstOrDefault();
            proyecto.UnidadEntity = _context.Unidad.Where(u => u.UnidadId == 1).FirstOrDefault();
            proyecto.LineaEntity = _context.Linea.Where(l => l.LineaId == 1).FirstOrDefault();
            proyecto.Fecha = System.DateTime.Now;
            proyecto.Nombre = string.Format("Equipo de pruebas de {0}", userNombre);
            proyecto.UserNombre = userNombre;
            proyecto.ProjectSize = 1;
            proyecto.TestProject = true;

            proyecto.Oficina = "Oficina de prueba";
            proyecto.Unidad = "Unidad de prueba";
            proyecto.Proyecto = "Proyecto de prueba";
            //Creamos el nuevo proyecto test
            _context.Proyectos.Add(proyecto);
            SaveChanges();
            int idProyecto = _context.Proyectos.Where(u => u.UserNombre == userNombre).FirstOrDefault().Id;
            //Asignamos el proyecto al usuario
            AddUserToProject(userNombre, idProyecto);

            return SaveChanges();
        }

        //Nos permite modificar un proyecto
        public bool AlterProj(ProyectoEntity proyecto)
        {
            var AlterProject = _context.Proyectos.Where(p => p.Id == proyecto.Id).FirstOrDefault();

            AlterProject.Nombre = proyecto.Nombre;
            AlterProject.Codigo = proyecto.Codigo;
            AlterProject.Fecha = System.DateTime.Now;
            AlterProject.UserNombre = proyecto.UserNombre;
            AlterProject.Unidad = proyecto.Unidad;
            //AlterProject.Oficina = proyecto.Oficina.Trim();  
            AlterProject.Proyecto = proyecto.Proyecto.Trim();
            //AlterProject.OficinaEntity = _context.Oficina.Where(o => o.OficinaId == 1).FirstOrDefault();
            AlterProject.UnidadEntity = _context.Unidad.Where(u => u.UnidadId == 1).FirstOrDefault();
            AlterProject.LineaEntity = _context.Linea.Where(l => l.LineaId == 1).FirstOrDefault();

            AlterProject.OficinaEntity = _context.Oficina.Where(o => o.OficinaId == proyecto.OficinaEntity.OficinaId).FirstOrDefault();     
            //AlterProject.UnidadEntity = _context.Unidad.Where(u => u.UnidadId == proyecto.UnidadEntity.UnidadId).FirstOrDefault();
            //AlterProject.LineaEntity = _context.Linea.Where(l => l.LineaId == proyecto.LineaEntity.LineaId).FirstOrDefault();      
            AlterProject.ProjectSize = proyecto.ProjectSize;

            return SaveChanges();
        }

        //Elimina un proyecto
        public bool DeleteProj(ProyectoEntity proyecto)
        {
            _context.Proyectos.Remove(proyecto);
            return SaveChanges();
        }

        //Devuelve si existe un proyecto
        public bool ProyectoExiste(int ProyectoId)
        {
            return _context.Proyectos.Any(p => p.Id == ProyectoId);
        }

        public bool AddUserToProject(string UserNombre, int ProyectoId)
        {
            UserProyectoEntity userProyecto = new UserProyectoEntity();

            //userProyecto.Id = _context.UserProyectos.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            userProyecto.UserNombre = UserNombre;
            userProyecto.ProyectoId = ProyectoId;

            if (_context.UserProyectos.Any(u => u.UserNombre == UserNombre && u.ProyectoId == ProyectoId))
            {
                return false;
            }

            _context.UserProyectos.Add(userProyecto);

            return SaveChanges();
        }

        public bool DeleteUserProject(string UserNombre, int ProyectoId)
        {
            UserProyectoEntity userProyecto = new UserProyectoEntity();
            userProyecto.UserNombre = UserNombre;
            userProyecto.ProyectoId = ProyectoId;

            var removed = _context.UserProyectos.Where(u => u.UserNombre == UserNombre && u.ProyectoId == ProyectoId).FirstOrDefault();

            if (removed == null)
            {
                return false;
            }

            _context.UserProyectos.Remove(removed);

            return SaveChanges();
        }

        public bool AddTeam(Equipos equipo)
        {
            ProyectoEntity proyecto = new ProyectoEntity();
            proyecto.Fecha = System.DateTime.Now;
            proyecto.Nombre = equipo.Nombre;
            proyecto.UserNombre = equipo.UserNombre;
            proyecto.ProjectSize = equipo.ProjectSize;
            proyecto.TestProject = false;
            proyecto.Oficina = "";
            proyecto.Unidad = equipo.Unidad;
            proyecto.Proyecto = equipo.Proyecto.Trim();
            proyecto.Codigo = equipo.Codigo;

            proyecto.OficinaEntity = _context.Oficina.Where(o => o.OficinaId == equipo.OficinaEntity.OficinaId).FirstOrDefault();  
            proyecto.UnidadEntity = _context.Unidad.Where(u => u.UnidadId == 1).FirstOrDefault();
            proyecto.LineaEntity = _context.Linea.Where(l => l.LineaId == 1).FirstOrDefault();


            /*//eliminado temporalmente hasta tener la lista de oficinas, unidades y proyectos   
            proyecto.UnidadEntity = _context.Unidad.Where(u => u.UnidadId == equipo.UnidadEntity.UnidadId).FirstOrDefault();
            proyecto.LineaEntity = _context.Linea.Where(l => l.LineaId == equipo.LineaEntity.LineaId).FirstOrDefault();*/

            //Creamos el nuevo team
            _context.Proyectos.Add(proyecto);
            SaveChanges();

            return SaveChanges();
        }

        public string getNombreCompleto(string nombre)
        {
            string res = "";
            UserEntity user = new UserEntity();
            user = _context.Users.Where(u => u.Nombre == nombre).FirstOrDefault();
            if (user.NombreCompleto == null || user.NombreCompleto == "")
            {
                res = user.Nombre;
            }
            else
            {
                res = user.NombreCompleto;
            }
            return res;
        }

    }
}