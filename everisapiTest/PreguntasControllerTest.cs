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
    public class PreguntasControllerTest
    {
        PreguntasController _controller;
        private readonly ILogger<PreguntasController> _logger;

        private readonly IAsignacionInfoRepository _asignacionInfoRepository;

        Mock<ILogger<PreguntasController>> mockLogger;
        Mock<IAsignacionInfoRepository> mockRepository;

        public PreguntasControllerTest()
        {
            mockLogger = new Mock<ILogger<PreguntasController>>();
            _logger = mockLogger.Object;

            mockRepository = new Mock<IAsignacionInfoRepository>();
            _asignacionInfoRepository = mockRepository.Object;

            var autoMapperInstance = AutoMapperConfig.Instance;
        }

        //Method: GetPreguntasAsignacion(int asignacionId)
        [Fact]
        public void GetPreguntasAsignacion_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntasEntities = new List<everisapi.API.Entities.PreguntaEntity>()
            {
                new everisapi.API.Entities.PreguntaEntity {
                    Id = 1,
                    Pregunta = "Pregunta 1"
                },
                new everisapi.API.Entities.PreguntaEntity {
                    Id = 2,
                    Pregunta = "Pregunta 2"
                }
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.GetPreguntaPorAsignacion(It.IsAny<int>())).Returns(preguntasEntities);

            //Act
            var okResult = _controller.GetPreguntasAsignacion(1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetPreguntasAsignacion_WhenCalledNotExitAsignacion_ReturnsNotFoundResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntasEntities = new List<everisapi.API.Entities.PreguntaEntity>()
            {
                new everisapi.API.Entities.PreguntaEntity {
                    Id = 1,
                    Pregunta = "Pregunta 1"
                },
                new everisapi.API.Entities.PreguntaEntity {
                    Id = 2,
                    Pregunta = "Pregunta 2"
                }
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.GetPreguntaPorAsignacion(It.IsAny<int>())).Returns(preguntasEntities);

            //Act
            var okResult = _controller.GetPreguntasAsignacion(1);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void GetPreguntasAsignacion_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.GetPreguntaPorAsignacion(It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetPreguntasAsignacion(1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetPreguntaAsignacion(int asignacionId, int id)
        [Fact]
        public void GetPreguntaAsignacion_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntaEntity = new everisapi.API.Entities.PreguntaEntity {
                Id = 1,
                Pregunta = "Pregunta 1",
                AsignacionId = 1
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.GetPreguntaDeAsignacion(It.IsAny<int>(), It.IsAny<int>())).Returns(preguntaEntity);

            //Act
            var okResult = _controller.GetPreguntaAsignacion(1,1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetPreguntaAsignacion_WhenCalledNotExitAsignacion_ReturnsNotFoundResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntaEntity = new everisapi.API.Entities.PreguntaEntity {
                Id = 1,
                Pregunta = "Pregunta 1",
                AsignacionId = 1
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.GetPreguntaDeAsignacion(It.IsAny<int>(), It.IsAny<int>())).Returns(preguntaEntity);

            //Act
            var okResult = _controller.GetPreguntaAsignacion(1,1);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void GetPreguntaAsignacion_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.GetPreguntaDeAsignacion(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetPreguntaAsignacion(1,1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        [Fact]
        public void GetPreguntaAsignacion_WhenPreguntaIsNull_ReturnsNotFoundResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            everisapi.API.Entities.PreguntaEntity preguntaEntity = null;

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.GetPreguntaDeAsignacion(It.IsAny<int>(), It.IsAny<int>())).Returns(preguntaEntity);

            //Act
            var okResult = _controller.GetPreguntaAsignacion(1,1);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        //Method: CreatePregunta(int asignacionId, [FromBody] PreguntaCreateDto PreguntaRecogida)
        [Fact]
        public void CreatePregunta_WhenCalledRespuestaTrue_ReturnsOkResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntaEntity = new everisapi.API.Models.PreguntaCreateDto {
                Pregunta = "Pregunta 1",
                Respuesta = true
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.CreatePregunta(1,preguntaEntity);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(okResult);
        }

        [Fact]
        public void CreatePregunta_WhenCalledRespuestaFalse_ReturnsOkResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntaEntity = new everisapi.API.Models.PreguntaCreateDto {
                Pregunta = "Pregunta 1",
                Respuesta = false
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.CreatePregunta(1,preguntaEntity);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(okResult);
        }

        [Fact]
        public void CreatePregunta_WhenCalledInvalidModel_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var preguntaEntity = new everisapi.API.Models.PreguntaCreateDto {
                Pregunta = "Pregunta 1",
                Respuesta = true
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.CreatePregunta(1,preguntaEntity);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        [Fact]
        public void CreatePregunta_WhenCalledWithNull_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            everisapi.API.Models.PreguntaCreateDto preguntaEntity = null;

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.CreatePregunta(1,preguntaEntity);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void CreatePregunta_WhenCalled_ReturnsStatusCode()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntaEntity = new everisapi.API.Models.PreguntaCreateDto {
                Pregunta = "Pregunta 1",
                Respuesta = true
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.SaveChanges()).Returns(false);

            //Act
            var okResult = _controller.CreatePregunta(1,preguntaEntity);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        [Fact]
        public void CreatePregunta_WhenCalledNotExitAsignacion_ReturnsNotFoundResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntaEntity = new everisapi.API.Models.PreguntaCreateDto {
                Pregunta = "Pregunta 1",
                Respuesta = true
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.CreatePregunta(1,preguntaEntity);            

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void CreatePregunta_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntaEntity = new everisapi.API.Models.PreguntaCreateDto {
                Pregunta = "Pregunta 1",
                Respuesta = true
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.SaveChanges()).Throws(new Exception());

            //Act
            var okResult = _controller.CreatePregunta(1,preguntaEntity);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: UpdatePregunta(int asignacionId, int id, [FromBody] PreguntaUpdateDto PreguntaCambiar)
        [Fact]
        public void UpdatePregunta_WhenCalled_ReturnsOkResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntaCambiar = new everisapi.API.Models.PreguntaUpdateDto {
                Pregunta = "Pregunta 1"
            };

            var preguntaEncontrada = new everisapi.API.Entities.PreguntaEntity 
            {
                Id = 1, 
                AsignacionEntity = null,
                AsignacionId = 1,
                Correcta = "Si",
                EsHabilitante = true,
                Nivel = 1,
                Peso = 0,
                Pregunta = "Pregunta 1",
                PreguntaHabilitante = null,
                PreguntaHabilitanteId = null
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.GetPreguntaDeAsignacion(It.IsAny<int>(), It.IsAny<int>())).Returns(preguntaEncontrada);
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.UpdatePregunta(1,1,preguntaCambiar);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void UpdatePregunta_WhenCalledNotExitPregunta_ReturnsNotFoundResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntaCambiar = new everisapi.API.Models.PreguntaUpdateDto {
                Pregunta = "Pregunta 1"
            };

            everisapi.API.Entities.PreguntaEntity preguntaEncontrada = null;

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.GetPreguntaDeAsignacion(It.IsAny<int>(), It.IsAny<int>())).Returns(preguntaEncontrada);
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.UpdatePregunta(1,1,preguntaCambiar);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void UpdatePregunta_WhenCalledInvalidModel_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var preguntaCambiar = new everisapi.API.Models.PreguntaUpdateDto {
                Pregunta = "Pregunta 1"
            };

            everisapi.API.Entities.PreguntaEntity preguntaEncontrada = new everisapi.API.Entities.PreguntaEntity {
                Pregunta = "Pregunta 1"
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.GetPreguntaDeAsignacion(It.IsAny<int>(), It.IsAny<int>())).Returns(preguntaEncontrada);
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.UpdatePregunta(1,1,preguntaCambiar);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        [Fact]
        public void UpdatePregunta_WhenCalledWithNull_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            everisapi.API.Models.PreguntaUpdateDto preguntaCambiar = null;

            var preguntaEncontrada = new everisapi.API.Entities.PreguntaEntity {
                Pregunta = "Pregunta 1"
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.GetPreguntaDeAsignacion(It.IsAny<int>(), It.IsAny<int>())).Returns(preguntaEncontrada);
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.UpdatePregunta(1,1,preguntaCambiar);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void UpdatePregunta_WhenCalled_ReturnsStatusCode()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntaCambiar = new everisapi.API.Models.PreguntaUpdateDto {
                Pregunta = "Pregunta 1"
            };

            var preguntaEncontrada = new everisapi.API.Entities.PreguntaEntity {
                Pregunta = "Pregunta 1"
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.GetPreguntaDeAsignacion(It.IsAny<int>(), It.IsAny<int>())).Returns(preguntaEncontrada);
            mockRepository.Setup(r => r.SaveChanges()).Returns(false);

            //Act
            var okResult = _controller.UpdatePregunta(1,1,preguntaCambiar);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        [Fact]
        public void UpdatePregunta_WhenCalledNotExitAsignacion_ReturnsNotFoundResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntaCambiar = new everisapi.API.Models.PreguntaUpdateDto {
                Pregunta = "Pregunta 1"
            };

            var preguntaEncontrada = new everisapi.API.Entities.PreguntaEntity {
                Pregunta = "Pregunta 1"
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.GetPreguntaDeAsignacion(It.IsAny<int>(), It.IsAny<int>())).Returns(preguntaEncontrada);
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.UpdatePregunta(1,1,preguntaCambiar);            

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void UpdatePregunta_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntaCambiar = new everisapi.API.Models.PreguntaUpdateDto {
                Pregunta = "Pregunta 1"
            };

            var preguntaEncontrada = new everisapi.API.Entities.PreguntaEntity {
                Pregunta = "Pregunta 1"
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.GetPreguntaDeAsignacion(It.IsAny<int>(), It.IsAny<int>())).Returns(preguntaEncontrada);
            mockRepository.Setup(r => r.SaveChanges()).Throws(new Exception());

            //Act
            var okResult = _controller.UpdatePregunta(1,1,preguntaCambiar);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: DeletePregunta(int asignacionId, int id)
        [Fact]
        public void DeletePregunta_WhenCalled_ReturnsOkResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntaEncontrada = new everisapi.API.Entities.PreguntaEntity {
                Pregunta = "Pregunta 1"
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.GetPreguntaDeAsignacion(It.IsAny<int>(), It.IsAny<int>())).Returns(preguntaEncontrada);
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.DeletePregunta(1,1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void DeletePregunta_WhenCalledNotExitPregunta_ReturnsOkResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            everisapi.API.Entities.PreguntaEntity preguntaEncontrada = null;

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.GetPreguntaDeAsignacion(It.IsAny<int>(), It.IsAny<int>())).Returns(preguntaEncontrada);
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.DeletePregunta(1,1);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void DeletePregunta_WhenCalled_ReturnsStatusCode()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntaEncontrada = new everisapi.API.Entities.PreguntaEntity {
                Pregunta = "Pregunta 1"
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.GetPreguntaDeAsignacion(It.IsAny<int>(), It.IsAny<int>())).Returns(preguntaEncontrada);
            mockRepository.Setup(r => r.SaveChanges()).Returns(false);

            //Act
            var okResult = _controller.DeletePregunta(1,1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        [Fact]
        public void DeletePregunta_WhenCalledNotExitAsignacion_ReturnsNotFoundResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntaEncontrada = new everisapi.API.Entities.PreguntaEntity {
                Pregunta = "Pregunta 1"
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.GetPreguntaDeAsignacion(It.IsAny<int>(), It.IsAny<int>())).Returns(preguntaEncontrada);
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.DeletePregunta(1,1);            

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void DeletePregunta_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new PreguntasController(_logger, _asignacionInfoRepository);

            var preguntaEncontrada = new everisapi.API.Entities.PreguntaEntity {
                Pregunta = "Pregunta 1"
            };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.GetPreguntaDeAsignacion(It.IsAny<int>(), It.IsAny<int>())).Returns(preguntaEncontrada);
            mockRepository.Setup(r => r.SaveChanges()).Throws(new Exception());

            //Act
            var okResult = _controller.DeletePregunta(1,1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }
    }
}