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
    public class RespuestaControllerTest
    {
        RespuestaController _controller;
        private readonly ILogger<RespuestaController> _logger;
        private readonly IRespuestasInfoRepository _respuestasInfoRepository;

        Mock<ILogger<RespuestaController>> mockLogger;
        Mock<IRespuestasInfoRepository> mockRepository;

        public RespuestaControllerTest()
        {
            mockLogger = new Mock<ILogger<RespuestaController>>();
            _logger = mockLogger.Object;

            mockRepository = new Mock<IRespuestasInfoRepository>();
            _respuestasInfoRepository = mockRepository.Object;

            var autoMapperInstance = AutoMapperConfig.Instance;
        }

        //Method: GetRespuestas
        [Fact]
        public void GetRespuestas_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            var respuestasEntities = new List<everisapi.API.Entities.RespuestaEntity>()
            {
                new everisapi.API.Entities.RespuestaEntity {
                    Id = 1,
                    PreguntaId = 1,
                    Estado = 1,
                    EvaluacionId = 1
                },
                new everisapi.API.Entities.RespuestaEntity {
                    Id = 2,
                    PreguntaId = 2,
                    Estado = 2,
                    EvaluacionId = 1
                }
            };

            mockRepository.Setup(r => r.GetRespuestas()).Returns(respuestasEntities);

            //Act
            var okResult = _controller.GetRespuestas();

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetRespuestas_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            mockRepository.Setup(r => r.GetRespuestas()).Throws(new Exception());

            //Act
            var okResult = _controller.GetRespuestas();

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetRespuesta
        [Fact]
        public void GetRespuesta_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            var respuestasEntitie = new everisapi.API.Entities.RespuestaEntity {
                Id = 1,
                PreguntaId = 1,
                Estado = 1,
                EvaluacionId = 1
            };

            mockRepository.Setup(r => r.GetRespuesta(1)).Returns(respuestasEntitie);

            //Act
            var okResult = _controller.GetRespuesta(1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetRespuesta_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            mockRepository.Setup(r => r.GetRespuesta(1)).Throws(new Exception());

            //Act
            var okResult = _controller.GetRespuesta(1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        [Fact]
        public void GetRespuesta_WhenGetRespuestaNull_ReturnNotFoundResult()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

                everisapi.API.Entities.RespuestaEntity respuesta = null;

            mockRepository.Setup(r => r.GetRespuesta(1)).Returns(respuesta);

            //Act
            var okResult = _controller.GetRespuesta(1);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        //Method:  GetSectionsDeProyectoYPregunta
        [Fact]
        public void GetSectionsDeProyectoYPregunta_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            var respuestasEntities = new List<everisapi.API.Entities.RespuestaEntity>()
            {
                new everisapi.API.Entities.RespuestaEntity {
                    Id = 1,
                    PreguntaId = 1,
                    Estado = 1,
                    EvaluacionId = 1
                },
                new everisapi.API.Entities.RespuestaEntity {
                    Id = 2,
                    PreguntaId = 2,
                    Estado = 2,
                    EvaluacionId = 1
                }
            };

            mockRepository.Setup(r => r.GetRespuestasFromAsigEval(1,1)).Returns(respuestasEntities);

            //Act
            var okResult = _controller.GetSectionsDeProyectoYPregunta(1,1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetSectionsDeProyectoYPregunta_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            mockRepository.Setup(r => r.GetRespuestasFromAsigEval(1,1)).Throws(new Exception());

            //Act
            var okResult = _controller.GetSectionsDeProyectoYPregunta(1,1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method:  GetSectionsDeProyectoYAsignacion
        [Fact]
        public void GetSectionsDeProyectoYAsignacion_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            var respuestasEntities = new List<everisapi.API.Entities.RespuestaEntity>()
            {
                new everisapi.API.Entities.RespuestaEntity {
                    Id = 1,
                    PreguntaId = 1,
                    Estado = 1,
                    EvaluacionId = 1
                },
                new everisapi.API.Entities.RespuestaEntity {
                    Id = 2,
                    PreguntaId = 2,
                    Estado = 2,
                    EvaluacionId = 1
                }
            };

            mockRepository.Setup(r => r.GetRespuestasFromAsigEval(1,1)).Returns(respuestasEntities);

            //Act
            var okResult = _controller.GetSectionsDeProyectoYAsignacion(1,1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetSectionsDeProyectoYAsignacion_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            mockRepository.Setup(r => r.GetRespuestasFromAsigEval(1,1)).Throws(new Exception());

            //Act
            var okResult = _controller.GetSectionsDeProyectoYAsignacion(1,1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method:  UpdateRespuestasAsignacion
        [Fact]
        public void UpdateRespuestasAsignacion_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            mockRepository.Setup(r => r.UpdateRespuestasAsignacion(1,1)).Returns(true);

            //Act
            var okResult = _controller.UpdateRespuestasAsignacion(1,1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void UpdateRespuestasAsignacion_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            mockRepository.Setup(r => r.UpdateRespuestasAsignacion(1,1)).Throws(new Exception());

            //Act
            var okResult = _controller.UpdateRespuestasAsignacion(1,1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }
        

    }
}