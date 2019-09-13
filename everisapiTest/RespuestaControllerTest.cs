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

        //Method:  AlterRespuesta
        [Fact]
        public void AlterRespuesta_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            var respuestasEntity = new everisapi.API.Models.RespuestaConUserDto {
                Id = 1,
                PreguntaId = 1,
                Estado = 1,
                EvaluacionId = 1,
                UserName = "fmoreno"
            };

            mockRepository.Setup(r => r.ExiteRespuesta(1)).Returns(true);
            mockRepository.Setup(r => r.UpdateRespuesta(respuestasEntity)).Returns(true);
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.AlterRespuesta(RespuestaUpdate: respuestasEntity);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void AlterRespuestas_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            var respuestasEntity = new everisapi.API.Models.RespuestaConUserDto {
                Id = 1,
                PreguntaId = 1,
                Estado = 1,
                EvaluacionId = 1,
                UserName = "fmoreno"
            };

            mockRepository.Setup(r => r.ExiteRespuesta(1)).Throws(new Exception());

            //Act
            var okResult = _controller.AlterRespuesta(RespuestaUpdate: respuestasEntity);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

       [Fact]
        public void AlterRespuestas_WhenNotExitRespuesta_ReturnNotFound()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            var respuestasEntity = new everisapi.API.Models.RespuestaConUserDto {
                Id = 1,
                PreguntaId = 1,
                Estado = 1,
                EvaluacionId = 1,
                UserName = "fmoreno"
            };

            mockRepository.Setup(r => r.ExiteRespuesta(1)).Returns(false);

            //Act
            var okResult = _controller.AlterRespuesta(RespuestaUpdate: respuestasEntity);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void AlterRespuestas_WhenRespuestaIsNull_ReturnNotFound()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            //Act
            var okResult = _controller.AlterRespuesta(RespuestaUpdate: null);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void AlterRespuestas_GivenInvalidModel_ReturnsBadRequest()
        {
            //Arrange
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            _controller.ModelState.AddModelError("error", "some error");

            var respuestasEntity = new everisapi.API.Models.RespuestaConUserDto {
                Id = 1,
                PreguntaId = 1,
                Estado = 1,
                EvaluacionId = 1,
                UserName = "fmoreno"
            };

            //Act
            mockRepository.Setup(r => r.ExiteRespuesta(1)).Returns(true);
            mockRepository.Setup(r => r.UpdateRespuesta(respuestasEntity)).Returns(true);

            var okResult = _controller.AlterRespuesta(RespuestaUpdate: respuestasEntity);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        [Fact]
        public void AlterRespuesta_WhenErrorSaveChanges_ReturnStatusCode()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            var respuestasEntity = new everisapi.API.Models.RespuestaConUserDto {
                Id = 1,
                PreguntaId = 1,
                Estado = 1,
                EvaluacionId = 1,
                UserName = "fmoreno"
            };

            mockRepository.Setup(r => r.ExiteRespuesta(1)).Returns(true);
            mockRepository.Setup(r => r.UpdateRespuesta(respuestasEntity)).Returns(true);
            mockRepository.Setup(r => r.SaveChanges()).Returns(false);

            //Act
            var okResult = _controller.AlterRespuesta(RespuestaUpdate: respuestasEntity);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method:  GetRespuestasConNotas
        [Fact]
        public void GetRespuestasConNotas_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            mockRepository.Setup(r => r.GetRespuestasConNotas(1,0)).Returns(new List<everisapi.API.Models.RespuestaConNotasDto>());

            //Act
            var okResult = _controller.GetRespuestasConNotas(idevaluacion: 1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetRespuestasConNotas_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            mockRepository.Setup(r => r.GetRespuestasConNotas(1,0)).Throws(new Exception());

            //Act
            var okResult = _controller.GetRespuestasConNotas(idevaluacion: 1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method:  GetRespuestasConNotasConAssessments
        [Fact]
        public void GetRespuestasConNotasConAssessments_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            mockRepository.Setup(r => r.GetRespuestasConNotas(1,1)).Returns(new List<everisapi.API.Models.RespuestaConNotasDto>());

            //Act
            var okResult = _controller.GetRespuestasConNotasConAssessments(idevaluacion: 1, assessmentid: 1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetRespuestasConNotasConAssessments_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            mockRepository.Setup(r => r.GetRespuestasConNotas(1,1)).Throws(new Exception());

            //Act
            var okResult = _controller.GetRespuestasConNotasConAssessments(idevaluacion: 1, assessmentid: 1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

//Method:  GetPreguntasNivelOrganizadas
        [Fact]
        public void GetPreguntasNivelOrganizadas_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            mockRepository.Setup(r => r.GetPreguntasNivelOrganizadas(1,1,1)).Returns(new List<everisapi.API.Models.SectionConAsignacionesDto>());

            //Act
            var okResult = _controller.GetPreguntasNivelOrganizadas(1, 1, 1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetPreguntasNivelOrganizadas_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            mockRepository.Setup(r => r.GetPreguntasNivelOrganizadas(1,1,1)).Throws(new Exception());

            //Act
            var okResult = _controller.GetPreguntasNivelOrganizadas(1, 1, 1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method:  AddRespuesta
        [Fact]
        public void AddRespuesta_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            var respuestasDto = new everisapi.API.Models.RespuestaDto {
                Id = 1,
                PreguntaId = 1,
                Estado = 1,
                EvaluacionId = 1
            };

            mockRepository.Setup(r => r.AddRespuesta(It.IsAny<everisapi.API.Models.RespuestaDto>())).Returns(true);

            //Act
            var okResult = _controller.AddRespuesta(RespuestaAdd: respuestasDto);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void AddRespuesta_WhenCalled_ReturnBadRequest()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            var respuestasDto = new everisapi.API.Models.RespuestaDto {
                Id = 1,
                PreguntaId = 1,
                Estado = 1,
                EvaluacionId = 1
            };

            mockRepository.Setup(r => r.AddRespuesta(It.IsAny<everisapi.API.Models.RespuestaDto>())).Returns(false);

            //Act
            var okResult = _controller.AddRespuesta(RespuestaAdd: respuestasDto);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddRespuesta_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            var respuestasDto = new everisapi.API.Models.RespuestaDto {
                Id = 1,
                PreguntaId = 1,
                Estado = 1,
                EvaluacionId = 1
            };

            mockRepository.Setup(r => r.ExiteRespuesta(1)).Returns(false);
            mockRepository.Setup(r => r.AddRespuesta(It.IsAny<everisapi.API.Models.RespuestaDto>())).Throws(new Exception());

            //Act
            var okResult = _controller.AddRespuesta(RespuestaAdd: respuestasDto);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

       [Fact]
        public void AddRespuesta_WhenExitRespuesta_ReturnNotFound()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            var respuestasDto = new everisapi.API.Models.RespuestaDto {
                Id = 1,
                PreguntaId = 1,
                Estado = 1,
                EvaluacionId = 1
            };

            mockRepository.Setup(r => r.ExiteRespuesta(1)).Returns(true);

            //Act
            var okResult = _controller.AddRespuesta(RespuestaAdd: respuestasDto);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddRespuesta_WhenRespuestaIsNull_ReturnNotFound()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            //Act
            var okResult = _controller.AddRespuesta(RespuestaAdd: null);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddRespuesta_GivenInvalidModel_ReturnsBadRequest()
        {
            //Arrange
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            _controller.ModelState.AddModelError("error", "some error");

            var respuestasDto = new everisapi.API.Models.RespuestaDto {
                Id = 1,
                PreguntaId = 1,
                Estado = 1,
                EvaluacionId = 1
            };

            //Act
            mockRepository.Setup(r => r.ExiteRespuesta(1)).Returns(false);
            mockRepository.Setup(r => r.AddRespuesta(It.IsAny<everisapi.API.Models.RespuestaDto>())).Returns(true);

            var okResult = _controller.AddRespuesta(RespuestaAdd: respuestasDto);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        //Method:  DeleteRespuesta
        [Fact]
        public void DeleteRespuesta_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            var respuestasDto = new everisapi.API.Models.RespuestaDto {
                Id = 1,
                PreguntaId = 1,
                Estado = 1,
                EvaluacionId = 1
            };

            mockRepository.Setup(r => r.ExiteRespuesta(1)).Returns(true);
            mockRepository.Setup(r => r.DeleteRespuesta(It.IsAny<everisapi.API.Models.RespuestaDto>())).Returns(true);

            //Act
            var okResult = _controller.DeleteRespuesta(RespuestaDelete: respuestasDto);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void DeleteRespuesta_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            var respuestasDto = new everisapi.API.Models.RespuestaDto {
                Id = 1,
                PreguntaId = 1,
                Estado = 1,
                EvaluacionId = 1
            };

            mockRepository.Setup(r => r.ExiteRespuesta(1)).Returns(true);
            mockRepository.Setup(r => r.DeleteRespuesta(It.IsAny<everisapi.API.Models.RespuestaDto>())).Throws(new Exception());

            //Act
            var okResult = _controller.DeleteRespuesta(RespuestaDelete: respuestasDto);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

       [Fact]
        public void DeleteRespuesta_WhenExitRespuesta_ReturnBadRequestResult()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            var respuestasDto = new everisapi.API.Models.RespuestaDto {
                Id = 1,
                PreguntaId = 1,
                Estado = 1,
                EvaluacionId = 1
            };

            mockRepository.Setup(r => r.ExiteRespuesta(1)).Returns(true);

            //Act
            var okResult = _controller.DeleteRespuesta(RespuestaDelete: respuestasDto);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void DeleteRespuesta_WhenRespuestaIsNull_ReturnNotFound()
        {
            //Arrange            
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            //Act
            var okResult = _controller.DeleteRespuesta(RespuestaDelete: null);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void DeleteRespuesta_GivenInvalidModel_ReturnsBadRequest()
        {
            //Arrange
            _controller = new RespuestaController(_logger, _respuestasInfoRepository);

            _controller.ModelState.AddModelError("error", "some error");

            var respuestasDto = new everisapi.API.Models.RespuestaDto {
                Id = 1,
                PreguntaId = 1,
                Estado = 1,
                EvaluacionId = 1
            };

            //Act
            mockRepository.Setup(r => r.ExiteRespuesta(1)).Returns(true);
            // mockRepository.Setup(r => r.DeleteRespuesta(It.IsAny<everisapi.API.Models.RespuestaDto>())).Returns(true);

            var okResult = _controller.DeleteRespuesta(RespuestaDelete: respuestasDto);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

    }
}