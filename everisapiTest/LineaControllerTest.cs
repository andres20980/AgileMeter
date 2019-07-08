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
    public class LineaControllerTest : IDisposable
    {
        LineaController _controller;
        private readonly ILogger<LineaController> _logger;
        private readonly ILineaInfoRepository _lineaInfoRepository;

        Mock<ILogger<LineaController>> mockLogger;
        Mock<ILineaInfoRepository> mockRepository;

        public LineaControllerTest()
        {
            mockLogger = new Mock<ILogger<LineaController>>();
            _logger = mockLogger.Object;

            mockRepository = new Mock<ILineaInfoRepository>();
            _lineaInfoRepository = mockRepository.Object;

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<everisapi.API.Entities.LineaEntity, everisapi.API.Models.Linea>();
            });
        }

        public void Dispose()
        {
            Mapper.Reset();
        } 

        //Method: GetLineas

        [Fact]
        public void GetLineas_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new LineaController(_logger, _lineaInfoRepository);

            var lineaEntities = new List<everisapi.API.Entities.LineaEntity>
            {
               new everisapi.API.Entities.LineaEntity
               {
                   LineaId = 1,
                   LineaNombre = "Sevilla", 
                   UnidadId = 1
                }
            };

            mockRepository.Setup(r => r.GetLineas(1)).Returns(lineaEntities);

            //Act
            var okResult = _controller.GetLineas(1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }


        [Fact]
        public void GetLineas_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new LineaController(_logger, _lineaInfoRepository);


            mockRepository.Setup(r => r.GetLineas(1)).Throws(new Exception());

            //Act
            var okResult = _controller.GetLineas(1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }
  
    } 
}