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
    public class EvaluacionControllerTest
    {
        EvaluacionController _controller;
        private readonly ILogger<EvaluacionController> _logger;

        private readonly IEvaluacionInfoRepository _evaluacionInfoRepository;

        Mock<ILogger<EvaluacionController>> mockLogger;
        Mock<IEvaluacionInfoRepository> mockRepository;

        public EvaluacionControllerTest()
        {
            mockLogger = new Mock<ILogger<EvaluacionController>>();
            _logger = mockLogger.Object;

            mockRepository = new Mock<IEvaluacionInfoRepository>();
            _evaluacionInfoRepository = mockRepository.Object;

            var autoMapperInstance = AutoMapperConfig.Instance;
        }

        //Method: GetEvaluaciones()
        [Fact]
        public void GetEvaluaciones_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository);

            var evaluacionesEntities = new List<everisapi.API.Entities.EvaluacionEntity>()
            {
                new everisapi.API.Entities.EvaluacionEntity {
                    Id = 1, Fecha = new DateTime()
                },
                new everisapi.API.Entities.EvaluacionEntity {
                    Id = 2, Fecha = new DateTime()
                },
            };

            mockRepository.Setup(r => r.GetEvaluaciones()).Returns(evaluacionesEntities);

            //Act
            var okResult = _controller.GetEvaluaciones();

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetPreguntasAsignacion_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository);

            var evaluacionesEntities = new List<everisapi.API.Entities.EvaluacionEntity>()
            {
                new everisapi.API.Entities.EvaluacionEntity {
                    Id = 1, Fecha = new DateTime()
                },
                new everisapi.API.Entities.EvaluacionEntity {
                    Id = 2, Fecha = new DateTime()
                },
            };

            mockRepository.Setup(r => r.GetEvaluaciones()).Throws(new Exception());

            //Act
            var okResult = _controller.GetEvaluaciones();

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetEvaluacion(int id, bool IncluirRespuestas = false)
        [Fact]
        public void GetEvaluacion_WhenCalledWithRespuesta_ReturnOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository);

            var evaluacionEntity = new  everisapi.API.Entities.EvaluacionEntity{
                Id = 1, 
                Fecha = new DateTime()
            };

            mockRepository.Setup(r => r.GetEvaluacion(1,true)).Returns(evaluacionEntity);

            //Act
            var okResult = _controller.GetEvaluacion(1,true);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetEvaluacion_WhenCalledWithoutRespuesta_ReturnOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository);

            var evaluacionEntity = new  everisapi.API.Entities.EvaluacionEntity{
                Id = 1, Fecha = new DateTime()
            };

            mockRepository.Setup(r => r.GetEvaluacion(1,false)).Returns(evaluacionEntity);

            //Act
            var okResult = _controller.GetEvaluacion(1,false);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetEvaluacion_WhenCalledNullEvaluacion_ReturnNotFoundResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository);

            everisapi.API.Entities.EvaluacionEntity evaluacionEntity = null;

            mockRepository.Setup(r => r.GetEvaluacion(1,false)).Returns(evaluacionEntity);

            //Act
            var okResult = _controller.GetEvaluacion(1,false);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void GetEvaluacion_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository);

            var evaluacionesEntities = new List<everisapi.API.Entities.EvaluacionEntity>()
            {
                new everisapi.API.Entities.EvaluacionEntity {
                    Id = 1, Fecha = new DateTime()
                },
                new everisapi.API.Entities.EvaluacionEntity {
                    Id = 2, Fecha = new DateTime()
                },
            };

            mockRepository.Setup(r => r.GetEvaluacion(It.IsAny<int>(),It.IsAny<bool>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetEvaluacion(1,true);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

    }
}