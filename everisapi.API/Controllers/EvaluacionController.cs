using everisapi.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using everisapi.API.Models;
using everisapi.API.Entities;
using Microsoft.AspNetCore.Authorization;

namespace everisapi.API.Controllers
{
    [Authorize]
    [Route("api/evaluaciones")]
    public class EvaluacionController : Controller
    {

        //Inyectamos un logger
        private ILogger<EvaluacionController> _logger;
        private IEvaluacionInfoRepository _evaluacionInfoRepository;

        //Utilizamos el constructor para inicializar el logger
        public EvaluacionController(ILogger<EvaluacionController> logger, IEvaluacionInfoRepository evaluacionInfoRepository)
        {
            _logger = logger;
            _evaluacionInfoRepository = evaluacionInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetEvaluaciones()
        {
            try
            {
                var EvaluacionesEntities = _evaluacionInfoRepository.GetEvaluaciones();

                var results = Mapper.Map<IEnumerable<EvaluacionesWithoutRespuestasDto>>(EvaluacionesEntities);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Se recogio un error al recibir todos los datos de las evaluaciones: " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }

        //Introduciendo la id de la evaluación devuelve una evaluación especifica
        [HttpGet("{id}")]
        public IActionResult GetEvaluacion(int id, bool IncluirRespuestas = false)
        {
            try
            {
                //Recoge si existe la evaluación si es asi la devuelve si no es así muestra un error
                var Evaluacion = _evaluacionInfoRepository.GetEvaluacion(id, IncluirRespuestas);

                if (Evaluacion == null)
                {
                    _logger.LogInformation("La evaluación con id " + id + " no pudo ser encontrado.");
                    return NotFound();
                }

                //Si tenemos como parametro recibir sus preguntas las incluira
                //sino lo devolvera sin respuestas
                if (IncluirRespuestas)
                {
                    var EvaluacionResult = Mapper.Map<EvaluacionDto>(Evaluacion);

                    return Ok(EvaluacionResult);

                }
                else
                {
                    var EvaluacionResult = Mapper.Map<EvaluacionesWithoutRespuestasDto>(Evaluacion);

                    return Ok(EvaluacionResult);
                }

            }
            catch (Exception ex)
            {
                _logger.LogCritical("Se recogio un error al recibir la evaluación con id " + id + ": " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }

        //Introduciendo la id de la evaluación devuelve una evaluación especifica
        [HttpGet("evaluacion/{idEvaluacion}/evaluationInfo")]
        public IActionResult GetEvaluationInfoFromIdEvaluation(int idEvaluacion)
        {
            try
            {
                //Recoge si existe la evaluación si es asi la devuelve si no es así muestra un error
                EvaluacionInfoDto progress = _evaluacionInfoRepository.GetEvaluationInfoFromIdEvaluation(idEvaluacion);

                if (progress == null)
                {
                    _logger.LogInformation("La evaluación información con id " + idEvaluacion + " no pudo ser encontrado.");
                    return NotFound();
                }

                return Ok(progress);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Se recogio un error al recibir la evaluación con toda su información con id " + idEvaluacion + ": " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }

        //Introduciendo la id de la evaluación devuelve una evaluación especifica
        [HttpGet("proyecto/{id}/info")]
        public IActionResult GetEvaluacionInfo(int id)
        {
            try
            {
                //Recoge si existe la evaluación si es asi la devuelve si no es así muestra un error
                var EvaluacionInfo = _evaluacionInfoRepository.GetEvaluationInfo(id);

                if (EvaluacionInfo == null)
                {
                    _logger.LogInformation("La evaluación información con id " + id + " no pudo ser encontrado.");
                    return NotFound();
                }

                return Ok(EvaluacionInfo);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Se recogio un error al recibir la evaluación con toda su información con id " + id + ": " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }

        //Introduciendo la id de la evaluación devuelve una evaluación especifica
        [HttpGet("proyecto/{id}/info/page/{pageNumber}")]
        public IActionResult GetEvaluacionInfoAndPage(int id, int pageNumber)
        {
            try
            {
                //Recoge si existe la evaluación si es asi la devuelve si no es así muestra un error
                var EvaluacionInfo = _evaluacionInfoRepository.GetEvaluationInfoAndPage(id, pageNumber);

                if (EvaluacionInfo == null)
                {
                    _logger.LogInformation("La evaluación información con id " + id + " no pudo ser encontrado.");
                    return NotFound();
                }

                return Ok(EvaluacionInfo);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Se recogio un error al recibir la evaluación con toda su información con id " + id + ": " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }

        //Este metodo lanza mediante un post una petición que devolvera una lista de evaluaciones
        //Es el que se usa en previousevaluation para la tabla
        [HttpPost("proyecto/{id}/info/page/{pageNumber}")]
        public IActionResult GetEvaluationInfoAndPageFiltered(int id, int pageNumber,
                [FromBody] EvaluacionInfoPaginationDto EvaluacionParaFiltrar)
        {
            try
            {

                //Comprueba que el body del json es correcto sino devolvera null
                //Si esto ocurre devolveremos un error
                if (EvaluacionParaFiltrar == null)
                {
                    return BadRequest();
                }

                //Si no cumple con el modelo de creación devuelve error
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var EvaluacionesFiltradas = new List<EvaluacionInfoDto>();
                var NumEvals = 0;
                if (id != 0)
                {
                    var Evals = _evaluacionInfoRepository.GetEvaluationInfoAndPageFiltered(id, pageNumber, EvaluacionParaFiltrar);
                    NumEvals = Evals.Count();
                    EvaluacionesFiltradas = Evals.ToList();//Skip(10 * pageNumber).Take(10).
                }
                else
                {
                    var Evals = _evaluacionInfoRepository.GetEvaluationInfoAndPageFilteredAdmin(pageNumber, EvaluacionParaFiltrar);
                    NumEvals = Evals.Count();
                    EvaluacionesFiltradas = Evals.ToList();//Skip(10 * pageNumber).Take(10).
                }

                //Hacemos un mapeo de la pregunta que recogimos
                var EvaluacionesResult = Mapper.Map<List<EvaluacionInfoDto>>(EvaluacionesFiltradas);

                return Ok(new { NumEvals, EvaluacionesResult });
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Se recogio un error al recibir la petición post de recoger una lista filtrada de evaluaciones con id de proyecto " + id + " y paginado con número " + pageNumber + ": " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }

        [HttpPost("proyecto/{id}/sectionsinfo/{codigoIdioma}")]
        public IActionResult GetEvaluationsWithSectionsInfo(int id, int codigoIdioma,
                [FromBody] EvaluacionInfoPaginationDto EvaluacionParaFiltrar)
        {
            try
            {

                //Comprueba que el body del json es correcto sino devolvera null
                //Si esto ocurre devolveremos un error
                if (EvaluacionParaFiltrar == null)
                {
                    return BadRequest();
                }

                //Si no cumple con el modelo de creación devuelve error
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var EvaluacionesFiltradas = new List<EvaluacionInfoWithSectionsDto>();

                var Evals = _evaluacionInfoRepository.GetEvaluationsWithSectionsInfo(id, EvaluacionParaFiltrar, codigoIdioma);
                EvaluacionesFiltradas = Evals.ToList();

                //Hacemos un mapeo de la pregunta que recogimos
                var EvaluacionesResult = Mapper.Map<List<EvaluacionInfoWithSectionsDto>>(EvaluacionesFiltradas);

                return Ok(new { EvaluacionesResult });
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Se recogio un error al recibir la petición post de recoger una lista filtrada de evaluaciones con id de proyecto " + id + ": " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }

        [HttpPost("proyecto/{id}/progress/")]
        public IActionResult GetEvaluationsWithProgress(int id,
                [FromBody] EvaluacionInfoPaginationDto EvaluacionParaFiltrar)
        {
            try
            {

                //Comprueba que el body del json es correcto sino devolvera null
                //Si esto ocurre devolveremos un error
                if (EvaluacionParaFiltrar == null)
                {
                    return BadRequest();
                }

                //Si no cumple con el modelo de creación devuelve error
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var EvaluacionesFiltradas = new List<EvaluacionInfoWithProgressDto>();

                var Evals = _evaluacionInfoRepository.GetEvaluationsWithProgress(id, EvaluacionParaFiltrar);
                EvaluacionesFiltradas = Evals.ToList();

                //Hacemos un mapeo de la pregunta que recogimos
                var EvaluacionesResult = Mapper.Map<List<EvaluacionInfoWithProgressDto>>(EvaluacionesFiltradas);

                return Ok(new { EvaluacionesResult });
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Se recogio un error al recibir la petición post de recoger una lista filtrada de evaluaciones con id de proyecto " + id + ": " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }

        //Este metodo devuelve el número de evaluaciones que contiene un proyecto o todos los proyectos
        [HttpGet("proyecto/{id}/num")]
        public IActionResult GetNumEvaluacionFromProject(int id)
        {
            try
            {

                var NumEvaluacion = _evaluacionInfoRepository.GetNumEval(id);

                return Ok(NumEvaluacion);

            }
            catch (Exception ex)
            {
                _logger.LogCritical("Se recogio un error al recibir la evaluación con id " + id + ": " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }

        [HttpGet("proyecto/{id}")]
        public IActionResult GetEvaluacionFromProject(int id)
        {
            try
            {
                //Recoge si existe la evaluación si es asi la devuelve si no es así muestra un error
                var Evaluacion = _evaluacionInfoRepository.GetEvaluacionesFromProject(id);

                if (Evaluacion == null)
                {
                    _logger.LogInformation("La evaluación con id " + id + " no pudo ser encontrado.");
                    return NotFound();
                }

                return Ok(Evaluacion);

            }
            catch (Exception ex)
            {
                _logger.LogCritical("Se recogio un error al recibir la evaluación con id " + id + ": " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }

        [HttpGet("proyecto/{id}/continue")]
        public IActionResult GetIncompleteEvaluationFromProject(int id)
        {
            try
            {
                //Recoge si existe la evaluación si es asi la devuelve si no es así muestra un error
                var Evaluacion = _evaluacionInfoRepository.EvaluationIncompletaFromProject(id);

                return Ok(Evaluacion);

            }
            catch (Exception ex)
            {
                _logger.LogCritical("Se recogio un error al recibir la evaluación con id " + id + ": " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }


        [HttpGet("proyecto/{projectId}/assessment/{assessmentId}/continue")]
        public IActionResult GetIncompleteEvaluationFromProjectAndAssessment(int projectId, int assessmentId)
        {
            try
            {
                //Recoge si existe la evaluación si es asi la devuelve si no es así muestra un error
                var Evaluacion = _evaluacionInfoRepository.EvaluationIncompletaFromProjectAndAssessment(projectId, assessmentId);

                return Ok(Evaluacion);

            }
            catch (Exception ex)
            {
                _logger.LogCritical("Se recogio un error al recibir la evaluación con id " + projectId + ": " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }

        //Este metodo nos permite introducir una evaluación
        [HttpPost()]
        public IActionResult CreateEvaluacion([FromBody] EvaluacionCreateUpdateDto EvaluacionRecogida)
        {
            try
            {
                //Comprueba que el body del json es correcto sino devolvera null
                //Si esto ocurre devolveremos un error
                if (EvaluacionRecogida == null)
                {
                    return BadRequest();
                }


                //Hacemos un mapeo de la evaluación que recogimos
                var IngresarEvaluacion = Mapper.Map<EvaluacionEntity>(EvaluacionRecogida);

                IngresarEvaluacion.Fecha = DateTime.Now;

                //La incluimos en la evaluación
                _evaluacionInfoRepository.IncluirEvaluacion(IngresarEvaluacion);

                //Guardamos los cambios a la entidad y esta debera devolver si es correcta o no
                if (!_evaluacionInfoRepository.SaveChanges())
                {
                    _logger.LogCritical("Ocurrio un error al guardar los cambios cuando intentamos incluir una evaluacion");
                    return StatusCode(500, "Ocurrio un problema en la petición.");
                }

                var EvaluationIngresada = Mapper.Map<EvaluacionesWithoutRespuestasDto>(IngresarEvaluacion);

                return Ok(EvaluationIngresada);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Se recogio un error al recibir la petición de creación de evaluacion: " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }

        //Este metodo nos permite cambiar una evaluación
        [HttpPut()]
        public IActionResult UpdateEvaluacion([FromBody] EvaluacionCreateUpdateDto EvaluacionRecogida)
        {
            try
            {
                //Comprueba que el body del json es correcto sino devolvera null
                //Si esto ocurre devolveremos un error
                if (EvaluacionRecogida == null)
                {
                    return BadRequest();
                }

                //Si no cumple con el modelo de update devuelve error
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //Hacemos un mapeo de la evaluación que recogimos
                var ModificarEvaluacion = Mapper.Map<EvaluacionEntity>(EvaluacionRecogida);

                //La incluimos en la evaluación
                _evaluacionInfoRepository.ModificarEvaluacion(ModificarEvaluacion);

                //Guardamos los cambios a la entidad y esta debera devolver si es correcta o no
                if (!_evaluacionInfoRepository.SaveChanges())
                {
                    _logger.LogCritical("Ocurrio un error al guardar los cambios cuando intentamos modificar una evaluacion con id: " + ModificarEvaluacion.Id);
                    return StatusCode(500, "Ocurrio un problema en la petición.");
                }


                return Ok("La evaluación fue modificada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Se recogio un error al recibir la petición de modificacion de evaluacion: " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }

        /*DELETE EVALUACIONES*/
        //Metodo encargado de eliminar una evaluacion con Id determinado
        [HttpPost("evaluacion/delete/")]
        public IActionResult EvaluationDelete([FromBody] int evaluationId)
        {
            //Se obtiene la evaluacion a borrar desde BBDD en base a su Id
            if (_evaluacionInfoRepository.GetEvaluationInfoFromIdEvaluation(evaluationId) == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Se elimina la evaluacion
            if (_evaluacionInfoRepository.EvaluationDelete(evaluationId))
            {
                return Ok("La evaluación fue eliminada correctamente.");
            }
            else
            {
                return BadRequest();
            }
        }


        //Introduciendo la id de la evaluación devuelve una evaluación especifica
        [HttpGet("proyecto/{idEvaluacion}/assessment/{idAssessment}/totalprogress")]
        public IActionResult CalculateEvaluationProgress(int idEvaluacion, int idAssessment)
        {
            try
            {
                //Recoge si existe la evaluación si es asi la devuelve si no es así muestra un error
                float? progress = _evaluacionInfoRepository.CalculateEvaluationProgress(idEvaluacion, idAssessment);

                if (progress == null)
                {
                    _logger.LogInformation("La evaluación información con id " + idEvaluacion + " no pudo ser encontrado.");
                    return NotFound();
                }

                return Ok(progress);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Se recogio un error al recibir la evaluación con toda su información con id " + idEvaluacion + ": " + ex);
                return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
            }
        }
    }
}
