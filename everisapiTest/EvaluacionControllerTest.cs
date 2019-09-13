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

        private readonly IUsersInfoRepository _usersInfoRepository;

        Mock<ILogger<EvaluacionController>> mockLogger;
        Mock<IEvaluacionInfoRepository> mockRepository;
        Mock<IUsersInfoRepository> mockRepositoryUsersInfo;

        public EvaluacionControllerTest()
        {
            mockLogger = new Mock<ILogger<EvaluacionController>>();
            _logger = mockLogger.Object;

            mockRepository = new Mock<IEvaluacionInfoRepository>();
            _evaluacionInfoRepository = mockRepository.Object;

            mockRepositoryUsersInfo = new Mock<IUsersInfoRepository>();
            _usersInfoRepository = mockRepositoryUsersInfo.Object;

            var autoMapperInstance = AutoMapperConfig.Instance;
        }

        //Method: GetEvaluaciones()
        [Fact]
        public void GetEvaluaciones_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

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
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            mockRepository.Setup(r => r.GetEvaluaciones()).Throws(new Exception());

            //Act
            var okResult = _controller.GetEvaluaciones();

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetEvaluacion(int id, bool IncluirRespuestas = false)
        [Fact]
        public void GetEvaluacion_WhenCalledWithRespuesta_ReturnsOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

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
        public void GetEvaluacion_WhenCalledWithoutRespuesta_ReturnsOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

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
        public void GetEvaluacion_WhenCalledNullEvaluacion_ReturnsNotFoundResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

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
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            mockRepository.Setup(r => r.GetEvaluacion(It.IsAny<int>(),It.IsAny<bool>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetEvaluacion(1,true);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetEvaluationInfoFromIdEvaluation(int idEvaluacion)

        [Fact]
        public void GetEvaluationInfoFromIdEvaluation_WhenCalled_ReturnsOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluacionInfoDto = new  everisapi.API.Models.EvaluacionInfoDto{
                Id = 1, Fecha = new DateTime()
            };

            mockRepository.Setup(r => r.GetEvaluationInfoFromIdEvaluation(It.IsAny<int>())).Returns(evaluacionInfoDto);

            //Act
            var okResult = _controller.GetEvaluationInfoFromIdEvaluation(1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetEvaluationInfoFromIdEvaluation_WhenCalledNullEvaluacion_ReturnNotFoundResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            everisapi.API.Models.EvaluacionInfoDto evaluacionInfoDto = null;

            mockRepository.Setup(r => r.GetEvaluationInfoFromIdEvaluation(It.IsAny<int>())).Returns(evaluacionInfoDto);

            //Act
            var okResult = _controller.GetEvaluationInfoFromIdEvaluation(1);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void GetEvaluationInfoFromIdEvaluation_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            mockRepository.Setup(r => r.GetEvaluationInfoFromIdEvaluation(It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetEvaluationInfoFromIdEvaluation(1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetEvaluacionInfo(int id)

        [Fact]
        public void GetEvaluacionInfo_WhenCalled_ReturnsOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluaciones = new  List<everisapi.API.Models.EvaluacionInfoDto>{
                new everisapi.API.Models.EvaluacionInfoDto{Id = 1, Fecha = new DateTime()}
            };

            mockRepository.Setup(r => r.GetEvaluationInfo(It.IsAny<int>())).Returns(evaluaciones);

            //Act
            var okResult = _controller.GetEvaluacionInfo(1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetEvaluacionInfo_WhenCalledNullEvaluacion_ReturnNotFoundResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            List<everisapi.API.Models.EvaluacionInfoDto> evaluacionesInfoDto= null;

            mockRepository.Setup(r => r.GetEvaluationInfo(It.IsAny<int>())).Returns(evaluacionesInfoDto);

            //Act
            var okResult = _controller.GetEvaluacionInfo(1);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void GetEvaluacionInfo_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            mockRepository.Setup(r => r.GetEvaluationInfo(It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetEvaluacionInfo(1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetEvaluacionInfoAndPage(int id, int pageNumber)

        [Fact]
        public void GetEvaluacionInfoAndPage_WhenCalled_ReturnsOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluaciones = new  List<everisapi.API.Models.EvaluacionInfoDto>{
                new everisapi.API.Models.EvaluacionInfoDto{Id = 1, Fecha = new DateTime()}
            };

            mockRepository.Setup(r => r.GetEvaluationInfoAndPage(It.IsAny<int>(), It.IsAny<int>())).Returns(evaluaciones);

            //Act
            var okResult = _controller.GetEvaluacionInfoAndPage(1,1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetEvaluacionInfoAndPage_WhenCalledNullEvaluacion_ReturnNotFoundResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            List<everisapi.API.Models.EvaluacionInfoDto> evaluacionesInfoDto= null;

            mockRepository.Setup(r => r.GetEvaluationInfoAndPage(It.IsAny<int>(), It.IsAny<int>())).Returns(evaluacionesInfoDto);

            //Act
            var okResult = _controller.GetEvaluacionInfoAndPage(1,1);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void GetEvaluacionInfoAndPage_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            mockRepository.Setup(r => r.GetEvaluationInfoAndPage(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetEvaluacionInfoAndPage(1,1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetEvaluationInfoAndPageFiltered(int id, int pageNumber, [FromBody] EvaluacionInfoPaginationDto EvaluacionParaFiltrar)

        [Fact]
        public void GetEvaluationInfoAndPageFiltered_WhenCalledIdDistintZero_ReturnsOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluaciones = new  List<everisapi.API.Models.EvaluacionInfoDto>{
                new everisapi.API.Models.EvaluacionInfoDto{Id = 1, Fecha = new DateTime()}
            };

            mockRepository.Setup(r => r.GetEvaluationInfoAndPageFiltered(It.IsAny<int>(),
                                                It.IsAny<int>(),
                                                It.IsAny<everisapi.API.Models.EvaluacionInfoPaginationDto>() 
                                            )).Returns(evaluaciones);

            //Act
            var okResult = _controller.GetEvaluationInfoAndPageFiltered(
                            1,
                            1,
                            new everisapi.API.Models.EvaluacionInfoPaginationDto());

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetEvaluationInfoAndPageFiltered_WhenCalledIdEqualZero_ReturnsOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluaciones = new  List<everisapi.API.Models.EvaluacionInfoDto>{
                new everisapi.API.Models.EvaluacionInfoDto{Id = 1, Fecha = new DateTime()}
            };

            mockRepository.Setup(r => r.GetEvaluationInfoAndPageFilteredAdmin(
                                                It.IsAny<int>(),
                                                It.IsAny<everisapi.API.Models.EvaluacionInfoPaginationDto>() 
                                            )).Returns(evaluaciones);

            //Act
            var okResult = _controller.GetEvaluationInfoAndPageFiltered(
                            0,
                            1,
                            new everisapi.API.Models.EvaluacionInfoPaginationDto());

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetEvaluationInfoAndPageFiltered_WhenCalledInvalidModel_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var evaluaciones = new  List<everisapi.API.Models.EvaluacionInfoDto>{
                            new everisapi.API.Models.EvaluacionInfoDto{Id = 1, Fecha = new DateTime()}
                        };

            mockRepository.Setup(r => r.GetEvaluationInfoAndPageFiltered(It.IsAny<int>(),
                                                It.IsAny<int>(),
                                                It.IsAny<everisapi.API.Models.EvaluacionInfoPaginationDto>() 
                                            )).Returns(evaluaciones);

            //Act
            var okResult = _controller.GetEvaluationInfoAndPageFiltered(
                            1,
                            1,
                            new everisapi.API.Models.EvaluacionInfoPaginationDto());

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        [Fact]
        public void GetEvaluationInfoAndPageFiltered_WhenCalledWithNull_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluaciones = new  List<everisapi.API.Models.EvaluacionInfoDto>{
                            new everisapi.API.Models.EvaluacionInfoDto{Id = 1, Fecha = new DateTime()}
                        };

            everisapi.API.Models.EvaluacionInfoPaginationDto evaluacionInfoPaginationDto = null;

            mockRepository.Setup(r => r.GetEvaluationInfoAndPageFiltered(It.IsAny<int>(),
                                                It.IsAny<int>(),
                                                evaluacionInfoPaginationDto 
                                            )).Returns(evaluaciones);

            //Act
            var okResult = _controller.GetEvaluationInfoAndPageFiltered(
                            1,
                            1,
                            evaluacionInfoPaginationDto);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void GetEvaluationInfoAndPageFiltered_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            mockRepository.Setup(r => r.GetEvaluationInfoAndPageFiltered(It.IsAny<int>(),
                                                It.IsAny<int>(),
                                                It.IsAny<everisapi.API.Models.EvaluacionInfoPaginationDto>() 
                                            )).Throws(new Exception());

            //Act
            var okResult = _controller.GetEvaluationInfoAndPageFiltered(
                            1,
                            1,
                            new everisapi.API.Models.EvaluacionInfoPaginationDto());

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetEvaluationsWithSectionsInfo(int id, [FromBody] EvaluacionInfoPaginationDto EvaluacionParaFiltrar)

        [Fact]
        public void GetEvaluationsWithSectionsInfo_WhenCalled_ReturnsOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluaciones = new  List<everisapi.API.Models.EvaluacionInfoWithSectionsDto>{
                new everisapi.API.Models.EvaluacionInfoWithSectionsDto{Id = 1, Fecha = new DateTime()}
            };

            mockRepository.Setup(r => r.GetEvaluationsWithSectionsInfo(It.IsAny<int>(),
                                                It.IsAny<everisapi.API.Models.EvaluacionInfoPaginationDto>(),
                                                It.IsAny<int>() 
                                            )).Returns(evaluaciones);

            //Act
            var okResult = _controller.GetEvaluationsWithSectionsInfo(
                            1,
                            1,
                            new everisapi.API.Models.EvaluacionInfoPaginationDto());

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetEvaluationsWithSectionsInfo_WhenCalledInvalidModel_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var evaluaciones = new  List<everisapi.API.Models.EvaluacionInfoWithSectionsDto>{
                new everisapi.API.Models.EvaluacionInfoWithSectionsDto{Id = 1, Fecha = new DateTime()}
            };

            mockRepository.Setup(r => r.GetEvaluationsWithSectionsInfo(It.IsAny<int>(),
                                                It.IsAny<everisapi.API.Models.EvaluacionInfoPaginationDto>(),
                                                It.IsAny<int>()
                                            )).Returns(evaluaciones);

            //Act
            var okResult = _controller.GetEvaluationsWithSectionsInfo(
                            1,
                            1,
                            new everisapi.API.Models.EvaluacionInfoPaginationDto());

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        [Fact]
        public void GetEvaluationsWithSectionsInfo_WhenCalledWithNull_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluaciones = new  List<everisapi.API.Models.EvaluacionInfoWithSectionsDto>{
                new everisapi.API.Models.EvaluacionInfoWithSectionsDto{Id = 1, Fecha = new DateTime()}
            };

            everisapi.API.Models.EvaluacionInfoPaginationDto evaluacionInfoPaginationDto = null;

            mockRepository.Setup(r => r.GetEvaluationsWithSectionsInfo(It.IsAny<int>(),
                                                evaluacionInfoPaginationDto,
                                                It.IsAny<int>() 
                                            )).Returns(evaluaciones);

            //Act
            var okResult = _controller.GetEvaluationsWithSectionsInfo(
                            1,
                            1,
                            evaluacionInfoPaginationDto);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void GetEvaluationsWithSectionsInfo_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            mockRepository.Setup(r => r.GetEvaluationsWithSectionsInfo(It.IsAny<int>(),
                                                It.IsAny<everisapi.API.Models.EvaluacionInfoPaginationDto>(),
                                                It.IsAny<int>()  
                                            )).Throws(new Exception());

            //Act
            var okResult = _controller.GetEvaluationsWithSectionsInfo(
                            1,
                            1,
                            new everisapi.API.Models.EvaluacionInfoPaginationDto());

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetEvaluationsWithProgress(int id, [FromBody] EvaluacionInfoPaginationDto EvaluacionParaFiltrar)

        [Fact]
        public void GetEvaluationsWithProgress_WhenCalled_ReturnsOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluaciones = new  List<everisapi.API.Models.EvaluacionInfoWithProgressDto>{
                new everisapi.API.Models.EvaluacionInfoWithProgressDto{Id = 1, Fecha = new DateTime()}
            };

            mockRepository.Setup(r => r.GetEvaluationsWithProgress(It.IsAny<int>(),
                                                It.IsAny<everisapi.API.Models.EvaluacionInfoPaginationDto>() 
                                            )).Returns(evaluaciones);

            //Act
            var okResult = _controller.GetEvaluationsWithProgress(
                            1,
                            new everisapi.API.Models.EvaluacionInfoPaginationDto());

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetEvaluationsWithProgress_WhenCalledInvalidModel_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var evaluaciones = new  List<everisapi.API.Models.EvaluacionInfoWithProgressDto>{
                new everisapi.API.Models.EvaluacionInfoWithProgressDto{Id = 1, Fecha = new DateTime()}
            };

            mockRepository.Setup(r => r.GetEvaluationsWithProgress(It.IsAny<int>(),
                                                It.IsAny<everisapi.API.Models.EvaluacionInfoPaginationDto>() 
                                            )).Returns(evaluaciones);

            //Act
            var okResult = _controller.GetEvaluationsWithProgress(
                            1,
                            new everisapi.API.Models.EvaluacionInfoPaginationDto());

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        [Fact]
        public void GetEvaluationsWithProgress_WhenCalledWithNull_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluaciones = new  List<everisapi.API.Models.EvaluacionInfoWithProgressDto>{
                new everisapi.API.Models.EvaluacionInfoWithProgressDto{Id = 1, Fecha = new DateTime()}
            };

            everisapi.API.Models.EvaluacionInfoPaginationDto evaluacionInfoPaginationDto = null;

            mockRepository.Setup(r => r.GetEvaluationsWithProgress(It.IsAny<int>(),
                                                evaluacionInfoPaginationDto 
                                            )).Returns(evaluaciones);

            //Act
            var okResult = _controller.GetEvaluationsWithProgress(
                            1,
                            evaluacionInfoPaginationDto);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void GetEvaluationsWithProgress_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            mockRepository.Setup(r => r.GetEvaluationsWithProgress(It.IsAny<int>(),
                                                It.IsAny<everisapi.API.Models.EvaluacionInfoPaginationDto>()  
                                            )).Throws(new Exception());

            //Act
            var okResult = _controller.GetEvaluationsWithProgress(
                            1,
                            new everisapi.API.Models.EvaluacionInfoPaginationDto());

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetNumEvaluacionFromProject(int id)

        [Fact]
        public void GetNumEvaluacionFromProject_WhenCalled_ReturnsOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            int numEvaluaciones = 1;

            mockRepository.Setup(r => r.GetNumEval(It.IsAny<int>())).Returns(numEvaluaciones);

            //Act
            var okResult = _controller.GetNumEvaluacionFromProject(1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetNumEvaluacionFromProject_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            mockRepository.Setup(r => r.GetNumEval(It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetNumEvaluacionFromProject(1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetEvaluacionFromProject(int id)

        [Fact]
        public void GetEvaluacionFromProject_WhenCalled_ReturnsOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var  evaluations = new List<everisapi.API.Entities.EvaluacionEntity>{
                new everisapi.API.Entities.EvaluacionEntity{
                Id = 1,
                Fecha = new DateTime()
                }
            };

            mockRepository.Setup(r => r.GetEvaluacionesFromProject(It.IsAny<int>())).Returns(evaluations);

            //Act
            var okResult = _controller.GetEvaluacionFromProject(1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetEvaluacionFromProject_WhenCalledNull_ReturnsNotFoundResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            List<everisapi.API.Entities.EvaluacionEntity>  evaluations = null;

            mockRepository.Setup(r => r.GetEvaluacionesFromProject(It.IsAny<int>())).Returns(evaluations);

            //Act
            var okResult = _controller.GetEvaluacionFromProject(1);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void GetEvaluacionFromProject_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            mockRepository.Setup(r => r.GetEvaluacionesFromProject(It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetEvaluacionFromProject(1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetIncompleteEvaluationFromProject(int id)

        [Fact]
        public void GetIncompleteEvaluationFromProject_WhenCalled_ReturnsOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var  evaluations = new everisapi.API.Entities.EvaluacionEntity{
                Id = 1,
                Fecha = new DateTime()
            };

            mockRepository.Setup(r => r.EvaluationIncompletaFromProject(It.IsAny<int>())).Returns(evaluations);

            //Act
            var okResult = _controller.GetIncompleteEvaluationFromProject(1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetIncompleteEvaluationFromProject_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            mockRepository.Setup(r => r.EvaluationIncompletaFromProject(It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetIncompleteEvaluationFromProject(1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetIncompleteEvaluationFromProjectAndAssessment(int projectId,int assessmentId)

        [Fact]
        public void GetIncompleteEvaluationFromProjectAndAssessment_WhenCalled_ReturnsOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var  evaluations = new everisapi.API.Entities.EvaluacionEntity{
                Id = 1,
                Fecha = new DateTime()
            };

            mockRepository.Setup(r => r.EvaluationIncompletaFromProjectAndAssessment(
                It.IsAny<int>(), It.IsAny<int>())).Returns(evaluations);

            //Act
            var okResult = _controller.GetIncompleteEvaluationFromProjectAndAssessment(1,1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetIncompleteEvaluationFromProjectAndAssessment_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            mockRepository.Setup(r => r.EvaluationIncompletaFromProjectAndAssessment(
                It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetIncompleteEvaluationFromProjectAndAssessment(1,1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: CreateEvaluacion([FromBody] EvaluacionCreateUpdateDto EvaluacionRecogida)

        [Fact]
        public void CreateEvaluacion_WhenCalled_ReturnsOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluacion = new  everisapi.API.Models.EvaluacionCreateUpdateDto
                                {
                                    Id = 1, Fecha = new DateTime()
                                };

            mockRepository.Setup(r => r.IncluirEvaluacion(It.IsAny<everisapi.API.Entities.EvaluacionEntity>()));
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.CreateEvaluacion(evaluacion);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void CreateEvaluacion_WhenCalled_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluacion = new  everisapi.API.Models.EvaluacionCreateUpdateDto
                                {
                                    Id = 1, Fecha = new DateTime()
                                };

            mockRepository.Setup(r => r.IncluirEvaluacion(It.IsAny<everisapi.API.Entities.EvaluacionEntity>()));
            mockRepository.Setup(r => r.SaveChanges()).Returns(false);

            //Act
            var okResult = _controller.CreateEvaluacion(evaluacion);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        [Fact]
        public void CreateEvaluacion_WhenCalledWithNull_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            everisapi.API.Models.EvaluacionCreateUpdateDto evaluacion = null;

            mockRepository.Setup(r => r.IncluirEvaluacion(It.IsAny<everisapi.API.Entities.EvaluacionEntity>()));
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.CreateEvaluacion(evaluacion);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void CreateEvaluacion_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluacion = new  everisapi.API.Models.EvaluacionCreateUpdateDto
                                {
                                    Id = 1, Fecha = new DateTime()
                                };

            mockRepository.Setup(r => r.IncluirEvaluacion(It.IsAny<everisapi.API.Entities.EvaluacionEntity>()));
            mockRepository.Setup(r => r.SaveChanges()).Throws(new Exception());

            //Act
            var okResult = _controller.CreateEvaluacion(evaluacion);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: UpdateEvaluacion([FromBody] EvaluacionCreateUpdateDto EvaluacionRecogida)

        [Fact]
        public void UpdateEvaluacion_WhenCalled_ReturnsOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluacion = new  everisapi.API.Models.EvaluacionCreateUpdateDto
                                {
                                    Id = 1, Fecha = new DateTime()
                                };

            mockRepository.Setup(r => r.ModificarEvaluacion(It.IsAny<everisapi.API.Entities.EvaluacionEntity>()));
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.UpdateEvaluacion(evaluacion);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void UpdateEvaluacion_WhenCalled_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluacion = new  everisapi.API.Models.EvaluacionCreateUpdateDto
                                {
                                    Id = 1, Fecha = new DateTime()
                                };

            mockRepository.Setup(r => r.ModificarEvaluacion(It.IsAny<everisapi.API.Entities.EvaluacionEntity>()));
            mockRepository.Setup(r => r.SaveChanges()).Returns(false);

            //Act
            var okResult = _controller.UpdateEvaluacion(evaluacion);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        [Fact]
        public void UpdateEvaluacion_WhenCalledWithNull_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            everisapi.API.Models.EvaluacionCreateUpdateDto evaluacion = null;

            mockRepository.Setup(r => r.ModificarEvaluacion(It.IsAny<everisapi.API.Entities.EvaluacionEntity>()));
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.UpdateEvaluacion(evaluacion);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void UpdateEvaluacion_WhenCalledInvalidModel_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var evaluacion = new  everisapi.API.Models.EvaluacionCreateUpdateDto
                                {
                                    Id = 1, Fecha = new DateTime()
                                };

            mockRepository.Setup(r => r.ModificarEvaluacion(It.IsAny<everisapi.API.Entities.EvaluacionEntity>()));
            mockRepository.Setup(r => r.SaveChanges()).Returns(true);

            //Act
            var okResult = _controller.UpdateEvaluacion(evaluacion);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        [Fact]
        public void UpdateEvaluacion_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluacion = new  everisapi.API.Models.EvaluacionCreateUpdateDto
                                {
                                    Id = 1, Fecha = new DateTime()
                                };

            mockRepository.Setup(r => r.ModificarEvaluacion(It.IsAny<everisapi.API.Entities.EvaluacionEntity>()));
            mockRepository.Setup(r => r.SaveChanges()).Throws(new Exception());

            //Act
            var okResult = _controller.UpdateEvaluacion(evaluacion);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: EvaluationDelete([FromBody] int evaluationId)

        [Fact]
        public void EvaluationDelete_WhenCalled_ReturnsOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluacion = new  everisapi.API.Models.EvaluacionInfoDto
                                {
                                    Id = 1, Fecha = new DateTime()
                                };

            mockRepository.Setup(r => r.GetEvaluationInfoFromIdEvaluation(It.IsAny<int>())).Returns(evaluacion);
            mockRepository.Setup(r => r.EvaluationDelete(It.IsAny<int>())).Returns(true);

            //Act
            var okResult = _controller.EvaluationDelete(1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void EvaluationDelete_WhenCalled_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            var evaluacion = new  everisapi.API.Models.EvaluacionInfoDto
                                {
                                    Id = 1, Fecha = new DateTime()
                                };

            mockRepository.Setup(r => r.GetEvaluationInfoFromIdEvaluation(It.IsAny<int>())).Returns(evaluacion);
            mockRepository.Setup(r => r.EvaluationDelete(It.IsAny<int>())).Returns(false);

            //Act
            var okResult = _controller.EvaluationDelete(1);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void EvaluationDelete_WhenCalledWithNull_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            everisapi.API.Models.EvaluacionInfoDto evaluacion = null;

            mockRepository.Setup(r => r.GetEvaluationInfoFromIdEvaluation(It.IsAny<int>())).Returns(evaluacion);
            mockRepository.Setup(r => r.EvaluationDelete(It.IsAny<int>())).Returns(true);

            //Act
            var okResult = _controller.EvaluationDelete(1);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void EvaluationDelete_WhenCalledInvalidModel_ReturnsBadRequestResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var evaluacion = new  everisapi.API.Models.EvaluacionInfoDto
                                {
                                    Id = 1, Fecha = new DateTime()
                                };

            mockRepository.Setup(r => r.GetEvaluationInfoFromIdEvaluation(It.IsAny<int>())).Returns(evaluacion);
            mockRepository.Setup(r => r.EvaluationDelete(It.IsAny<int>())).Returns(false);

            //Act
            var okResult = _controller.EvaluationDelete(1);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        //Method: CalculateEvaluationProgress(int idEvaluacion,  int idAssessment)

        [Fact]
        public void CalculateEvaluationProgress_WhenCalled_ReturnsOkResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            mockRepository.Setup(r => r.CalculateEvaluationProgress(It.IsAny<int>(), It.IsAny<int>())).Returns(It.IsAny<float>());

            //Act
            var okResult = _controller.CalculateEvaluationProgress(1,1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void CalculateEvaluationProgress_WhenCalledThrowException_ReturnsStatusCodeResult()
        {
            //Arrange            
            _controller = new EvaluacionController(_logger, _evaluacionInfoRepository, _usersInfoRepository);

            mockRepository.Setup(r => r.CalculateEvaluationProgress(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.CalculateEvaluationProgress(1,1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }
        
    }
}