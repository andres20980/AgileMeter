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
    public class OficinaControllerTest : IDisposable
    {
        OficinaController _controller;
        private readonly ILogger<OficinaController> _logger;
        private readonly IOficinaInfoRepository _oficinaInfoRepository;

        Mock<ILogger<OficinaController>> mockLogger;
        Mock<IOficinaInfoRepository> mockRepository;

        public OficinaControllerTest()
        {
            mockLogger = new Mock<ILogger<OficinaController>>();
            _logger = mockLogger.Object;

            mockRepository = new Mock<IOficinaInfoRepository>();
            _oficinaInfoRepository = mockRepository.Object;

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<everisapi.API.Entities.OficinaEntity, everisapi.API.Models.Oficina>();
            });
        }

        public void Dispose()
        {
            Mapper.Reset();
        } 

        //Method: GetOficinas

        [Fact]
        public void GetOficinas_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new OficinaController(_logger, _oficinaInfoRepository);

            var oficinaEntities = new List<everisapi.API.Entities.OficinaEntity>
            {
               new everisapi.API.Entities.OficinaEntity
               {
                   OficinaId = 1,
                   OficinaNombre = "Sevilla"
                }
            };

            mockRepository.Setup(r => r.GetOficinas()).Returns(oficinaEntities);

            //Act
            var okResult = _controller.GetOficinas();

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }


        [Fact]
        public void GetOficinas_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new OficinaController(_logger, _oficinaInfoRepository);


            mockRepository.Setup(r => r.GetOficinas()).Throws(new Exception());

            //Act
            var okResult = _controller.GetOficinas();

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }
  
    } 
}