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
    public class SectionControllerTest
    {
        SectionController _controller;
        private readonly ILogger<SectionController> _logger;

        private readonly ISectionsInfoRepository _sectionInfoRepository;

        Mock<ILogger<SectionController>> mockLogger;
        Mock<ISectionsInfoRepository> mockRepository;

        public SectionControllerTest()
        {
            mockLogger = new Mock<ILogger<SectionController>>();
            _logger = mockLogger.Object;

            mockRepository = new Mock<ISectionsInfoRepository>();
            _sectionInfoRepository = mockRepository.Object;

            var autoMapperInstance = AutoMapperConfig.Instance;
        }

        //Method: GetSections()
        [Fact]
        public void GetSections_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var sections = new List<everisapi.API.Entities.SectionEntity>()
            {
                new everisapi.API.Entities.SectionEntity {
                    Id = 1, Nombre = "Section_1"
                },
                new everisapi.API.Entities.SectionEntity {
                    Id = 2, Nombre = "Section_2"
                },
            };

            mockRepository.Setup(r => r.GetSections()).Returns(sections);

            //Act
            var okResult = _controller.GetSections();

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetSections_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var sections = new List<everisapi.API.Entities.SectionEntity>()
            {
                new everisapi.API.Entities.SectionEntity {
                    Id = 1, Nombre = "Section_1"
                },
                new everisapi.API.Entities.SectionEntity {
                    Id = 2, Nombre = "Section_2"
                },
            };

            mockRepository.Setup(r => r.GetSections()).Throws(new Exception());


            //Act
            var okResult = _controller.GetSections();

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetSectionsInfoFromSectionId(int evaluationId, int sectionId)
        [Fact]
        public void GetSectionsInfoFromSectionId_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Models.SectionInfoDto
            {
                Id = 1,
                Nombre = "Section_1"
            };

            mockRepository.Setup(r => r.GetSectionsInfoFromSectionId(It.IsAny<int>(), It.IsAny<int>())).Returns(section);

            //Act
            var okResult = _controller.GetSectionsInfoFromSectionId(1, 1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetSectionsInfoFromSectionId_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Models.SectionInfoDto
            {
                Id = 1,
                Nombre = "Section_1"
            };

            mockRepository.Setup(r => r.GetSectionsInfoFromSectionId(It.IsAny<int>(), It.IsAny<int>()))
                                            .Throws(new Exception());

            //Act
            var okResult = _controller.GetSectionsInfoFromSectionId(1, 1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetSection(int id, bool IncluirAsignaciones = false)
        [Fact]
        public void GetSection_WhenCalledIncluirAsignaciones_ReturnOkResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Section_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(section);

            //Act
            var okResult = _controller.GetSection(1, true);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(okResult);
            Assert.IsType<everisapi.API.Models.SectionDto>(okObjectResult.Value);
        }

        [Fact]
        public void GetSection_WhenCalledNoIncluirAsignaciones_ReturnOkResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Section_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(section);

            //Act
            var okResult = _controller.GetSection(1, false);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(okResult);
            Assert.IsType<everisapi.API.Models.SectionWithoutAreaDto>(okObjectResult.Value);
        }

        [Fact]
        public void GetSection_WhenCalledNull_ReturnNotFoundResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            everisapi.API.Entities.SectionEntity section = null;

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(section);

            //Act
            var okResult = _controller.GetSection(1, false);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void GetSection_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Section_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetSection(1, false);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetNumPreguntas(int id, int idevaluacion)
        [Fact]
        public void GetNumPreguntas_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Section_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(section);
            mockRepository.Setup(r => r.GetNumPreguntasFromSection(It.IsAny<int>(), It.IsAny<int>())).Returns(It.IsAny<int>());

            //Act
            var okResult = _controller.GetNumPreguntas(1, 1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetNumPreguntas_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Section_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(section);
            mockRepository.Setup(r => r.GetNumPreguntasFromSection(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetNumPreguntas(1, 1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        [Fact]
        public void GetNumPreguntas_WhenCalledNull_ReturnsNotFoundResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            everisapi.API.Entities.SectionEntity section = null;

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(section);
            mockRepository.Setup(r => r.GetNumPreguntasFromSection(It.IsAny<int>(), It.IsAny<int>())).Returns(It.IsAny<int>());

            //Act
            var okResult = _controller.GetNumPreguntas(1, 1);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        //Method: GetNumRespuestas(int id, int idevaluacion)
        [Fact]
        public void GetNumRespuestas_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Section_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(section);
            mockRepository.Setup(r => r.GetRespuestasCorrectasFromSection(It.IsAny<int>(), It.IsAny<int>())).Returns(It.IsAny<int>());

            //Act
            var okResult = _controller.GetNumRespuestas(1, 1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetNumRespuestas_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Section_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(section);
            mockRepository.Setup(r => r.GetRespuestasCorrectasFromSection(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetNumRespuestas(1, 1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        [Fact]
        public void GetNumRespuestas_WhenCalledNull_ReturnsNotFoundResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            everisapi.API.Entities.SectionEntity section = null;

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(section);
            mockRepository.Setup(r => r.GetRespuestasCorrectasFromSection(It.IsAny<int>(), It.IsAny<int>())).Returns(It.IsAny<int>());

            //Act
            var okResult = _controller.GetNumRespuestas(1, 1);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        //Method: GetDatosEvaluacionFromEval(int id)
        [Fact]
        public void GetDatosEvaluacionFromEval_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var sections = new List<everisapi.API.Models.SectionInfoDto>()
            {
                new everisapi.API.Models.SectionInfoDto {
                    Id = 1, Nombre = "Section_1"
                },
            };

            mockRepository.Setup(r => r.GetSectionsInfoFromEval(It.IsAny<int>())).Returns(sections);

            //Act
            var okResult = _controller.GetDatosEvaluacionFromEval(1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetDatosEvaluacionFromEval_WhenCalledNull_ReturnsNotFoundResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            List<everisapi.API.Models.SectionInfoDto> sections = null;

            mockRepository.Setup(r => r.GetSectionsInfoFromEval(It.IsAny<int>())).Returns(sections);

            //Act
            var okResult = _controller.GetDatosEvaluacionFromEval(1);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void GetDatosEvaluacionFromEval_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var sections = new List<everisapi.API.Models.SectionInfoDto>()
            {
                new everisapi.API.Models.SectionInfoDto {
                    Id = 1, Nombre = "Section_1"
                },
            };

            mockRepository.Setup(r => r.GetSectionsInfoFromEval(It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetDatosEvaluacionFromEval(1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetDatosEvaluacionFromEvalNew(int id,int assessmentId)
        [Fact]
        public void GetDatosEvaluacionFromEvalNew_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var sections = new List<everisapi.API.Models.SectionInfoDto>()
            {
                new everisapi.API.Models.SectionInfoDto {
                    Id = 1, Nombre = "Section_1"
                },
            };

            mockRepository.Setup(r => r.GetSectionsInfoFromEvalNew(It.IsAny<int>(), It.IsAny<int>())).Returns(sections);

            //Act
            var okResult = _controller.GetDatosEvaluacionFromEvalNew(1, 1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetDatosEvaluacionFromEvalNew_WhenCalledNull_ReturnsNotFoundResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            List<everisapi.API.Models.SectionInfoDto> sections = null;

            mockRepository.Setup(r => r.GetSectionsInfoFromEvalNew(It.IsAny<int>(), It.IsAny<int>())).Returns(sections);

            //Act
            var okResult = _controller.GetDatosEvaluacionFromEvalNew(1, 1);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void GetDatosEvaluacionFromEvalNew_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var sections = new List<everisapi.API.Models.SectionInfoDto>()
            {
                new everisapi.API.Models.SectionInfoDto {
                    Id = 1, Nombre = "Section_1"
                },
            };

            mockRepository.Setup(r => r.GetSectionsInfoFromEvalNew(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetDatosEvaluacionFromEvalNew(1, 1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetAsignacionesFromSection(int id)
        [Fact]
        public void GetAsignacionesFromSection_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Section_1"
            };

            var asignaciones = new List<everisapi.API.Entities.AsignacionEntity>{
                new everisapi.API.Entities.AsignacionEntity{
                    Id = 1,
                    Nombre = "Asignacion_1"
                }
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(section);
            mockRepository.Setup(r => r.GetAsignacionesFromSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(asignaciones);

            //Act
            var okResult = _controller.GetAsignacionesFromSection(1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetAsignacionesFromSection_WhenCalledNull_ReturnsNotFoundResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            everisapi.API.Entities.SectionEntity section = null;

            var asignaciones = new List<everisapi.API.Entities.AsignacionEntity>{
                new everisapi.API.Entities.AsignacionEntity{
                    Id = 1,
                    Nombre = "Asignacion_1"
                }
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(section);
            mockRepository.Setup(r => r.GetAsignacionesFromSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(asignaciones);

            //Act
            var okResult = _controller.GetAsignacionesFromSection(1);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void GetAsignacionesFromSection_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Section_1"
            };

            var asignaciones = new List<everisapi.API.Entities.AsignacionEntity>{
                new everisapi.API.Entities.AsignacionEntity{
                    Id = 1,
                    Nombre = "Asignacion_1"
                }
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(section);
            mockRepository.Setup(r => r.GetAsignacionesFromSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetAsignacionesFromSection(1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: AddAsignacion([FromBody] AsignacionCreateUpdateDto AsignacionAdd)
        [Fact]
        public void AddAsignacion_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Models.SectionWithoutAreaDto
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            everisapi.API.Entities.SectionEntity returnsSection = null;

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(returnsSection);
            mockRepository.Setup(r => r.AddSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(true);

            //Act
            var okResult = _controller.AddAsignacion(section);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void AddAsignacion_WhenCalled_BadRequestResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Models.SectionWithoutAreaDto
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            everisapi.API.Entities.SectionEntity returnsSection = null;

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(returnsSection);
            mockRepository.Setup(r => r.AddSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(false);

            //Act
            var okResult = _controller.AddAsignacion(section);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddAsignacion_WhenCalledWithReturnhNotNull__ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Models.SectionWithoutAreaDto
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            var returnsSection = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            mockRepository.Setup(r => r.GetSection(1, false)).Returns(returnsSection);
            mockRepository.Setup(r => r.AddSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(true);

            //Act
            var okResult = _controller.AddAsignacion(section);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddAsignacion_WhenCalledWithNull__ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            everisapi.API.Models.SectionWithoutAreaDto section = null;
            var returnsSection = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            mockRepository.Setup(r => r.GetSection(1, false)).Returns(returnsSection);
            mockRepository.Setup(r => r.AddSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(true);

            //Act
            var okResult = _controller.AddAsignacion(section);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddAsignacion_WhenCalledWithNullAndReturnsNull__ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            everisapi.API.Models.SectionWithoutAreaDto section = null;
            everisapi.API.Entities.SectionEntity returnsSection = null;

            mockRepository.Setup(r => r.GetSection(1, false)).Returns(returnsSection);
            mockRepository.Setup(r => r.AddSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(true);

            //Act
            var okResult = _controller.AddAsignacion(section);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddAsignacion_WhenCalledWithWithInValidModel_ReturnsBadRequestObjectResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var section = new everisapi.API.Models.SectionWithoutAreaDto
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            everisapi.API.Entities.SectionEntity returnsSection = null;

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(returnsSection);
            mockRepository.Setup(r => r.AddSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(true);

            //Act
            var okResult = _controller.AddAsignacion(section);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        //Method: UpdateSection([FromBody] SectionWithoutAreaDto SectionUpdate)
        [Fact]
        public void UpdateSection_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Models.SectionWithoutAreaDto
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            var returnsSection = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(returnsSection);
            mockRepository.Setup(r => r.AlterSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(true);

            //Act
            var okResult = _controller.UpdateSection(section);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void UpdateSection_WhenCalled_BadRequestResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Models.SectionWithoutAreaDto
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            var returnsSection = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(returnsSection);
            mockRepository.Setup(r => r.AlterSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(false);

            //Act
            var okResult = _controller.UpdateSection(section);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void UpdateSection_WhenCalledWithReturnhNull__ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Models.SectionWithoutAreaDto
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            everisapi.API.Entities.SectionEntity returnsSection = null;

            mockRepository.Setup(r => r.GetSection(1, false)).Returns(returnsSection);
            mockRepository.Setup(r => r.AlterSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(true);

            //Act
            var okResult = _controller.UpdateSection(section);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void UpdateSection_WhenCalledWithNull__ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            everisapi.API.Models.SectionWithoutAreaDto section = null;
            var returnsSection = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            mockRepository.Setup(r => r.GetSection(1, false)).Returns(returnsSection);
            mockRepository.Setup(r => r.AlterSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(true);

            //Act
            var okResult = _controller.UpdateSection(section);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void UpdateSection_WhenCalledWithWithInValidModel_ReturnsBadRequestObjectResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var section = new everisapi.API.Models.SectionWithoutAreaDto
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            var returnsSection = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(returnsSection);
            mockRepository.Setup(r => r.AlterSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(true);

            //Act
            var okResult = _controller.UpdateSection(section);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        //Method: AddNotas([FromBody] SectionWithNotasDto SectionUpdate)
        [Fact]
        public void AddNotas_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Models.SectionWithNotasDto
            {
                EvaluacionId = 1,
                SectionId = 1,
                Notas = "Notas_1"
            };

            var returnsSection = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(returnsSection);
            mockRepository.Setup(r => r.AddNotasSection(It.IsAny<everisapi.API.Models.SectionWithNotasDto>())).Returns(true);

            //Act
            var okResult = _controller.AddNotas(section);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void AddNotas_WhenCalled_BadRequestResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Models.SectionWithNotasDto
            {
                EvaluacionId = 1,
                SectionId = 1,
                Notas = "Notas_1"
            };

            var returnsSection = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(returnsSection);
            mockRepository.Setup(r => r.AddNotasSection(It.IsAny<everisapi.API.Models.SectionWithNotasDto>())).Returns(false);

            //Act
            var okResult = _controller.AddNotas(section);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddNotas_WhenCalledWithReturnhNull__ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Models.SectionWithNotasDto
            {
                EvaluacionId = 1,
                SectionId = 1,
                Notas = "Notas_1"
            };

            everisapi.API.Entities.SectionEntity returnsSection = null;

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(returnsSection);
            mockRepository.Setup(r => r.AddNotasSection(It.IsAny<everisapi.API.Models.SectionWithNotasDto>())).Returns(true);

            //Act
            var okResult = _controller.AddNotas(section);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddNotas_WhenCalledWithNull__ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            everisapi.API.Models.SectionWithNotasDto section = null;
            var returnsSection = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(returnsSection);
            mockRepository.Setup(r => r.AddNotasSection(It.IsAny<everisapi.API.Models.SectionWithNotasDto>())).Returns(true);

            //Act
            var okResult = _controller.AddNotas(section);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddNotas_WhenCalledWithWithInValidModel_ReturnsBadRequestObjectResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var section = new everisapi.API.Models.SectionWithNotasDto
            {
                EvaluacionId = 1,
                SectionId = 1,
                Notas = "Notas_1"
            };

            var returnsSection = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(returnsSection);
            mockRepository.Setup(r => r.AddNotasSection(It.IsAny<everisapi.API.Models.SectionWithNotasDto>())).Returns(true);

            //Act
            var okResult = _controller.AddNotas(section);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        [Fact]
        public void AddNotas_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Models.SectionWithNotasDto
            {
                EvaluacionId = 1,
                SectionId = 1,
                Notas = "Notas_1"
            };

            var returnsSection = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(returnsSection);
            mockRepository.Setup(r => r.AddNotasSection(It.IsAny<everisapi.API.Models.SectionWithNotasDto>())).Throws(new Exception());

            //Act
            var okResult = _controller.AddNotas(section);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: DeleteSection([FromBody] SectionWithoutAreaDto SectionDelete)
        [Fact]
        public void DeleteSection_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Models.SectionWithoutAreaDto
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            var returnsSection = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(returnsSection);
            mockRepository.Setup(r => r.DeleteSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(true);

            //Act
            var okResult = _controller.DeleteSection(section);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void DeleteSection_WhenCalled_BadRequestResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Models.SectionWithoutAreaDto
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            var returnsSection = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(returnsSection);
            mockRepository.Setup(r => r.DeleteSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(false);

            //Act
            var okResult = _controller.DeleteSection(section);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void DeleteSection_WhenCalledWithReturnhNull__ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            var section = new everisapi.API.Models.SectionWithoutAreaDto
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            everisapi.API.Entities.SectionEntity returnsSection = null;

            mockRepository.Setup(r => r.GetSection(1, false)).Returns(returnsSection);
            mockRepository.Setup(r => r.DeleteSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(true);

            //Act
            var okResult = _controller.DeleteSection(section);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void DeleteSection_WhenCalledWithNull__ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);

            everisapi.API.Models.SectionWithoutAreaDto section = null;
            var returnsSection = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            mockRepository.Setup(r => r.GetSection(1, false)).Returns(returnsSection);
            mockRepository.Setup(r => r.DeleteSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(true);

            //Act
            var okResult = _controller.DeleteSection(section);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void DeleteSection_WhenCalledWithWithInValidModel_ReturnsBadRequestObjectResult()
        {
            //Arrange            
            _controller = new SectionController(_logger, _sectionInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var section = new everisapi.API.Models.SectionWithoutAreaDto
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            var returnsSection = new everisapi.API.Entities.SectionEntity
            {
                Id = 1,
                Nombre = "Asignacion_1"
            };

            mockRepository.Setup(r => r.GetSection(It.IsAny<int>(), It.IsAny<bool>())).Returns(returnsSection);
            mockRepository.Setup(r => r.DeleteSection(It.IsAny<everisapi.API.Entities.SectionEntity>())).Returns(true);

            //Act
            var okResult = _controller.DeleteSection(section);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }
        

    }
}