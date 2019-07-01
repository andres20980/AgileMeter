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


            //_controller.ModelState.AddModelError("error", "some error");

            //Act
            var okResult = _controller.AddUsers(UsuarioAdd: null);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }


        [Fact]
        public void AddUser_GivenUserNuevoDeMomento_ReturnsError()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);

            mockRepository.Setup(r => r.AddUser(It.Is<everisapi.API.Entities.UserEntity>(u => true))).Returns(true);

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
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void AddUser_GivenUserInvalido_ReturnsError()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);

            mockRepository.Setup(r => r.AddUser(It.Is<everisapi.API.Entities.UserEntity>(u => true))).Returns(false);

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

    } //end of class
} //end of namespace
