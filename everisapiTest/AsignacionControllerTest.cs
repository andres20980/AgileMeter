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
    public class AsignacionControllerTest
    {
        AsignacionController _controller;
        private readonly ILogger<AsignacionController> _logger;

        private readonly IAsignacionInfoRepository _asignacionInfoRepository;

        Mock<ILogger<AsignacionController>> mockLogger;
        Mock<IAsignacionInfoRepository> mockRepository;

        public AsignacionControllerTest()
        {
            mockLogger = new Mock<ILogger<AsignacionController>>();
            _logger = mockLogger.Object;

            mockRepository = new Mock<IAsignacionInfoRepository>();
            _asignacionInfoRepository = mockRepository.Object;

            var autoMapperInstance = AutoMapperConfig.Instance;
        }

        //Method: GetAsignaciones()
        [Fact]
        public void GetAsignaciones_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignaciones = new List<everisapi.API.Entities.AsignacionEntity>()
            {
                new everisapi.API.Entities.AsignacionEntity {
                    Id = 1, Nombre = "Asignacion_1"
                },
                new everisapi.API.Entities.AsignacionEntity {
                    Id = 2, Nombre = "Asignacion_2"
                },
            };

            mockRepository.Setup(r => r.GetAsignaciones()).Returns(asignaciones);

            //Act
            var okResult = _controller.GetAsignaciones();

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetAsignaciones_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            mockRepository.Setup(r => r.GetAsignaciones()).Throws(new Exception());


            //Act
            var okResult = _controller.GetAsignaciones();

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: AssignationLastQuestionUpdated(int evaluationId)
        [Fact]
        public void AssignationLastQuestionUpdated_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignacion = new everisapi.API.Entities.AsignacionEntity 
            {
                Id = 1, 
                Nombre = "Asignacion_1"
            };

            mockRepository.Setup(r => r.AssignationLastQuestionUpdated(It.IsAny<int>())).Returns(asignacion);

            //Act
            var okResult = _controller.AssignationLastQuestionUpdated(1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void AssignationLastQuestionUpdated_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            mockRepository.Setup(r => r.AssignationLastQuestionUpdated(It.IsAny<int>())).Throws(new Exception());


            //Act
            var okResult = _controller.AssignationLastQuestionUpdated(1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetAsignacionFromEval(int id)
        [Fact]
        public void GetAsignacionFromEval_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignaciones = new List<everisapi.API.Models.AsignacionInfoDto >{
                new everisapi.API.Models.AsignacionInfoDto 
                {
                    Id = 1, 
                    Nombre = "Asignacion_1"
                },
                new everisapi.API.Models.AsignacionInfoDto 
                {
                    Id = 1, 
                    Nombre = "Asignacion_1"
                }};

            mockRepository.Setup(r => r.GetAsignFromEval(It.IsAny<int>())).Returns(asignaciones);

            //Act
            var okResult = _controller.GetAsignacionFromEval(1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetAsignacionFromEval_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            mockRepository.Setup(r => r.GetAsignFromEval(It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetAsignacionFromEval(1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetAsignacionFromEval(int idEval, int idAsig)
        [Fact]
        public void GetAsignacionFromEvalIdEvalIdAsig_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignacion = new everisapi.API.Models.AsignacionInfoDto 
                {
                    Id = 1, 
                    Nombre = "Asignacion_1"
                };

            mockRepository.Setup(r => r.GetAsignFromEvalAndAsig(It.IsAny<int>(), It.IsAny<int>())).Returns(asignacion);

            //Act
            var okResult = _controller.GetAsignacionFromEval(1,1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetAsignacionFromEvalIdEvalIdAsig_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            mockRepository.Setup(r => r.GetAsignFromEvalAndAsig(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetAsignacionFromEval(1,1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetAsignacionFromSection(int idEval, int idSection)
        [Fact]
        public void GetAsignacionFromSection_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignaciones = new List<everisapi.API.Models.AsignacionInfoDto >{
                new everisapi.API.Models.AsignacionInfoDto 
                {
                    Id = 1, 
                    Nombre = "Asignacion_1"
                },
                new everisapi.API.Models.AsignacionInfoDto 
                {
                    Id = 1, 
                    Nombre = "Asignacion_1"
                }};

            mockRepository.Setup(r => r.GetAsignFromEvalAndSection(It.IsAny<int>(), It.IsAny<int>())).Returns(asignaciones);

            //Act
            var okResult = _controller.GetAsignacionFromSection(1,1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetAsignacionFromSection_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            mockRepository.Setup(r => r.GetAsignFromEvalAndSection(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetAsignacionFromSection(1,1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetAsignacion(int id, bool IncluirPreguntas = false)
        [Fact]
        public void GetAsignacion_WhenCalledWithPreguntas_ReturnOkResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignacion = new everisapi.API.Entities.AsignacionEntity 
                {
                    Id = 1, 
                    Nombre = "Asignacion_1"
                };

            mockRepository.Setup(r => r.GetAsignacion(It.IsAny<int>(), It.IsAny<bool>())).Returns(asignacion);

            //Act
            var okResult = _controller.GetAsignacion(1,true);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetAsignacion_WhenCalledWithoutPreguntas_ReturnOkResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignacion = new everisapi.API.Entities.AsignacionEntity 
                {
                    Id = 1, 
                    Nombre = "Asignacion_1"
                };

            mockRepository.Setup(r => r.GetAsignacion(It.IsAny<int>(), It.IsAny<bool>())).Returns(asignacion);

            //Act
            var okResult = _controller.GetAsignacion(1,false);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetAsignacion_WhenCalledNull_ReturnsNotFoundResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            everisapi.API.Entities.AsignacionEntity  asignacion = null;

            mockRepository.Setup(r => r.GetAsignacion(It.IsAny<int>(), It.IsAny<bool>())).Returns(asignacion);

            //Act
            var okResult = _controller.GetAsignacion(1,true);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void GetAsignacion_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            mockRepository.Setup(r => r.GetAsignacion(It.IsAny<int>(), It.IsAny<bool>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetAsignacion(1,true);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: AddAsignacion([FromBody] AsignacionCreateUpdateDto AsignacionAdd)
        [Fact]
        public void AddAsignacion_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignacion = new everisapi.API.Models.AsignacionCreateUpdateDto 
                {
                    Id = 1, 
                    Nombre = "Asignacion_1"
                };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.AddAsig(It.IsAny<everisapi.API.Entities.AsignacionEntity>())).Returns(true);

            //Act
            var okResult = _controller.AddAsignacion(asignacion);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void AddAsignacion_WhenCalled_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignacion = new everisapi.API.Models.AsignacionCreateUpdateDto 
                {
                    Id = 1, 
                    Nombre = "Asignacion_1"
                };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.AddAsig(It.IsAny<everisapi.API.Entities.AsignacionEntity>())).Returns(false);

            //Act
            var okResult = _controller.AddAsignacion(asignacion);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddAsignacion_WhenCalledWithExitAsignacion_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignacion = new everisapi.API.Models.AsignacionCreateUpdateDto 
                {
                    Id = 1, 
                    Nombre = "Asignacion_1"
                };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.AddAsig(It.IsAny<everisapi.API.Entities.AsignacionEntity>())).Returns(true);

            //Act
            var okResult = _controller.AddAsignacion(asignacion);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddAsignacion_WhenCalledNull_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            everisapi.API.Models.AsignacionCreateUpdateDto  asignacion = null;

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.AddAsig(It.IsAny<everisapi.API.Entities.AsignacionEntity>())).Returns(true);

            //Act
            var okResult = _controller.AddAsignacion(asignacion);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddAsignacion_WhenCalledWithWithInValidModel_ReturnsBadRequestObjectResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var asignacion = new everisapi.API.Models.AsignacionCreateUpdateDto 
                {
                    Id = 1, 
                    Nombre = "Asignacion_1"
                };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.AddAsig(It.IsAny<everisapi.API.Entities.AsignacionEntity>())).Returns(true);

            //Act
            var okResult = _controller.AddAsignacion(asignacion);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        //Method: UpdateAsignacion([FromBody] AsignacionCreateUpdateDto AsignacionUpdate)
        [Fact]
        public void UpdateAsignacion_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignacion = new everisapi.API.Models.AsignacionCreateUpdateDto 
                {
                    Id = 1, 
                    Nombre = "Asignacion_1"
                };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.AlterAsig(It.IsAny<everisapi.API.Entities.AsignacionEntity>())).Returns(true);

            //Act
            var okResult = _controller.UpdateAsignacion(asignacion);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void UpdateAsignacion_WhenCalled_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignacion = new everisapi.API.Models.AsignacionCreateUpdateDto 
                {
                    Id = 1, 
                    Nombre = "Asignacion_1"
                };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.AlterAsig(It.IsAny<everisapi.API.Entities.AsignacionEntity>())).Returns(false);

            //Act
            var okResult = _controller.UpdateAsignacion(asignacion);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void UpdateAsignacion_WhenCalledWithExitAsignacion_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignacion = new everisapi.API.Models.AsignacionCreateUpdateDto 
                {
                    Id = 1, 
                    Nombre = "Asignacion_1"
                };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.AlterAsig(It.IsAny<everisapi.API.Entities.AsignacionEntity>())).Returns(true);

            //Act
            var okResult = _controller.UpdateAsignacion(asignacion);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void UpdateAsignacion_WhenCalledNull_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            everisapi.API.Models.AsignacionCreateUpdateDto  asignacion = null;

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.AlterAsig(It.IsAny<everisapi.API.Entities.AsignacionEntity>())).Returns(true);

            //Act
            var okResult = _controller.UpdateAsignacion(asignacion);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void UpdateAsignacion_WhenCalledWithWithInValidModel_ReturnsBadRequestObjectResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var asignacion = new everisapi.API.Models.AsignacionCreateUpdateDto 
                {
                    Id = 1, 
                    Nombre = "Asignacion_1"
                };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.AlterAsig(It.IsAny<everisapi.API.Entities.AsignacionEntity>())).Returns(true);

            //Act
            var okResult = _controller.UpdateAsignacion(asignacion);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        //Method: AddNotasAsignacion([FromBody] AsignacionUpdateNotasDto AsignacionUpdate)
        [Fact]
        public void AddNotasAsignacion_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignacion = new everisapi.API.Models.AsignacionUpdateNotasDto 
                {
                    Id = 1,
                    EvId = 1, 
                    Notas = "Notas_1"
                };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.AddNotas(It.IsAny<everisapi.API.Models.AsignacionUpdateNotasDto>())).Returns(true);

            //Act
            var okResult = _controller.AddNotasAsignacion(asignacion);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void AddNotasAsignacion_WhenCalled_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignacion = new everisapi.API.Models.AsignacionUpdateNotasDto 
                {
                    Id = 1,
                    EvId = 1, 
                    Notas = "Notas_1"
                };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.AddNotas(It.IsAny<everisapi.API.Models.AsignacionUpdateNotasDto>())).Returns(false);

            //Act
            var okResult = _controller.AddNotasAsignacion(asignacion);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddNotasAsignacion_WhenCalledWithExitAsignacion_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignacion = new everisapi.API.Models.AsignacionUpdateNotasDto 
                {
                    Id = 1,
                    EvId = 1, 
                    Notas = "Notas_1"
                };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.AddNotas(It.IsAny<everisapi.API.Models.AsignacionUpdateNotasDto>())).Returns(true);

            //Act
            var okResult = _controller.AddNotasAsignacion(asignacion);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddNotasAsignacion_WhenCalledNull_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            everisapi.API.Models.AsignacionUpdateNotasDto  asignacion = null;

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.AddNotas(It.IsAny<everisapi.API.Models.AsignacionUpdateNotasDto>())).Returns(true);

            //Act
            var okResult = _controller.AddNotasAsignacion(asignacion);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddNotasAsignacion_WhenCalledWithWithInValidModel_ReturnsBadRequestObjectResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var asignacion = new everisapi.API.Models.AsignacionUpdateNotasDto 
                {
                    Id = 1,
                    EvId = 1, 
                    Notas = "Notas_1"
                };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.AddNotas(It.IsAny<everisapi.API.Models.AsignacionUpdateNotasDto>())).Returns(true);

            //Act
            var okResult = _controller.AddNotasAsignacion(asignacion);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        [Fact]
        public void AddNotasAsignacion_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignacion = new everisapi.API.Models.AsignacionUpdateNotasDto 
                {
                    Id = 1,
                    EvId = 1, 
                    Notas = "Notas_1"
                };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.AddNotas(It.IsAny<everisapi.API.Models.AsignacionUpdateNotasDto>())).Throws(new Exception());

            //Act
            var okResult = _controller.AddNotasAsignacion(asignacion);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetNotasAsignaciones(int id)
        [Fact]
        public void GetNotasAsignaciones_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignaciones = new List<everisapi.API.Models.AsignacionConNotasDto>{
                new everisapi.API.Models.AsignacionConNotasDto{
                    Section = "Section_1",
                    Asignacion = "Asignacion_1",
                    Notas = "Nota_1",
                }
            };

            mockRepository.Setup(r => r.GetAsignConNotas(It.IsAny<int>())).Returns(asignaciones);

            //Act
            var okResult = _controller.GetNotasAsignaciones(1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetNotasAsignaciones_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignaciones = new List<everisapi.API.Models.AsignacionConNotasDto>{
                new everisapi.API.Models.AsignacionConNotasDto{
                    Section = "Section_1",
                    Asignacion = "Asignacion_1",
                    Notas = "Nota_1",
                }
            };

            mockRepository.Setup(r => r.GetAsignConNotas(It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetNotasAsignaciones(1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: DeleteAsignacion([FromBody] AsignacionCreateUpdateDto AsignacionDelete)
        [Fact]
        public void DeleteAsignacion_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignacion = new everisapi.API.Models.AsignacionCreateUpdateDto 
                {
                    Id = 1,
                    Nombre = "Asignacion_1", 
                    SectionId = 1
                };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.DeleteAsig(It.IsAny<everisapi.API.Entities.AsignacionEntity>())).Returns(true);

            //Act
            var okResult = _controller.DeleteAsignacion(asignacion);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void DeleteAsignacion_WhenCalled_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignacion = new everisapi.API.Models.AsignacionCreateUpdateDto 
                {
                    Id = 1,
                    Nombre = "Asignacion_1", 
                    SectionId = 1
                };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.DeleteAsig(It.IsAny<everisapi.API.Entities.AsignacionEntity>())).Returns(false);

            //Act
            var okResult = _controller.DeleteAsignacion(asignacion);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void DeleteAsignacion_WhenCalledWithExitAsignacion_ReturnsBadRequestResult()
        {
            //Arrange                     
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            var asignacion = new everisapi.API.Models.AsignacionCreateUpdateDto 
                {
                    Id = 1,
                    Nombre = "Asignacion_1", 
                    SectionId = 1
                };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.DeleteAsig(It.IsAny<everisapi.API.Entities.AsignacionEntity>())).Returns(true);

            //Act
            var okResult = _controller.DeleteAsignacion(asignacion);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void DeleteAsignacion_WhenCalledNull_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);

            everisapi.API.Models.AsignacionCreateUpdateDto  asignacion = null;

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.DeleteAsig(It.IsAny<everisapi.API.Entities.AsignacionEntity>())).Returns(true);

            //Act
            var okResult = _controller.DeleteAsignacion(asignacion);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void ADeleteAsignacion_WhenCalledWithWithInValidModel_ReturnsBadRequestObjectResult()
        {
            //Arrange            
            _controller = new AsignacionController(_logger, _asignacionInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var asignacion = new everisapi.API.Models.AsignacionCreateUpdateDto 
                {
                    Id = 1,
                    Nombre = "Asignacion_1", 
                    SectionId = 1
                };

            mockRepository.Setup(r => r.AsignacionExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.DeleteAsig(It.IsAny<everisapi.API.Entities.AsignacionEntity>())).Returns(true);

            //Act
            var okResult = _controller.DeleteAsignacion(asignacion);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }
        
    }
}