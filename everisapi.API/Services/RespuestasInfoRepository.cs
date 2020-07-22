using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using everisapi.API.Entities;
using everisapi.API.Models;
using Microsoft.EntityFrameworkCore;

namespace everisapi.API.Services
{
    public class RespuestasInfoRepository : IRespuestasInfoRepository
    {

        private AsignacionInfoContext _context;

        private IPuntuacionSectionRepository _puntuacionSection;

        //Le damos un contexto en el constructor
        public RespuestasInfoRepository(AsignacionInfoContext context, IPuntuacionSectionRepository puntuacionSection)
        {
            _context = context;
            _puntuacionSection = puntuacionSection;
        }

        //Recoge una unica respuesta filtrada por id
        public RespuestaEntity GetRespuesta(int RespuestaId)
        {
            return _context.Respuestas.Where(r => r.Id == RespuestaId).FirstOrDefault();
        }

        //Recoge todas las respuestas existentes
        public IEnumerable<RespuestaEntity> GetRespuestas()
        {
            return _context.Respuestas.ToList();
        }

        //Introduciendo el id de evaluacion y el id de pregunta te da la lista de respuestas
        public IEnumerable<RespuestaEntity> GetRespuestasFromPregEval(int idEvaluacion, int IdPregunta)
        {
            return _context.Respuestas.Where(r => r.PreguntaId == IdPregunta && r.EvaluacionId == idEvaluacion).ToList();
        }

        //Metodo que devuelve la lista de respuestas de las preguntas que pertenecen a una pregunta hablitante de una evaluacion
        public IEnumerable<RespuestaEntity> GetAnswersByEnablingQuestion(int evaluationId, int enablingQuestionId)
        {
            return _context.Respuestas.Where(r => r.PreguntaEntity.PreguntaHabilitanteId == enablingQuestionId
            && r.EvaluacionId == evaluationId).ToList();
        }

        //Introduciendo la id de asignacion y la id de evaluacion sacaremos una lista con todas las respuestas
        public IEnumerable<RespuestaEntity> GetRespuestasFromAsigEval(int idEvaluacion, int IdAsignacion)
        {
            return _context.Respuestas.Where(r => r.PreguntaEntity.AsignacionId == IdAsignacion && r.EvaluacionId == idEvaluacion).ToList();
        }

        //Guarda todos los cambios en la base de datos
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        //Realiza un update de la respuesta por el id de la respuesta y el estado que se desea cambiar
        public bool UpdateRespuesta(RespuestaConUserDto Respuesta)
        {
            RespuestaEntity respuestaAnterior = _context.Respuestas.Where(r => r.Id == Respuesta.Id).FirstOrDefault();
            EvaluacionEntity currentEvaluation = _context.Evaluaciones.First(x => x.Id == respuestaAnterior.EvaluacionId);


            if (respuestaAnterior != null)
            {
                respuestaAnterior.Estado = Respuesta.Estado;
                respuestaAnterior.Notas = Respuesta.Notas;
                respuestaAnterior.NotasAdmin = Respuesta.NotasAdmin;

                currentEvaluation.LastQuestionUpdated = respuestaAnterior.PreguntaId;
                currentEvaluation.Fecha = DateTime.Now;
                currentEvaluation.UserNombre = Respuesta.UserName;

                return SaveChanges();
            }
            else
            {
                return false;
            }
        }

        //Metodo que actualiza la respuesta de una pregunta habilitante a No y la de sus preguntas habilitadas a SinResponder
        public bool UpdateRespuestasAsignacion(int evaluationId, int enablingQuestionId)
        {
            List<RespuestaEntity> disabledsAnswers = GetAnswersByEnablingQuestion(evaluationId, enablingQuestionId).ToList();
            RespuestaEntity enablingAnswer = _context.Respuestas.First(r => r.PreguntaEntity.Id == enablingQuestionId && r.EvaluacionId == evaluationId);
            EvaluacionEntity evaluation = _context.Evaluaciones.First(e => e.Id == evaluationId);
            if (disabledsAnswers != null)
            {
                disabledsAnswers.ForEach(r => r.Estado = 0);
                enablingAnswer.Estado = 2;
                evaluation.LastQuestionUpdated = enablingQuestionId;
                evaluation.Fecha = DateTime.Now; //new DateTime();

                return SaveChanges();
            }
            else
            {
                return false;
            }
        }

        //Introduciendo la id de evaluacion sacaremos una lista con todas las respuestas 
        public IEnumerable<RespuestaConNotasDto> GetRespuestasConNotas(int idEvaluacion, int? assessmentId)
        {
            List<RespuestaEntity> respuestas;
            if (assessmentId != null || assessmentId != 0)
            {
                respuestas = _context.Respuestas.
                    Include(r => r.PreguntaEntity).
                      ThenInclude(p => p.AsignacionEntity).
                      ThenInclude(p => p.SectionEntity).
                    Where(r => r.EvaluacionId == idEvaluacion && r.PreguntaEntity.AsignacionEntity.SectionEntity.AssessmentId == assessmentId).ToList();
            }
            else
            {
                respuestas = _context.Respuestas.
                       Include(r => r.PreguntaEntity).
                         ThenInclude(p => p.AsignacionEntity).
                         ThenInclude(p => p.SectionEntity).
                         ThenInclude(p => p.Assessment).
                       Where(r => r.EvaluacionId == idEvaluacion).ToList();
            }



            List<RespuestaConNotasDto> lista = new List<RespuestaConNotasDto>();
            foreach (RespuestaEntity resp in respuestas)
            {
                lista.Add(new RespuestaConNotasDto
                {
                    Id = resp.Id,
                    Estado = resp.Estado,
                    Correcta = resp.PreguntaEntity.Correcta,
                    Notas = resp.Notas,
                    NotasAdmin = resp.NotasAdmin,
                    Asignacion = "",
                    // Section = resp.PreguntaEntity.AsignacionEntity.SectionEntity.Nombre,
                    Section = "",
                    //Pregunta = resp.PreguntaEntity.Pregunta,
                    Pregunta = "",
                    Peso = resp.PreguntaEntity.Peso,
                    Nivel = resp.PreguntaEntity.Nivel
                });
            }

            return lista;
        }

        public IEnumerable<SectionConAsignacionesDto> GetPreguntasNivelOrganizadas(int idEvaluacion, int assessmentId, int codigoIdioma, bool final = false)
        {

            List<SectionConAsignacionesDto> sectionsConAsignaciones = new List<SectionConAsignacionesDto>();


            var sections = from s in _context.Sections.Where(x => x.AssessmentId == assessmentId)
                           join n in _context.NotasSections.Where(x => x.EvaluacionId == idEvaluacion) on s.Id equals n.SectionId into sn
                           from y1 in sn.DefaultIfEmpty()
                           orderby s.Id
                           select new { section = s, notas = y1.Notas };


            foreach (var s in sections)
            {
                SectionConAsignacionesDto sectionConAsignacion = new SectionConAsignacionesDto();
                sectionConAsignacion.EvaluacionId = idEvaluacion;
                sectionConAsignacion.SectionId = s.section.Id;
                // sectionConAsignacion.Nombre = s.section.Nombre;
                sectionConAsignacion.Nombre = _context.TraduccionesSections.Where(sec => sec.IdiomaId == codigoIdioma && s.section.Id == sec.SectionsId).Select(sec => sec.Traduccion).FirstOrDefault();
                sectionConAsignacion.Notas = s.notas;
                sectionConAsignacion.Peso = s.section.Peso;
                sectionConAsignacion.PesoNivel1 = s.section.PesoNivel1;
                sectionConAsignacion.PesoNivel2 = s.section.PesoNivel2;
                sectionConAsignacion.PesoNivel3 = s.section.PesoNivel3;
                sectionConAsignacion.NumNotas = (s.notas == null || s.notas.Trim() == "") ? 0 : 1;

                List<AsignacionConPreguntaNivelDto> asignacionesConPreguntaNivel = new List<AsignacionConPreguntaNivelDto>();
                //List<AsignacionEntity> asignaciones;
                // var asignaciones = _context.Asignaciones
                // .Join(_context.NotasAsignaciones, // the source table of the inner join
                // a => a.Id,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
                // n => n.AsignacionId,   // Select the foreign key (the second part of the "on" clause)
                // (a, n) => new { asignacion = a, notas = n })
                // .Where(a => a.asignacion.SectionId == s.section.Id).ToList();

                var asignaciones = from a in _context.Asignaciones.Where(x => x.SectionId == s.section.Id)
                                   join n in _context.NotasAsignaciones.Where(x => x.EvaluacionId == idEvaluacion) on a.Id equals n.AsignacionId into an
                                   from y1 in an.DefaultIfEmpty()
                                   orderby a.Id
                                   select new { asignacion = a, notas = y1.Notas };

                List<TraduccionesPreguntasEntity> traducciones = _context.TraduccionesPreguntas.Where(y => y.IdiomaId == codigoIdioma).ToList();
                foreach (var a in asignaciones)
                {
                    AsignacionConPreguntaNivelDto asignacionConPreguntaNivel = new AsignacionConPreguntaNivelDto();
                    asignacionConPreguntaNivel.Id = a.asignacion.Id;
                    // asignacionConPreguntaNivel.Nombre = a.asignacion.Nombre;
                    asignacionConPreguntaNivel.Nombre = _context.TraduccionesAsignaciones.Where(asig => asig.IdiomaId == codigoIdioma && asig.AsignacionesId == a.asignacion.Id).Select(asig => asig.Traduccion).FirstOrDefault();
                    asignacionConPreguntaNivel.Peso = a.asignacion.Peso;
                    asignacionConPreguntaNivel.Notas = a.notas;
                    asignacionConPreguntaNivel.NumNotas = (a.notas == null || a.notas.Trim() == "") ? 0 : 1;

                    List<PreguntaRespuestaNivelDto> preguntasRespuestaNivel = new List<PreguntaRespuestaNivelDto>();
                    List<RespuestaEntity> preguntas = new List<RespuestaEntity>();
                    preguntas = _context.Respuestas.
                    Include(r => r.PreguntaEntity).Where(p => p.EvaluacionId == idEvaluacion && p.PreguntaEntity.AsignacionId == a.asignacion.Id).ToList();
                    preguntas = preguntas.OrderBy(x => x.PreguntaEntity.Id).ToList();

                    foreach (RespuestaEntity p in preguntas)
                    {
                        PreguntaRespuestaNivelDto preguntaRespuestaNivel = new PreguntaRespuestaNivelDto();
                        preguntaRespuestaNivel.Id = p.PreguntaEntity.Id;
                        preguntaRespuestaNivel.Nivel = p.PreguntaEntity.Nivel;
                        preguntaRespuestaNivel.Notas = p.Notas;
                        preguntaRespuestaNivel.NotasAdmin = p.NotasAdmin;
                        preguntaRespuestaNivel.Peso = p.PreguntaEntity.Peso;
                        //preguntaRespuestaNivel.Pregunta = p.PreguntaEntity.Pregunta;
                        preguntaRespuestaNivel.Pregunta = traducciones.Where(tra => tra.PreguntaId == p.PreguntaId && tra.IdiomaId == codigoIdioma).Select(tra => tra.Traduccion).FirstOrDefault();
                        preguntaRespuestaNivel.Estado = p.Estado;
                        preguntaRespuestaNivel.Correcta = p.PreguntaEntity.Correcta;

                        if (p.Notas != null && p.Notas.Trim() != "")
                        {
                            asignacionConPreguntaNivel.NumNotas++;
                        }

                        preguntasRespuestaNivel.Add(preguntaRespuestaNivel);
                    }
                    sectionConAsignacion.NumNotas += asignacionConPreguntaNivel.NumNotas;
                    asignacionConPreguntaNivel.Preguntas = preguntasRespuestaNivel;

                    asignacionesConPreguntaNivel.Add(asignacionConPreguntaNivel);
                }
                sectionConAsignacion.Asignaciones = asignacionesConPreguntaNivel;

                sectionsConAsignaciones.Add(sectionConAsignacion);
            }
            
            // var path =@"C:\Users\jfrancom\ScrumMeter\everisapi.API\seccioneseval.txt"; 
            // using (StreamWriter writerTxt = File.CreateText(path))
            // {

            foreach (SectionConAsignacionesDto seccion in sectionsConAsignaciones)
            {
                
                float sumaNoBinary = 0;
                int MaxRange = _context.Assessment.Where(w => w.AssessmentId == assessmentId).Select(x => x.AssessmentRange).FirstOrDefault();
                

                //calculamos los niveles individuales para cada asignacion
                foreach (AsignacionConPreguntaNivelDto asignacion in seccion.Asignaciones)
                {

                    var maxLevel = asignacion.Preguntas.Max(x => x.Nivel);
                    bool nivelCompleto = true;
                    for (int i = 1; i <= maxLevel && nivelCompleto; i++)
                    {
                        nivelCompleto = false;
                        var preguntas = asignacion.Preguntas.Where(p => p.Nivel == i);

                         var preguntasCorrectas = asignacion.Preguntas // preguntas noBinary
                            .Where(p => p.Nivel == i && ((p.Estado == MaxRange && p.Correcta == "Si") || ( p.Estado == 1 && p.Correcta == "No")));

                        // if(!noBinary){ // probar una vez más
                        //     writerTxt.WriteLine("\t \t ENTRANDO JUNTO BINARYSSS");
                        //     preguntasCorrectas = asignacion.Preguntas
                        //     .Where(p => p.Nivel == i && ((p.Estado == 1 && p.Correcta == "Si") || ( p.Estado == 2 && p.Correcta == "No")));
                
                        //     asignacion.Puntuacion = preguntasCorrectas.Sum(x => x.Peso);   
                        //     writerTxt.WriteLine("\t \t " + asignacion.Puntuacion);
                        // }


                        if (preguntas.Count() == preguntasCorrectas.Count())
                        {
                            nivelCompleto = true;
                        }

                        asignacion.NivelAlcanzado = i;
                      
                    }
                }

                //comparamos los niveles de cada asignacion y cogemos el mas bajo
                var minLevel = seccion.Asignaciones.Min(x => x.NivelAlcanzado);
                float sumaPesosAsignaciones = 0;

                foreach (AsignacionConPreguntaNivelDto asignacion in seccion.Asignaciones)
                {
                    sumaNoBinary = 0;
                 
                    // asignacion.Preguntas.ToList().ForEach(f => {
                    //      var noCorrecta = f.Correcta == "Si" ? 1 : MaxRange;
                    //      writerTxt.WriteLine("-->Pregunta - Correcta: "+f.Correcta+"  Estado " + f.Estado +"  Peso: "+f.Peso + " Nivel " + f.Nivel + "resultado final: "+ ((Math.Abs((f.Estado - noCorrecta) / (MaxRange - 1 ))) * f.Peso));
                    // });
  
                    
                    asignacion.NivelAlcanzado = minLevel;
                    // asignacion.Puntuacion = asignacion.Preguntas
                    // .Where(p => p.Nivel == minLevel && ((p.Estado == 1 && p.Correcta == "Si") || (p.Estado == 2 && p.Correcta == "No")))
                    // .Sum(p => p.Peso);

                    
                    asignacion.Preguntas.ToList().ForEach(f => {
                        var noCorrecta = f.Correcta == "Si" ? 1 : MaxRange;
                        if(f.Nivel == minLevel && f.Estado != 0){
                            sumaNoBinary += ((Math.Abs((f.Estado - noCorrecta) / (MaxRange - 1 ))) * f.Peso);
                        }
                    });

                    //sumaNoBinary para puntuación
                    asignacion.Puntuacion = sumaNoBinary;
                    //writerTxt.WriteLine("\t \t Incluye el noBinary!");


                    sumaPesosAsignaciones += asignacion.Peso * asignacion.Puntuacion;
                }

                seccion.NivelAlcanzado = minLevel;
                seccion.Puntuacion = sumaPesosAsignaciones;
                if (seccion.Puntuacion == 0 && seccion.NivelAlcanzado > 1)
                {
                    seccion.NivelAlcanzado = seccion.NivelAlcanzado - 1;
                    seccion.Puntuacion = 100;
                }
      

            //writerTxt.WriteLine("puntuacion "+ seccion.Puntuacion);
          
            }

            

                if(final) {
                sectionsConAsignaciones.ToList().ForEach( x => { 
                    _puntuacionSection.setPuntuacionSection(idEvaluacion, x.SectionId, x.NivelAlcanzado,x.Puntuacion);
                });
            }
            // fin using writer 
            //}
            return sectionsConAsignaciones;
        }

        //Aqui introducimos una nueva respuesta
        public bool AddRespuesta(RespuestaDto respuesta)
        {
            _context.Respuestas.Add(new RespuestaEntity {
                Estado = respuesta.Estado, 
                Notas = respuesta.Notas,
                PreguntaId = respuesta.PreguntaId,
                EvaluacionId = respuesta.EvaluacionId
                });
            return SaveChanges();
        }

        //Elimina una respuesta
        public bool DeleteRespuesta(RespuestaDto respuesta)
        {
            _context.Respuestas.Remove(_context.Respuestas.Where(r => r.Id == respuesta.Id).FirstOrDefault());
            return SaveChanges();
        }

        //Muestra si existe la respuesta
        public bool ExiteRespuesta(int idRespuesta)
        {
            return _context.Respuestas.Any(r => r.Id == idRespuesta);
        }


    }
}
