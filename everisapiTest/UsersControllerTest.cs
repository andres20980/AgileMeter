using AutoMapper;
using everisapi.API;
using everisapi.API.Controllers;
using everisapi.API.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace everisapiTest
{
    public class UsersControllerTest : IDisposable
    {
        UsersController _controller;
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersInfoRepository _userInfoRepository;

        Mock<ILogger<UsersController>> mockLogger;
        Mock<IUsersInfoRepository> mockRepository;

        public UsersControllerTest()
        {
            mockLogger = new Mock<ILogger<UsersController>>();
            _logger = mockLogger.Object;

            mockRepository = new Mock<IUsersInfoRepository>();
            _userInfoRepository = mockRepository.Object;

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<everisapi.API.Entities.UserEntity, everisapi.API.Models.UsersSinProyectosDto>();
                cfg.CreateMap<everisapi.API.Entities.UserEntity, everisapi.API.Models.UsersDto>();
                cfg.CreateMap<everisapi.API.Entities.UserEntity, everisapi.API.Models.UsersWithRolesDto>();
                cfg.CreateMap<everisapi.API.Entities.RoleEntity, everisapi.API.Models.RoleDto>();
            });

        }

        public void Dispose()
        {
            Mapper.Reset();
        }


        //Method: GetUsers

        [Fact]
        public void GetUsers_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new UsersController(_logger, _userInfoRepository);

            var usersEntities = new List<everisapi.API.Entities.UserEntity>()
            {
                new everisapi.API.Entities.UserEntity {
                    Nombre = "Jose Antonio Beltran"
                },
                new everisapi.API.Entities.UserEntity {
                    Nombre = "Francisco Javier Moreno"
                }
            };

            mockRepository.Setup(r => r.GetUsers()).Returns(usersEntities);

            //Act
            var okResult = _controller.GetUsers();

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }


        [Fact]
        public void GetUsers_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new UsersController(_logger, _userInfoRepository);


            mockRepository.Setup(r => r.GetUsers()).Throws(new Exception());

            //Act
            var okResult = _controller.GetUsers();

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetUser

        [Fact]
        public void GetUser_WhenCalledWithoutIncluirProyectos_ReturnOkResult()
        {
            //Arrange            
            _controller = new UsersController(_logger, _userInfoRepository);

            var entity = new everisapi.API.Entities.UserEntity
            {
                Nombre = "Jose Antonio Beltran"
            };

            mockRepository.Setup(r => r.GetUser("jbeltrma", false)).Returns(entity);

            //Act
            var okResult = _controller.GetUser("jbeltrma", false);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetUser_WhenCalledWithIncluirProyectos_ReturnOkResult()
        {
            //Arrange            
            _controller = new UsersController(_logger, _userInfoRepository);

            var entity = new everisapi.API.Entities.UserEntity
            {
                Nombre = "Jose Antonio Beltran"
            };

            mockRepository.Setup(r => r.GetUser("jbeltrma", true)).Returns(entity);

            //Act
            var okResult = _controller.GetUser("jbeltrma", true);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetUser_WhenGetUserNull_ReturnNotFoundResult()
        {
            //Arrange            
            _controller = new UsersController(_logger, _userInfoRepository);

            everisapi.API.Entities.UserEntity entity = null;

            mockRepository.Setup(r => r.GetUser("jbeltrma", false)).Returns(entity);

            //Act
            var okResult = _controller.GetUser("jbeltrma", false);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }


        [Fact]
        public void GetUser_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new UsersController(_logger, _userInfoRepository);


            mockRepository.Setup(r => r.GetUser("jbeltrma", false)).Throws(new Exception());

            //Act
            var okResult = _controller.GetUser("jbeltrma", false);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetRoles

        [Fact]
        public void GetRoles_WhenCalledWithNameBeltran_BeltranHasAdminPermission()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);

            var entity = new everisapi.API.Entities.UserEntity
            {
                Nombre = "Jose Antonio Beltran"
            };

            mockRepository.Setup(r => r.GetUser("jbeltrma", false)).Returns(entity);


            var rolEntity = new everisapi.API.Entities.RoleEntity
            {
                Id = 1,
                Role = "Admin"
            };
            mockRepository.Setup(x => x.GetRolesUsuario(entity)).Returns(rolEntity);


            //Act
            var okResult = _controller.GetRoles("jbeltrma");

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }


        [Fact]
        public void GetRoles_WhenThrowException_ReturnStatusCode()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);

            var entity = new everisapi.API.Entities.UserEntity
            {
                Nombre = "Jose Antonio Beltran"
            };

            mockRepository.Setup(r => r.GetUser("jbeltrma", false)).Returns(entity);


            mockRepository.Setup(x => x.GetRolesUsuario(entity)).Throws(new Exception());


            //Act
            var okResult = _controller.GetRoles("jbeltrma");

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        [Fact]
        public void GetRoles_WhenGetUserNull_ReturnNotFoundResult()
        {
            //Arrange            
            _controller = new UsersController(_logger, _userInfoRepository);

            everisapi.API.Entities.UserEntity entity = null;

            mockRepository.Setup(r => r.GetUser("jbeltrma", false)).Returns(entity);

            //Act
            var okResult = _controller.GetRoles("jbeltrma");

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        //Method: AddUsers

        [Fact]
        public void AddUsers_GivenNullUser_ReturnsBadRequest()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);

            //Act
            var okResult = _controller.AddUsers(UsuarioAdd: null);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddUsers_GivenInvalidModel_ReturnsBadRequest()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var rol = new everisapi.API.Models.RoleDto {Id = 1, Role = "Usuario"};

            var proyectosDeUsuario = new List<everisapi.API.Models.ProyectoDto> {
                new everisapi.API.Models.ProyectoDto{Id = 1, Nombre = "Mi Proyecto"}};

            //Act
            var okResult = _controller.AddUsers(new everisapi.API.Models.UsersWithRolesDto 
            {Nombre = "Pedro"
            , Password="clave"
            , Activo = true
            , Role = rol
            , ProyectosDeUsuario = proyectosDeUsuario});

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        [Fact]
        public void AddUsers_GivenExitUserActivo_ReturnsBadRequest()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);
            mockRepository.Setup(r => r.AddUser(It.Is<everisapi.API.Entities.UserEntity>(u => true))).Returns(true);
            mockRepository.Setup(r => r.UserExiste("fmorenov")).Returns(true);
            mockRepository.Setup(r => r.UserActivo("fmorenov")).Returns(true);

            var rol = new everisapi.API.Models.RoleDto {Id = 1, Role = "Usuario"};
            var proyectosDeUsuario = new List<everisapi.API.Models.ProyectoDto> 
                    { new everisapi.API.Models.ProyectoDto 
                        {Id = 1, Nombre = "Mi Proyecto"}};
            var usuario = new everisapi.API.Models.UsersWithRolesDto 
                    {Nombre = "fmorenov"
                    , Password="clave"
                    , Activo = true
                    , Role = rol
                    , ProyectosDeUsuario = proyectosDeUsuario};

            //Act
            var okResult = _controller.AddUsers(usuario);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddUsers_GivenExitUserNoActivo_ReturnsBadRequest()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);
            mockRepository.Setup(r => r.AddUser(It.Is<everisapi.API.Entities.UserEntity>(u => true))).Returns(true);
            mockRepository.Setup(r => r.UserExiste("fmorenov")).Returns(true);
            mockRepository.Setup(r => r.UserActivo("fmorenov")).Returns(false);
            mockRepository.Setup(r => r.AlterUserRole(It.Is<everisapi.API.Entities.UserEntity>(u => true))).Returns(false);

            var rol = new everisapi.API.Models.RoleDto {Id = 1, Role = "Usuario"};
            var proyectosDeUsuario = new List<everisapi.API.Models.ProyectoDto> 
                    { new everisapi.API.Models.ProyectoDto 
                        {Id = 1, Nombre = "Mi Proyecto"}};
            var usuario = new everisapi.API.Models.UsersWithRolesDto 
                    {Nombre = "fmorenov"
                    , Password="clave"
                    , Activo = true
                    , Role = rol
                    , ProyectosDeUsuario = proyectosDeUsuario};

            //Act
            var okResult = _controller.AddUsers(usuario);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

                [Fact]
        public void AddUsers_GivenExitUserNoActivo_ReturnsOk()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);
            mockRepository.Setup(r => r.AddUser(It.Is<everisapi.API.Entities.UserEntity>(u => true))).Returns(true);
            mockRepository.Setup(r => r.UserExiste("fmorenov")).Returns(true);
            mockRepository.Setup(r => r.UserActivo("fmorenov")).Returns(false);
            mockRepository.Setup(r => r.AlterUserRole(It.Is<everisapi.API.Entities.UserEntity>(u => true))).Returns(true);

            var rol = new everisapi.API.Models.RoleDto {Id = 1, Role = "Usuario"};
            var proyectosDeUsuario = new List<everisapi.API.Models.ProyectoDto> 
                    { new everisapi.API.Models.ProyectoDto 
                        {Id = 1, Nombre = "Mi Proyecto"}};
            var usuario = new everisapi.API.Models.UsersWithRolesDto 
                    {Nombre = "fmorenov"
                    , Password="clave"
                    , Activo = true
                    , Role = rol
                    , ProyectosDeUsuario = proyectosDeUsuario};

            //Act
            var okResult = _controller.AddUsers(usuario);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }                 
                 

        [Fact]
        public void AddUsers_GivenUserNuevo_ReturnsOk()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);
            mockRepository.Setup(r => r.AddUser(It.Is<everisapi.API.Entities.UserEntity>(u => true))).Returns(true);

            var rol = new everisapi.API.Models.RoleDto {Id = 1, Role = "Usuario"};
            var proyectosDeUsuario = new List<everisapi.API.Models.ProyectoDto> 
                    { new everisapi.API.Models.ProyectoDto 
                        {Id = 1, Nombre = "Mi Proyecto"}};
            var usuario = new everisapi.API.Models.UsersWithRolesDto 
                    {Nombre = "Pedro"
                    , Password="clave"
                    , Activo = true
                    , Role = rol
                    , ProyectosDeUsuario = proyectosDeUsuario};

            //Act
            var okResult = _controller.AddUsers(usuario);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void AddUsers_GivenUserNuevo_ReturnsBadRequest()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);

            mockRepository.Setup(r => r.AddUser(It.Is<everisapi.API.Entities.UserEntity>(u => true))).Returns(false);

            var rol = new everisapi.API.Models.RoleDto {Id = 1, Role = "Usuario"};
            var proyectosDeUsuario = new List<everisapi.API.Models.ProyectoDto> {
                new everisapi.API.Models.ProyectoDto{Id = 1, Nombre = "Mi Proyecto"}};         
            var usuario = new everisapi.API.Models.UsersWithRolesDto 
                    {Nombre = "Pedro"
                    , Password="clave"
                    , Activo = true
                    , Role = rol
                    , ProyectosDeUsuario = proyectosDeUsuario};

            //Act
            var okResult = _controller.AddUsers(usuario);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        //Method: UpdateUsers

        [Fact]
        public void UpdateUsers_GivenNullUser_ReturnsBadRequest()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);

            //Act
            var okResult = _controller.UpdateUsers(UsuarioUpdate: null);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void UpdateUsers_GivenInvalidModel_ReturnsBadRequest()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var rol = new everisapi.API.Models.RoleDto {Id = 1, Role = "Usuario"};

            var proyectosDeUsuario = new List<everisapi.API.Models.ProyectoDto> {
                new everisapi.API.Models.ProyectoDto{Id = 1, Nombre = "Mi Proyecto"}};

            //Act
            var okResult = _controller.UpdateUsers(new everisapi.API.Models.UsersWithRolesDto 
            {Nombre = "Pedro"
            , Password="clave"
            , Activo = true
            , Role = rol
            , ProyectosDeUsuario = proyectosDeUsuario});

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        [Fact]
        public void UpdateUsers_GivenUser_ReturnsBadRequest()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);

            var entidad = new everisapi.API.Entities.UserEntity
            {
                Nombre = "fmorenov",
                Activo = true,
                RoleId = 1,
                NombreCompleto = "Francisco Javier Moreno Vicente"
            };

            mockRepository.Setup(r => r.GetUser("fmorenov",false)).Returns(entidad);

            var rol = new everisapi.API.Models.RoleDto {Id = 1, Role = "Usuario"};
            var proyectosDeUsuario = new List<everisapi.API.Models.ProyectoDto> 
                    { new everisapi.API.Models.ProyectoDto 
                        {Id = 1, Nombre = "Mi Proyecto"}};
            var usuario = new everisapi.API.Models.UsersWithRolesDto 
                    {
                    Nombre = "fmorenov"
                    , Password="clave"
                    , Activo = true
                    , Role = rol
                    , ProyectosDeUsuario = proyectosDeUsuario};

            //Act
            var okResult = _controller.UpdateUsers(usuario);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }   

        [Fact]
        public void UpdateUsers_GivenUser_ReturnsOk()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);

            var entidad = new everisapi.API.Entities.UserEntity
            {
                Nombre = "fmorenov",
                Activo = true,
                RoleId = 1,
                NombreCompleto = "Francisco Javier Moreno Vicente"
            };

            mockRepository.Setup(r => r.GetUser("fmorenov",false)).Returns(entidad);

            mockRepository.Setup(r => r.AlterUserRole(It.Is<everisapi.API.Entities.UserEntity>(u => true))).Returns(true);

            mockRepository.Setup(r => r.GetUser("fmorenov",false)).Returns(entidad);

            var rol = new everisapi.API.Models.RoleDto {Id = 1, Role = "Usuario"};
            var proyectosDeUsuario = new List<everisapi.API.Models.ProyectoDto> 
                    { new everisapi.API.Models.ProyectoDto 
                        {Id = 1, Nombre = "Mi Proyecto"}};
            var usuario = new everisapi.API.Models.UsersWithRolesDto 
                    {
                    Nombre = "fmorenov"
                    , Password="clave"
                    , Activo = true
                    , Role = rol
                    , ProyectosDeUsuario = proyectosDeUsuario};

            //Act
            var okResult = _controller.UpdateUsers(usuario);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        //Method: UpdateUsers

        [Fact]
        public void DeleteUsers_GivenNullUser_ReturnsBadRequest()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);

            //Act
            var okResult = _controller.DeleteUsers(usuarioDelete: null);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void DeleteUsers_GivenInvalidModel_ReturnsBadRequest()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var rol = new everisapi.API.Models.RoleDto {Id = 1, Role = "Usuario"};

            //Act
            var okResult = _controller.DeleteUsers(new everisapi.API.Models.UsersSinProyectosDto 
            {Nombre = "Pedro"
            , Password="clave"});

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        [Fact]
        public void DeleteUsers_GivenUser_ReturnsBadRequest()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);

            var entidad = new everisapi.API.Entities.UserEntity
            {
                Nombre = "fmorenov",
                Password="clave",
                Activo = true,
                RoleId = 1,
                NombreCompleto = "Francisco Javier Moreno Vicente"
            };

            mockRepository.Setup(r => r.GetUser(entidad.Nombre, false)).Returns(entidad);
            mockRepository.Setup(r => r.DeleteUser(entidad)).Returns(false);

            var usuario = new everisapi.API.Models.UsersSinProyectosDto 
                    {
                    Nombre = "fmorenov"
                    , Password="clave"
                    , NombreCompleto = "Francisco Javier Moreno Vicente"};

            //Act
            var okResult = _controller.DeleteUsers(usuario);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }   

        [Fact]
        public void DeleteUsers_GivenUser_ReturnsOk()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);

            var entidad = new everisapi.API.Entities.UserEntity
            {
                Nombre = "fmorenov",
                Password="clave",
                Activo = true,
                RoleId = 1,
                NombreCompleto = "Francisco Javier Moreno Vicente"
            };

            mockRepository.Setup(r => r.GetUser(entidad.Nombre, false)).Returns(entidad);
            mockRepository.Setup(r => r.DeleteUser(entidad)).Returns(true);

            var usuario = new everisapi.API.Models.UsersSinProyectosDto 
                    {
                    Nombre = "fmorenov"
                    , Password="clave"
                    , NombreCompleto = "Francisco Javier Moreno Vicente"};

            //Act
            var okResult = _controller.DeleteUsers(usuario);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        //Method: GetAllRoles

        [Fact]
        public void GetAllRoles_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new UsersController(_logger, _userInfoRepository);


            mockRepository.Setup(r => r.GetAllRoles()).Returns(new List<everisapi.API.Entities.RoleEntity>());

            //Act
            var okResult = _controller.GetAllRoles();

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }      

        [Fact]
        public void GetAllRoles_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new UsersController(_logger, _userInfoRepository);


            mockRepository.Setup(r => r.GetAllRoles()).Throws(new Exception());

            //Act
            var okResult = _controller.GetAllRoles();

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: AddUserProject

        [Fact]
        public void AddUserProject_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new UsersController(_logger, _userInfoRepository);


            mockRepository.Setup(r => r.AddUserToProject("fmoreno",1)).Returns(true);

            var param = new everisapi.API.Models.UserProyectoDto {
                UserNombre = "fmoreno",
                ProyectoId = 1
            };

            //Act
            var okResult = _controller.AddUserProject(UserProyectoAdd: param);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }    

        [Fact]
        public void AddUserProject_WhenCalled_ReturnBadRequestResult()
        {
            //Arrange            
            _controller = new UsersController(_logger, _userInfoRepository);


            mockRepository.Setup(r => r.AddUserToProject("fmoreno",1)).Returns(false);

            var param = new everisapi.API.Models.UserProyectoDto {
                UserNombre = "fmoreno",
                ProyectoId = 1
            };

            //Act
            var okResult = _controller.AddUserProject(UserProyectoAdd: param);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }  

//Method: AddUserProject

        [Fact]
        public void removeUserProject_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new UsersController(_logger, _userInfoRepository);


            mockRepository.Setup(r => r.DeleteUserProject("fmoreno",1)).Returns(true);

            var param = new everisapi.API.Models.UserProyectoDto {
                UserNombre = "fmoreno",
                ProyectoId = 1
            };

            //Act
            var okResult = _controller.removeUserProject(UserProyectoRemove: param);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }    

        [Fact]
        public void removeUserProject_WhenCalled_ReturnBadRequestResult()
        {
            //Arrange            
            _controller = new UsersController(_logger, _userInfoRepository);


            mockRepository.Setup(r => r.DeleteUserProject("fmoreno",1)).Returns(false);

            var param = new everisapi.API.Models.UserProyectoDto {
                UserNombre = "fmoreno",
                ProyectoId = 1
            };

            //Act
            var okResult = _controller.removeUserProject(UserProyectoRemove: param);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }           

    } //end of class
} //end of namespace
