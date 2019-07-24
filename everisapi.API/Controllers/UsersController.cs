using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using everisapi.API.Services;
using everisapi.API.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using everisapi.API.Entities;
using System.Text;
using System.Security.Cryptography;

namespace everisapi.API.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UsersController : Controller
    {

        //Creamos un logger
        private ILogger<UsersController> _logger;
        private IUsersInfoRepository _userInfoRepository;

        public UsersController(ILogger<UsersController> logger, IUsersInfoRepository userInfoRepository)
        {
            _logger = logger;
            _userInfoRepository = userInfoRepository;

        }

        //Introduciendo la petición de la route devuelve todos los usuarios
        [Authorize]
        [HttpGet("allusers")]
        public IActionResult GetUsers()
        {
            try
            {
                var UsersEntities = _userInfoRepository.GetUsers();

                var results = Mapper.Map<IEnumerable<UsersWithRolesDto>>(UsersEntities);

                _logger.LogInformation("Mandamos correctamente todos los usuarios");

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Se recogio un error al recibir todos los datos de los usuarios: " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }

        }

        //Introduciendo el nombre de usuario encuentra todos los datos de este si existe
        [HttpGet("{Nombre}")]
        public IActionResult GetUser(String Nombre, bool IncluirProyectos = false)
        {
            try
            {
                //Recoge si existe el usuario si es así la devuelve si no es así muestra un error
                var Usuario = _userInfoRepository.GetUser(Nombre, IncluirProyectos);

                if (Usuario == null)
                {
                    _logger.LogInformation("El usuario con nombre " + Nombre + " no pudo ser encontrado.");
                    return NotFound();
                }

                //Si tenemos como parametro recibir sus proyectos los incluirá
                //sino lo devolverá sin proyectos
                if (IncluirProyectos)
                {
                    var UserResult = Mapper.Map<UsersDto>(Usuario);

                    return Ok(UserResult);

                }
                else
                {
                    var UserSinProyectosResult = Mapper.Map<UsersSinProyectosDto>(Usuario);

                    return Ok(UserSinProyectosResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Se recogio un error al recibir el usuario con nombre " + Nombre + ": " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }

        //Introduciendo el nombre del usuario recogemos todos sus roles
        [HttpGet("{Nombre}/roles")]
        public IActionResult GetRoles(String Nombre)
        {

            try
            {
                //Recoge si existe el usuario y si no es así devolvera un error
                var Usuario = _userInfoRepository.GetUser(Nombre, false);

                if (Usuario == null)
                {
                    _logger.LogInformation("El usuario con nombre " + Nombre + " no pudo ser encontrado recogiendo roles.");
                    return NotFound();
                }
                //Recoge todos los roles para este usuario en específico
                var RolAsignado = _userInfoRepository.GetRolesUsuario(Usuario);

                //Devolvera sus roles aunque esten vacios
                var RolResult = Mapper.Map<RoleDto>(RolAsignado);

                return Ok(RolResult);

            }
            catch (Exception ex)
            {
                _logger.LogCritical("Se recogio un error al recibir los roles de usuario con nombre " + Nombre + ": " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }

        /*ADD USUARIOS*/
        [HttpPost("add")]
        public IActionResult AddUsers([FromBody] UsersWithRolesDto UsuarioAdd)
        {
            //Si los datos son validos los guardara
            if (UsuarioAdd == null)
            {
                return BadRequest();
            }

            //Encriptamos la contraseña
            using (var sha256 = SHA256.Create())
            {
                // Le damos la contraseña
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(UsuarioAdd.Password));
                // Recogemos el hash como string
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                // Y se lo damos 
                UsuarioAdd.Password = hash;
            }

            if (_userInfoRepository.UserExiste(UsuarioAdd.Nombre))
            {//comprueba si el usuario existe
                if (_userInfoRepository.UserActivo(UsuarioAdd.Nombre))
                {//si esta activo bad request
                    return BadRequest();
                }
                else
                {//si no esta activo lo activamos
                    UsuarioAdd.Activo = true;
                    if (_userInfoRepository.AlterUserRole(Mapper.Map<UserEntity>(UsuarioAdd)))
                    {
                        return Ok("El usuario fue creado.");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            else
            {//si el usuario no existe 
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //Comprueba que se guardo bien y lo envia
                if (_userInfoRepository.AddUser(Mapper.Map<UserEntity>(UsuarioAdd)))
                {
                    return Ok("El usuario fue creado.");
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        /*UPDATE USUARIOS*/
        [HttpPut()]
        public IActionResult UpdateUsers([FromBody] UsersWithRolesDto UsuarioUpdate)
        {
            //Si los datos son validos los guardara
            if (UsuarioUpdate == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            UserEntity userEntity = _userInfoRepository.GetUser(UsuarioUpdate.Nombre, false);
            userEntity.Activo = UsuarioUpdate.Activo;
            userEntity.RoleId = UsuarioUpdate.Role.Id;
            userEntity.NombreCompleto = UsuarioUpdate.NombreCompleto;

            if (UsuarioUpdate.Password != null)
            {             
                using (var sha256 = SHA256.Create())
                {
                    // Le damos la contraseña
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(UsuarioUpdate.Password));
                    // Recogemos el hash como string
                    var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                    // Y se lo damos 
                    userEntity.Password = hash;
                }
            }

            //Comprueba que se guardo bien y lo envia
            if (_userInfoRepository.AlterUserRole(userEntity))
            {
                return Ok("El usuario fue modificado correctamente.");
            }
            else
            {
                return BadRequest();
            }
        }

        /*DELETE USUARIOS*/
        [HttpDelete("delete")]
        public IActionResult DeleteUsers([FromBody] UsersSinProyectosDto usuarioDelete)
        {
            //Si los datos son validos los guardara
            if (usuarioDelete == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Encriptamos la contraseña
            using (var sha256 = SHA256.Create())
            {
                // Le damos la contraseña
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(usuarioDelete.Password));
                // Recogemos el hash como string
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                // Y se lo damos 
                usuarioDelete.Password = hash;
            }

            UserEntity userEntity = _userInfoRepository.GetUser(usuarioDelete.Nombre, false);

            //Comprueba que se guardo bien y lo envia
            if (_userInfoRepository.DeleteUser(userEntity))
            {
                return Ok("Eliminación completada");
            }
            else
            {
                return BadRequest();
            }
        }

        //Introduciendo el nombre del usuario recogemos todos sus roles
        [HttpGet("allroles/{codigoIdioma}")]
        public IActionResult GetAllRoles(int codigoIdioma)
        {

            try
            {
                // var RolesEntities = _userInfoRepository.GetAllRoles();

                // var results = Mapper.Map<IEnumerable<RoleDto>>(RolesEntities);
                var results = _userInfoRepository.GetAllRoles(codigoIdioma);                
                _logger.LogInformation("Mandamos correctamente todos los roles");

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Se recogio un error al recibir todos los datos de los roles: " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }

        /*ADD PROYECTOS*/
        [HttpPost("addUserProject")]
        public IActionResult AddUserProject([FromBody] UserProyectoDto UserProyectoAdd)
        {

            // //Comprueba que se guardo bien y lo envia
            if (this._userInfoRepository.AddUserToProject(UserProyectoAdd.UserNombre, UserProyectoAdd.ProyectoId))
            {
                return Ok("El UserProyecto fue creado.");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("removeUserProject")]
        public IActionResult removeUserProject([FromBody] UserProyectoDto UserProyectoRemove)
        {

            // //Comprueba que se guardo bien y lo envia
            if (this._userInfoRepository.DeleteUserProject(UserProyectoRemove.UserNombre, UserProyectoRemove.ProyectoId))
            {
                return Ok("El UserProyecto fue eliminado.");
            }
            else
            {
                return BadRequest();
            }
        }

    }

}
