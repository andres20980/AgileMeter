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
    public class UnidadControllerTest : IDisposable
    {
        UnidadController _controller;
        private readonly ILogger<UnidadController> _logger;
        private readonly IUnidadInfoRepository _unidadInfoRepository;

        Mock<ILogger<UnidadController>> mockLogger;
        Mock<IUnidadInfoRepository> mockRepository;

        public UnidadControllerTest()
        {
            mockLogger = new Mock<ILogger<UnidadController>>();
            _logger = mockLogger.Object;

            mockRepository = new Mock<IUnidadInfoRepository>();
            _unidadInfoRepository = mockRepository.Object;

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<everisapi.API.Entities.UnidadEntity, everisapi.API.Models.Unidad>();
            });
        }

        public void Dispose()
        {
            Mapper.Reset();
        } 

        //Method: GetUnidades

        [Fact]
        public void GetUnidades_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new UnidadController(_logger, _unidadInfoRepository);

            var unidadEntities = new List<everisapi.API.Entities.UnidadEntity>
            {
               new everisapi.API.Entities.UnidadEntity
               {
                   UnidadId = 1,
                   UnidadNombre = "Sevilla",  
                   OficinaId =1
                }
            };

            mockRepository.Setup(r => r.GetUnidades(1)).Returns(unidadEntities);

            //Act
            var okResult = _controller.GetUnidades(1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }


        [Fact]
        public void GetUnidades_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new UnidadController(_logger, _unidadInfoRepository);


            mockRepository.Setup(r => r.GetUnidades(1)).Throws(new Exception());

            //Act
            var okResult = _controller.GetUnidades(1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }
  
    } 
}