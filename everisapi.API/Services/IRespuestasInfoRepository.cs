using everisapi.API.Entities;
using everisapi.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Services
{
  public interface IRespuestasInfoRepository
  {
    //Devuelve todas las respuestas
    IEnumerable<RespuestaEntity> GetRespuestas();

    //Devuelve una respuesta
    RespuestaEntity GetRespuesta(int RespuestaId);

    //Devuelve todas las respuestas filtrada por proyecto y asignacion
    IEnumerable<RespuestaEntity> GetRespuestasFromPregEval(int IdProyecto, int IdAsignacion);

    //Metodo que devuelve la lista de respuestas de las preguntas que pertenecen a una pregunta hablitante de una evaluacion
    IEnumerable<RespuestaEntity> GetAnswersByEnablingQuestion(int evaluationId, int enablingQuestionId);

    //Devuelve todas las respuestas filtrada por proyecto y pregunta
    IEnumerable<RespuestaEntity> GetRespuestasFromAsigEval(int IdProyecto, int IdPregunta);

    //Update de una respuesta
    bool UpdateRespuesta(RespuestaDto respuesta);

    //Guardar cambio de las entidades
    bool SaveChanges();

    //Aqui introducimos una nueva respuesta
    bool AddRespuesta(RespuestaEntity respuesta);

    //Elimina una respuesta
    bool DeleteRespuesta(RespuestaEntity respuesta);

    //Muestra si existe la respuesta
    bool ExiteRespuesta(int idRespuesta);

    //Introduciendo la id de evaluacion sacaremos una lista con todas las respuestas que tengan notas
    IEnumerable<RespuestaConNotasDto> GetRespuestasConNotas(int idEvaluacion,int? assessmentId);

    IEnumerable<SectionConAsignacionesDto> GetPreguntasNivelOrganizadas(int idEvaluacion, int assessmentId, int codigoIdioma);

    //Este metodo se usa cuando se quiere poner todas las respuestas de una asignacion a No Contestado
    //Excepto la primera, que se pone a No
    bool UpdateRespuestasAsignacion(int idEvaluacion, int enablingQuestionId);
  }
}
