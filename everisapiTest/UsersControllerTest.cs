using AutoMapper;
using everisapi.API;
using everisapi.API.Controllers;
using everisapi.API.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
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
                cfg.CreateMap<everisapi.API.Entities.RoleEntity, everisapi.API.Models.RoleDto>();
            });

        }

        public void Dispose()
        {
            Mapper.Reset();
        }

        [Fact]
        public void GetUser_WhenCalledWithoutIncluirProyectos_ReturnOkResult()
        {
            //Arrange            
            _controller = new UsersController(_logger, _userInfoRepository);

            var entity = new everisapi.API.Entities.UserEntity
            {
                Nombre = "Jose Antonio Beltrán"
            };

            mockRepository.Setup(r => r.GetUser("jbeltrma", false)).Returns(entity);

            //Act
            var okResult = _controller.GetUser("jbeltrma", false);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetRoles_WhenCalledWithNameBeltran_BeltranHasAdminPermission()
        {
            //Arrange
            _controller = new UsersController(_logger, _userInfoRepository);

            var entity = new everisapi.API.Entities.UserEntity
            {
                Nombre = "Jose Antonio Beltrán"
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

    } //end of class
} //end of namespace
