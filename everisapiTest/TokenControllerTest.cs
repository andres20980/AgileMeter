using AutoMapper;
using everisapi.API;
using everisapi.API.Controllers;
using everisapi.API.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace everisapiTest
{
    public class TokenControllerTest
    {
        TokenController _controller;
        private readonly IConfiguration _configuration;
        private readonly IUsersInfoRepository _usersInfoRepository;

        Mock<IConfiguration> mockConfiguracion;
        Mock<IUsersInfoRepository> mockRepository;

        public TokenControllerTest()
        {
            mockConfiguracion = new Mock<IConfiguration>();
            _configuration = mockConfiguracion.Object;

            mockRepository = new Mock<IUsersInfoRepository>();
            _usersInfoRepository = mockRepository.Object;

            var autoMapperInstance = AutoMapperConfig.Instance;
        }

         //Method: Test

        [Fact]
        public void Test_WhenCalled_ReturnOkRequest()
        {
            //Arrange            
            _controller = new TokenController(_configuration, _usersInfoRepository);

            //Act
            var okResult = _controller.Test();

            //Assert
            Assert.Equal("API working successfully",okResult);
        }

        //Method: GetTokens

        [Fact]
        public void CreateToken_WhenCalledUserNull_ReturnBadRequest()
        {
            //Arrange            
            _controller = new TokenController(_configuration, _usersInfoRepository);

            //Act
            var okResult = _controller.CreateToken(UserAuth: null);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void CreateToken_WhenCalledNombreNull_ReturnBadRequest()
        {
            //Arrange            
            _controller = new TokenController(_configuration, _usersInfoRepository);

            var usuario = new everisapi.API.Models.UsersSinProyectosDto 
                    {Nombre = null
                    , Password ="clave"
                    , NombreCompleto = "Francisco Javier Moreno Vicente" };

            //Act
            var okResult = _controller.CreateToken(UserAuth: usuario);

            //Assert
            Assert.IsType<UnauthorizedResult>(okResult);
        }

        [Fact]
        public void CreateToken_GivenInvalidModel_ReturnsBadRequest()
        {
            //Arrange            
            _controller = new TokenController(_configuration, _usersInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var usuario = new everisapi.API.Models.UsersSinProyectosDto 
                    {Nombre = "fmorenov"
                    , Password ="clave"
                    , NombreCompleto = "Francisco Javier Moreno Vicente" };

            //Act
            var okResult = _controller.CreateToken(UserAuth: usuario);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        [Fact]
        public void CreateToken_WhenCalled_WithUser_Unauthorized_ReturnsUnauthorizedResult()
        {
            //Arrange            
            _controller = new TokenController(_configuration, _usersInfoRepository);

            var usuario = new everisapi.API.Models.UsersSinProyectosDto 
            {
                Nombre = "fmorenov"
                , Password ="clave"
                , NombreCompleto = "Francisco Javier Moreno Vicente"
            };

            

            //Act
            var okResult = _controller.CreateToken(UserAuth: usuario);

            //Assert
            Assert.IsType<UnauthorizedResult>(okResult);
        }

        [Fact]
        public void CreateToken_WhenCalled_WithUser_Authorized_ReturnsOkObjectResult()
        {
            //Arrange            
            _controller = new TokenController(_configuration, _usersInfoRepository);

            var usuario = new everisapi.API.Models.UsersSinProyectosDto 
            {
                Nombre = "fmorenov"
                , Password ="clave"
                , NombreCompleto = "Francisco Javier Moreno Vicente"
            };

            mockRepository.Setup(r => r.UserAuth(usuario)).Returns(true);
            mockRepository.Setup(r => r.getNombreCompleto(usuario.Nombre)).Returns("Francisco Javier Moreno Vicente");
            mockConfiguracion.SetupGet(c => c["JWT:key"]).Returns(",.eVeRiSaGiLeMeTeRSuPeRSeCReTKEY6754986.,");
            
            //Act
            var okResult = _controller.CreateToken(UserAuth: usuario);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void isNewUser_WhenCalled_WithUser_Authorized_ReturnsOkObjectResult()
        {
            //Arrange            
            _controller = new TokenController(_configuration, _usersInfoRepository);

            var usuario = new everisapi.API.Models.UsersSinProyectosDto 
            {
                Nombre = "fmorenov"
                , Password ="clave"
                , NombreCompleto = "Francisco Javier Moreno Vicente"
            };

            mockRepository.Setup(r => r.UserAuth(usuario)).Returns(true);
            mockRepository.Setup(r => r.getNombreCompleto(usuario.Nombre)).Returns("Francisco Javier Moreno Vicente");
            mockConfiguracion.SetupGet(c => c["JWT:key"]).Returns(",.eVeRiSaGiLeMeTeRSuPeRSeCReTKEY6754986.,");
            
            //Act
            var okResult = _controller.CreateToken(UserAuth: usuario);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }
    } 
}