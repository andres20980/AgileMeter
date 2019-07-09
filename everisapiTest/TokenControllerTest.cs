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
    } 
}