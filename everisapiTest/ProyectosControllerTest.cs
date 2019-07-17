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
    public class ProyectosControllerTest
    {
        ProyectosController _controller;
        private readonly ILogger<ProyectosController> _logger;
        private readonly IUsersInfoRepository _userInfoRepository;

        Mock<ILogger<ProyectosController>> mockLogger;
        Mock<IUsersInfoRepository> mockRepository;

        public ProyectosControllerTest()
        {
            mockLogger = new Mock<ILogger<ProyectosController>>();
            _logger = mockLogger.Object;

            mockRepository = new Mock<IUsersInfoRepository>();
            _userInfoRepository = mockRepository.Object;

            var autoMapperInstance = AutoMapperConfig.Instance;
        }

        //Method: GetFullProyectos()
        [Fact]
        public void GetFullProyectos_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyectosDto = new List<everisapi.API.Models.ProyectoDto>()
            {
                new everisapi.API.Models.ProyectoDto {
                    Nombre = "Proyecto prueba 1"
                },
                new everisapi.API.Models.ProyectoDto {
                    Nombre = "Proyecto prueba 2"
                }
            };

            mockRepository.Setup(r => r.GetFullProyectos()).Returns(proyectosDto);

            //Act
            var okResult = _controller.GetFullProyectos();

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetFullProyectos_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);
            mockRepository.Setup(r => r.GetFullProyectos()).Throws(new Exception());

            //Act
            var okResult = _controller.GetFullProyectos();

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetAllNotTestProjects()
        [Fact]
        public void GetAllNotTestProjects_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyectosEntities = new List<everisapi.API.Entities.ProyectoEntity>()
            {
                new everisapi.API.Entities.ProyectoEntity {
                    Nombre = "Proyecto prueba 1"
                },
                new everisapi.API.Entities.ProyectoEntity {
                    Nombre = "Proyecto prueba 2"
                }
            };

            mockRepository.Setup(r => r.GetAllNotTestProjects()).Returns(proyectosEntities);

            //Act
            var okResult = _controller.GetAllNotTestProjects();

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetAllNotTestProjects_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);
            mockRepository.Setup(r => r.GetAllNotTestProjects()).Throws(new Exception());

            //Act
            var okResult = _controller.GetAllNotTestProjects();

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetAllAssessments()
        [Fact]
        public void GetAllAssessments_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var assessmentEntities = new List<everisapi.API.Entities.AssessmentEntity>()
            {
                new everisapi.API.Entities.AssessmentEntity {
                    AssessmentName = "Assessment 1"
                },
                new everisapi.API.Entities.AssessmentEntity {
                    AssessmentName = "Assessment 2"                    
                }
            };

            mockRepository.Setup(r => r.GetAllAssessments()).Returns(assessmentEntities);

            //Act
            var okResult = _controller.GetAllAssessments();

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetAllAssessments_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);
            mockRepository.Setup(r => r.GetAllAssessments()).Throws(new Exception());

            //Act
            var okResult = _controller.GetAllAssessments();

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetProyectosUsuario(string nombreUsuario)
        [Fact]
        public void GetProyectosUsuario_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyectosDto = new List<everisapi.API.Models.ProyectoDto>()
            {
                new everisapi.API.Models.ProyectoDto {
                    Nombre = "Proyecto prueba 1",
                    UserNombre = "fmoreno"
                },
                new everisapi.API.Models.ProyectoDto {
                    Nombre = "Proyecto prueba 2",
                    UserNombre = "fmoreno"
                }
            };

            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(true);
            mockRepository.Setup(r => r.GetProyectosDeUsuario(It.IsAny<string>())).Returns(proyectosDto);

            //Act
            var okResult = _controller.GetProyectosUsuario("fmoreno");

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetProyectosUsuario_WhenCalledNoExitUser_ReturnNotFound()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyectosDto = new List<everisapi.API.Models.ProyectoDto>()
            {
                new everisapi.API.Models.ProyectoDto {
                    Nombre = "Proyecto prueba 1",
                    UserNombre = "fmoreno"
                },
                new everisapi.API.Models.ProyectoDto {
                    Nombre = "Proyecto prueba 2",
                    UserNombre = "fmoreno"
                }
            };

            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(false);
            mockRepository.Setup(r => r.GetProyectosDeUsuario(It.IsAny<string>())).Returns(proyectosDto);

            //Act
            var okResult = _controller.GetProyectosUsuario("fmoreno");

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void GetProyectosUsuario_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);
            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(true);
            mockRepository.Setup(r => r.GetProyectosDeUsuario(It.IsAny<string>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetProyectosUsuario("fmoreno");

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetProyectoUsuario(string nombreUsuario, int id)
        [Fact]
        public void GetProyectoUsuario_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyectoDto = new everisapi.API.Models.ProyectoDto {
                    Id = 1,
                    Nombre = "Proyecto prueba 1",

                    UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(true);
            mockRepository.Setup(r => r.GetOneProyecto(It.IsAny<string>(), It.IsAny<int>())).Returns(proyectoDto);

            //Act
            var okResult = _controller.GetProyectoUsuario("fmoreno",1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetProyectoUsuario_WhenCalledNoExitUser_ReturnNotFound()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

             var proyectoDto = new everisapi.API.Models.ProyectoDto {
                    Nombre = "Proyecto prueba 1",
                    UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(false);
            mockRepository.Setup(r => r.GetOneProyecto(It.IsAny<string>(), It.IsAny<int>())).Returns(proyectoDto);

            //Act
            var okResult = _controller.GetProyectoUsuario("fmoreno",1);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void GetProyectoUsuario_WhenCalledNoExitProject_ReturnNotFound()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            everisapi.API.Models.ProyectoDto proyectoDto = null;

            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(true);
            mockRepository.Setup(r => r.GetOneProyecto(It.IsAny<string>(), It.IsAny<int>())).Returns(proyectoDto);

            //Act
            var okResult = _controller.GetProyectoUsuario("fmoreno",1);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void GetProyectoUsuario_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);
            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(true);
            mockRepository.Setup(r => r.GetOneProyecto(It.IsAny<string>(), It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetProyectoUsuario("fmoreno",1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: GetProyecto(int id)
        [Fact]
        public void GetProyecto_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyectoDto = new everisapi.API.Entities.ProyectoEntity {
                    Id = 1,
                    Nombre = "Proyecto prueba 1",
                    UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.GetFullProject(It.IsAny<int>())).Returns(proyectoDto);

            //Act
            var okResult = _controller.GetProyecto(1);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetProyecto_WhenCalledNoExitProject_ReturnNotFound()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            everisapi.API.Entities.ProyectoEntity proyectoEntity = null;

            mockRepository.Setup(r => r.GetFullProject(It.IsAny<int>())).Returns(proyectoEntity);

            //Act
            var okResult = _controller.GetProyecto(1);

            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public void GetProyecto_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);
            mockRepository.Setup(r => r.GetFullProject(It.IsAny<int>())).Throws(new Exception());

            //Act
            var okResult = _controller.GetProyecto(1);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        //Method: AddProyecto([FromBody] ProyectoCreateUpdateDto ProyectoAdd)
        [Fact]
        public void AddProyecto_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyecto = new everisapi.API.Models.ProyectoCreateUpdateDto {
                Id = 1,
                Nombre = "Proyecto prueba 1",
                UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.ProyectoExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(true);
            mockRepository.Setup(r => r.AddProj(It.IsAny<everisapi.API.Entities.ProyectoEntity>())).Returns(true);

            //Act
            var okResult = _controller.AddProyecto(proyecto);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void AddProyecto_WhenCalled_ReturnBadRequest()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyecto = new everisapi.API.Models.ProyectoCreateUpdateDto {
                Id = 1,
                Nombre = "Proyecto prueba 1",
                UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.ProyectoExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(true);
            mockRepository.Setup(r => r.AddProj(It.IsAny<everisapi.API.Entities.ProyectoEntity>())).Returns(false);

            //Act
            var okResult = _controller.AddProyecto(proyecto);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddProj_WhenThrowException_ReturnStatusCode()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyecto = new everisapi.API.Models.ProyectoCreateUpdateDto {
                Id = 1,
                Nombre = "Proyecto prueba 1",
                UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.ProyectoExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(true);
            mockRepository.Setup(r => r.AddProj(It.IsAny<everisapi.API.Entities.ProyectoEntity>())).Throws(new Exception());

            //Act
            var okResult = _controller.AddProyecto(proyecto);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }

        [Fact]
        public void AddProj_WhenExitProject_ReturnsBadRequest()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyecto = new everisapi.API.Models.ProyectoCreateUpdateDto {
                Id = 1,
                Nombre = "Proyecto prueba 1",
                UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.ProyectoExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(true);
            mockRepository.Setup(r => r.AddProj(It.IsAny<everisapi.API.Entities.ProyectoEntity>())).Returns(true);

            //Act
            var okResult =  _controller.AddProyecto(proyecto);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddProj_WhenNotExitUser_ReturnsBadRequest()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyecto = new everisapi.API.Models.ProyectoCreateUpdateDto {
                Id = 1,
                Nombre = "Proyecto prueba 1",
                UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.ProyectoExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(false);
            mockRepository.Setup(r => r.AddProj(It.IsAny<everisapi.API.Entities.ProyectoEntity>())).Returns(true);

            //Act
            var okResult = _controller.AddProyecto(proyecto);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void AddProject_GivenInvalidModel_ReturnsBadRequest()
        {
            //Arrange
            _controller = new ProyectosController(_logger, _userInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var proyecto = new everisapi.API.Models.ProyectoCreateUpdateDto {
                Id = 1,
                Nombre = "Proyecto prueba 1",
                UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.ProyectoExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(true);
            mockRepository.Setup(r => r.AddProj(It.IsAny<everisapi.API.Entities.ProyectoEntity>())).Returns(true);

            //Act
            var okResult = _controller.AddProyecto(proyecto);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        //Method: UpdateProyecto([FromBody] ProyectoCreateUpdateDto ProyectoUpdate)
        [Fact]
        public void UpdateProyecto_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyecto = new everisapi.API.Models.ProyectoCreateUpdateDto {
                Id = 1,
                Nombre = "Proyecto prueba 1",
                UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.ProyectoExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(true);
            mockRepository.Setup(r => r.AlterProj(It.IsAny<everisapi.API.Entities.ProyectoEntity>())).Returns(true);

            //Act
            var okResult = _controller.UpdateProyecto(proyecto);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void UpdateProyecto_WhenCalled_ReturnBadRequest()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyecto = new everisapi.API.Models.ProyectoCreateUpdateDto {
                Id = 1,
                Nombre = "Proyecto prueba 1",
                UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.ProyectoExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(true);
            mockRepository.Setup(r => r.AlterProj(It.IsAny<everisapi.API.Entities.ProyectoEntity>())).Returns(false);

            //Act
            var okResult = _controller.UpdateProyecto(proyecto);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void UpdateProyecto_WhenNotExitProject_ReturnsBadRequest()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyecto = new everisapi.API.Models.ProyectoCreateUpdateDto {
                Id = 1,
                Nombre = "Proyecto prueba 1",
                UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.ProyectoExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(true);
            mockRepository.Setup(r => r.AlterProj(It.IsAny<everisapi.API.Entities.ProyectoEntity>())).Returns(true);

            //Act
            var okResult =  _controller.UpdateProyecto(proyecto);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void UpdateProyecto_WhenNotExitUser_ReturnsBadRequest()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyecto = new everisapi.API.Models.ProyectoCreateUpdateDto {
                Id = 1,
                Nombre = "Proyecto prueba 1",
                UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.ProyectoExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(false);
            mockRepository.Setup(r => r.AlterProj(It.IsAny<everisapi.API.Entities.ProyectoEntity>())).Returns(true);

            //Act
            var okResult = _controller.UpdateProyecto(proyecto);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void UpdateProyecto_GivenInvalidModel_ReturnsBadRequest()
        {
            //Arrange
            _controller = new ProyectosController(_logger, _userInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var proyecto = new everisapi.API.Models.ProyectoCreateUpdateDto {
                Id = 1,
                Nombre = "Proyecto prueba 1",
                UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.ProyectoExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.UserExiste(It.IsAny<string>())).Returns(true);
            mockRepository.Setup(r => r.AlterProj(It.IsAny<everisapi.API.Entities.ProyectoEntity>())).Returns(true);

            //Act
            var okResult = _controller.UpdateProyecto(proyecto);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        //Method: DeleteProyecto([FromBody] ProyectoDto ProyectoDelete)
        [Fact]
        public void DeleteProyecto_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyecto = new everisapi.API.Models.ProyectoDto {
                Id = 1,
                Nombre = "Proyecto prueba 1",
                UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.ProyectoExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.DeleteProj(It.IsAny<everisapi.API.Entities.ProyectoEntity>())).Returns(true);

            //Act
            var okResult = _controller.DeleteProyecto(proyecto);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void DeleteProyecto_WhenCalled_ReturnBadRequest()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyecto = new everisapi.API.Models.ProyectoDto {
                Id = 1,
                Nombre = "Proyecto prueba 1",
                UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.ProyectoExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.DeleteProj(It.IsAny<everisapi.API.Entities.ProyectoEntity>())).Returns(false);

            //Act
            var okResult = _controller.DeleteProyecto(proyecto);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void DeleteProyecto_WhenNotExitProject_ReturnsBadRequest()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyecto = new everisapi.API.Models.ProyectoDto {
                Id = 1,
                Nombre = "Proyecto prueba 1",
                UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.ProyectoExiste(It.IsAny<int>())).Returns(false);
            mockRepository.Setup(r => r.DeleteProj(It.IsAny<everisapi.API.Entities.ProyectoEntity>())).Returns(true);

            //Act
            var okResult =  _controller.DeleteProyecto(proyecto);

            //Assert
            Assert.IsType<BadRequestResult>(okResult);
        }

        [Fact]
        public void DeleteProyecto_GivenInvalidModel_ReturnsBadRequest()
        {
            //Arrange
            _controller = new ProyectosController(_logger, _userInfoRepository);
            _controller.ModelState.AddModelError("error", "some error");

            var proyecto = new everisapi.API.Models.ProyectoDto {
                Id = 1,
                Nombre = "Proyecto prueba 1",
                UserNombre = "fmoreno"
            };

            mockRepository.Setup(r => r.ProyectoExiste(It.IsAny<int>())).Returns(true);
            mockRepository.Setup(r => r.DeleteProj(It.IsAny<everisapi.API.Entities.ProyectoEntity>())).Returns(true);

            //Act
            var okResult = _controller.DeleteProyecto(proyecto);

            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }          

        //Method: AddTeam([FromBody] Equipos equipo)
        [Fact]
        public void AddTeam_WhenCalled_ReturnOkResult()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyecto = new everisapi.API.Models.Equipos {
                Id = 1,
                Nombre = "Proyecto prueba 1",
            };

            mockRepository.Setup(r => r.AddTeam(proyecto)).Returns(true);

            //Act
            var okResult = _controller.AddTeam(proyecto);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void AddTeam_WhenCalled_ReturnStatusCode()
        {
            //Arrange            
            _controller = new ProyectosController(_logger, _userInfoRepository);

            var proyecto = new everisapi.API.Models.Equipos {
                Id = 1,
                Nombre = "Proyecto prueba 1",
            };

            mockRepository.Setup(r => r.AddTeam(proyecto)).Throws(new Exception());

            //Act
            var okResult = _controller.AddTeam(proyecto);

            //Assert
            Assert.IsType<ObjectResult>(okResult);
        }
    }
}