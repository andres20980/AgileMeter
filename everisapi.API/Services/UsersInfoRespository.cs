using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using everisapi.API.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using everisapi.API.Models;



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
    public ProyectoEntity GetOneProyecto(string userNombre, int proyectoId)
    {
      return _context.Proyectos.Where(p => p.UserNombre == userNombre && p.Id == proyectoId).FirstOrDefault();
    }

    //Recoge todos los proyectos de un usuario
    public IEnumerable<ProyectoDto> GetProyectosDeUsuario(string userNombre)
    {
      List<ProyectoDto> proyectos = new List<ProyectoDto>();
      
      UserEntity usuario = _context.Users.Where(u => u.Nombre == userNombre).FirstOrDefault();
      
      if(usuario.RoleId != (int)Roles.User)
      {
        var proyectosE = _context.Proyectos.Include(r=>r.LineaEntity).Where(p => p.TestProject == false || p.UserNombre == userNombre).OrderBy(p => p.Proyecto).ToList();
        foreach (ProyectoEntity pe in proyectosE){
          ProyectoDto p = new ProyectoDto();
          p.Id = pe.Id;
          //p.Nombre = pe.TestProject ? pe.Nombre : String.Concat(pe.Nombre, " - " , pe.LineaEntity.LineaNombre);
          p.Nombre = pe.Nombre;
          p.Fecha = pe.Fecha;
          p.Proyecto = pe.Proyecto;
          p.numFinishedEvals = _context.Evaluaciones.Where(e => e.ProyectoId == pe.Id && e.Estado).Count();
          p.numPendingEvals = _context.Evaluaciones.Where(e => e.ProyectoId == pe.Id && !e.Estado).Count();
          p.TestProject = pe.TestProject;
          proyectos.Add(p);
        }
      }
      else
      {
        var ProyectosUsuario = _context.UserProyectos.Where(up => up.UserNombre == userNombre).ToList();
      
        foreach (UserProyectoEntity userProyecto in ProyectosUsuario){
          var pe = _context.Proyectos.Where(pr => pr.Id == userProyecto.ProyectoId).FirstOrDefault();
          ProyectoDto p = new ProyectoDto();
          p.Id = pe.Id;
          p.Nombre = pe.Nombre;
          p.Proyecto = pe.Proyecto;
          p.Fecha = pe.Fecha;
          p.numFinishedEvals = _context.Evaluaciones.Where(e => e.ProyectoId == pe.Id && e.Estado).Count();
          p.numPendingEvals = _context.Evaluaciones.Where(e => e.ProyectoId == pe.Id && !e.Estado).Count();
          p.TestProject = pe.TestProject;
          proyectos.Add(p);         
        }
        proyectos = proyectos.OrderBy(pro=>pro.Proyecto).ToList();    
      }
     
      return proyectos;

    }

    //Recoge todos los proyectos de todos los usuarios
    public IEnumerable<ProyectoEntity> GetFullProyectos(string userNombre)
    {
      return _context.Proyectos.Where(p => p.TestProject == false).OrderBy(p => p.Proyecto).ToList();
    }

    //Devuelve un listado con todos los proyectos dados de alta en el sistema que no pertenezcan al grupo de pruebas de usuario
    public IEnumerable<ProyectoEntity> GetAllNotTestProjects()
    {
      return _context.Proyectos            
            .Include(r=>r.OficinaEntity)
            .Include(r=>r.UnidadEntity)
            .Include(r=>r.LineaEntity)
            .Include(r=>r.Evaluaciones)            
            .Where(p => !p.TestProject).ToList();
    }

    public IEnumerable<AssessmentEntity> GetAllAssessments(){
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
    public IEnumerable<UserEntity> GetUsers()
    {
      //Devolvemos todos los usuarios activos ordenadas por Nombre 
      return _context.Users.Include(r => r.Role).Where(u => u.Activo == true).OrderBy(c => c.Nombre).ToList();
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
    public  IEnumerable<RoleEntity> GetAllRoles()
    { 
      return _context.Roles.OrderBy(r => r.Role).ToList();
    }

    //Devuelve una lista con todos los datos del proyecto por su id
    public ProyectoEntity GetFullProject(int id)
    {
      return _context.Proyectos
              .Include(r=>r.OficinaEntity)
              .Include(r=>r.UnidadEntity)
              .Include(r=>r.LineaEntity)
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
      proyecto.Nombre = string.Format("Equipo de pruebas de {0}",userNombre);
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
      AddUserToProject(userNombre,idProyecto);

      return SaveChanges();
    }

    //Nos permite modificar un proyecto
    public bool AlterProj(ProyectoEntity proyecto)
    {
      var AlterProject = _context.Proyectos.Where(p => p.Id == proyecto.Id).FirstOrDefault();

      AlterProject.Nombre = proyecto.Nombre;
      AlterProject.Fecha = System.DateTime.Now;
      AlterProject.UserNombre = proyecto.UserNombre;
      AlterProject.Oficina = proyecto.Oficina;
      AlterProject.Unidad = proyecto.Unidad;
      AlterProject.Proyecto = proyecto.Proyecto;
      AlterProject.OficinaEntity = _context.Oficina.Where(o => o.OficinaId == 1).FirstOrDefault();     
      AlterProject.UnidadEntity = _context.Unidad.Where(u => u.UnidadId == 1).FirstOrDefault();
      AlterProject.LineaEntity = _context.Linea.Where(l => l.LineaId == 1).FirstOrDefault(); 

      // AlterProject.OficinaEntity = _context.Oficina.Where(o => o.OficinaId == proyecto.OficinaEntity.OficinaId).FirstOrDefault();     
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

      if(_context.UserProyectos.Any(u => u.UserNombre == UserNombre && u.ProyectoId == ProyectoId)){
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

      var removed =_context.UserProyectos.Where(u => u.UserNombre == UserNombre && u.ProyectoId == ProyectoId).FirstOrDefault();
      
      if(removed == null){
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
      proyecto.Oficina = equipo.Oficina;
      proyecto.Unidad = equipo.Unidad;
      proyecto.Proyecto = equipo.Proyecto;

      proyecto.OficinaEntity = _context.Oficina.Where(o => o.OficinaId == 1).FirstOrDefault();     
      proyecto.UnidadEntity = _context.Unidad.Where(u => u.UnidadId == 1).FirstOrDefault();
      proyecto.LineaEntity = _context.Linea.Where(l => l.LineaId == 1).FirstOrDefault();

      /*//eliminado temporalmente hasta tener la lista de oficinas, unidades y proyectos
      proyecto.OficinaEntity = _context.Oficina.Where(o => o.OficinaId == equipo.OficinaEntity.OficinaId).FirstOrDefault();     
      proyecto.UnidadEntity = _context.Unidad.Where(u => u.UnidadId == equipo.UnidadEntity.UnidadId).FirstOrDefault();
      proyecto.LineaEntity = _context.Linea.Where(l => l.LineaId == equipo.LineaEntity.LineaId).FirstOrDefault();*/
      
      //Creamos el nuevo team
      _context.Proyectos.Add(proyecto);
      SaveChanges();

      return SaveChanges();
    }

    public string getNombreCompleto(string nombre){
      string res = "";
      UserEntity user = new UserEntity();
      user = _context.Users.Where(u => u.Nombre == nombre).FirstOrDefault();
      if (user.NombreCompleto == null || user.NombreCompleto == "" ){
        res = user.Nombre;
      }else{
        res = user.NombreCompleto;
      }
      return res;
    }

  }
}


