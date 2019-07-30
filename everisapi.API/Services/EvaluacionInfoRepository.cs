using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using everisapi.API.Entities;
using everisapi.API.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace everisapi.API.Services
{
    public class EvaluacionInfoRepository : IEvaluacionInfoRepository
    {

        private AsignacionInfoContext _context;

        //Le damos un contexto en el constructor
        public EvaluacionInfoRepository(AsignacionInfoContext context)
        {
            _context = context;
        }

        //Recoge una evaluación incluyendo o no las respuestas
        public EvaluacionEntity GetEvaluacion(int IdEvaluacion, bool IncluirRespuestas)
        {
            if (IncluirRespuestas)
            {
                return _context.Evaluaciones.Include(e => e.Respuestas)
                  .Where(e => e.Id == IdEvaluacion).FirstOrDefault();
            }
            else
            {
                return _context.Evaluaciones.Where(e => e.Id == IdEvaluacion).FirstOrDefault();
            }
        }

        //Recoge una evaluacion con datos de información de muchas tablas
        public EvaluacionInfoDto GetEvaluationInfoFromIdEvaluation(int idEvaluation)
        {
            var evaluation = _context.Evaluaciones.
              Include(r => r.ProyectoEntity).
              ThenInclude(p => p.UserEntity).OrderByDescending(e => e.Fecha).
              Include(a => a.Assessment).
              Where(e => e.Id == idEvaluation).First();

            //Encuentra la informacion de la evaluacion y lo introduce en un objeto

            EvaluacionInfoDto EvaluacionInfo = new EvaluacionInfoDto
            {
                Id = evaluation.Id,
                Fecha = evaluation.Fecha,
                Estado = evaluation.Estado,
                Nombre = evaluation.ProyectoEntity.Nombre,
                UserNombre = evaluation.UserNombre,
                NotasEvaluacion = evaluation.NotasEvaluacion,
                NotasObjetivos = evaluation.NotasObjetivos,
                AssessmentId = evaluation.AssessmentId,
                Puntuacion = (float)evaluation.Puntuacion
            };

            // if (evaluation.Estado == false)
            // {
            //   EvaluacionInfo.Puntuacion = CalculaPuntuacion(evaluation.Id,  evaluation.AssessmentId);
            // }
            // else
            // {
            //   EvaluacionInfo.Puntuacion = (float)evaluation.Puntuacion;
            // }

            // if (0 < _context.NotasAsignaciones.Where(r => r.EvaluacionId == evaluation.Id && r.Notas != null && r.Notas != "").Count())
            // {
            //   EvaluacionInfo.FlagNotasAsig = true;
            // }
            // else
            // {
            //   EvaluacionInfo.FlagNotasAsig = false;

            // }

            // if (0 < _context.NotasSections.Where(r => r.EvaluacionId == evaluation.Id && r.Notas != null && r.Notas != "").Count())
            // {
            //   EvaluacionInfo.FlagNotasSec = true;
            // }
            // else
            // {
            //   EvaluacionInfo.FlagNotasSec = false;

            // }

            // var listaev = _context.Evaluaciones.Where(r => r.ProyectoId == evaluation.ProyectoId && r.Estado == true).ToList();
            // double suma = 0;

            // foreach(var ev in listaev)
            // {
            //   suma += ev.Puntuacion;
            // }

            // if (listaev.Count > 0)
            // {
            //   //EvaluacionInfo.Media = suma / listaev.Count;
            // }
            // else
            // {
            //   EvaluacionInfo.Media = -1;
            // }

            return EvaluacionInfo;
        }

        //Recoge una lista de evaluaciones con datos de información de muchas tablas
        public List<EvaluacionInfoDto> GetEvaluationInfo(int IdProject)
        {
            List<EvaluacionInfoDto> EvaluacionesInformativas = new List<EvaluacionInfoDto>();
            var Evaluaciones = _context.Evaluaciones.
              Include(r => r.ProyectoEntity).
              ThenInclude(p => p.UserEntity).OrderByDescending(e => e.Fecha).
               Include(a => a.Assessment).
              Where(e => e.ProyectoId == IdProject).ToList();

            //Encuentra la informacion de la evaluacion y lo introduce en un objeto
            foreach (var evaluacion in Evaluaciones)
            {
                EvaluacionInfoDto EvaluacionInfo = new EvaluacionInfoDto
                {
                    Id = evaluacion.Id,
                    Fecha = evaluacion.Fecha,
                    Estado = evaluacion.Estado,
                    Nombre = evaluacion.ProyectoEntity.Nombre,
                    UserNombre = evaluacion.UserNombre,
                    NotasEvaluacion = evaluacion.NotasEvaluacion,
                    NotasObjetivos = evaluacion.NotasObjetivos
                };


                if (evaluacion.Estado == false)
                {
                    EvaluacionInfo.Puntuacion = CalculaPuntuacion(evaluacion.Id, evaluacion.AssessmentId);
                }
                else
                {
                    EvaluacionInfo.Puntuacion = (float)evaluacion.Puntuacion;
                }

                if (0 < _context.NotasAsignaciones.Where(r => r.EvaluacionId == evaluacion.Id && r.Notas != null && r.Notas != "").Count())
                {
                    EvaluacionInfo.FlagNotasAsig = true;
                }
                else
                {
                    EvaluacionInfo.FlagNotasAsig = false;

                }


                if (0 < _context.NotasSections.Where(r => r.EvaluacionId == evaluacion.Id && r.Notas != null && r.Notas != "").Count())
                {
                    EvaluacionInfo.FlagNotasSec = true;
                }
                else
                {
                    EvaluacionInfo.FlagNotasSec = false;

                }

                var listaev = _context.Evaluaciones.Where(r => r.ProyectoId == evaluacion.ProyectoId && r.Estado == true).ToList();
                double suma = 0;

                foreach (var ev in listaev)
                {
                    suma += ev.Puntuacion;
                }

                if (listaev.Count > 0)
                {
                    //EvaluacionInfo.Media = (float)suma / listaev.Count;
                }
                else
                {
                    EvaluacionInfo.Media = -1;
                }

                //Añade el objeto en la lista
                EvaluacionesInformativas.Add(EvaluacionInfo);
            }
            return EvaluacionesInformativas;
        }

        //Recoge una lista de evaluaciones con datos de información de muchas tablas filtrandola por paginado
        public List<EvaluacionInfoDto> GetEvaluationInfoAndPage(int IdProject, int pageNumber)
        {
            //Recogemos las evaluaciones y la paginamos
            List<EvaluacionInfoDto> EvaluacionesInformativas = new List<EvaluacionInfoDto>();
            var Evaluaciones = _context.Evaluaciones.
              Include(r => r.ProyectoEntity).
              ThenInclude(p => p.UserEntity).
               Include(a => a.Assessment).
              Where(e => e.ProyectoId == IdProject)//.Skip(10 * pageNumber)
              .OrderByDescending(e => e.Fecha).ToList();
            //Encuentra la informacion de la evaluacion y lo introduce en un objeto
            foreach (var evaluacion in Evaluaciones)
            {
                EvaluacionInfoDto EvaluacionInfo = new EvaluacionInfoDto
                {
                    Id = evaluacion.Id,
                    Fecha = evaluacion.Fecha,
                    Estado = evaluacion.Estado,
                    Nombre = evaluacion.ProyectoEntity.Nombre,
                    UserNombre = evaluacion.UserNombre,
                    NotasEvaluacion = evaluacion.NotasEvaluacion,
                    NotasObjetivos = evaluacion.NotasObjetivos,
                    AssessmentName = evaluacion.Assessment.AssessmentName,
                    AssessmentId = evaluacion.AssessmentId
                };

                if (evaluacion.Estado == false)
                {
                    EvaluacionInfo.Puntuacion = CalculaPuntuacion(evaluacion.Id, evaluacion.AssessmentId);
                }
                else
                {
                    EvaluacionInfo.Puntuacion = (float)evaluacion.Puntuacion;
                }

                if (0 < _context.NotasAsignaciones.Where(r => r.EvaluacionId == evaluacion.Id && r.Notas != null && r.Notas != "").Count())
                {
                    EvaluacionInfo.FlagNotasAsig = true;
                }
                else
                {
                    EvaluacionInfo.FlagNotasAsig = false;

                }


                if (0 < _context.NotasSections.Where(r => r.EvaluacionId == evaluacion.Id && r.Notas != null && r.Notas != "").Count())
                {
                    EvaluacionInfo.FlagNotasSec = true;
                }
                else
                {
                    EvaluacionInfo.FlagNotasSec = false;

                }

                var listaev = _context.Evaluaciones.Where(r => r.ProyectoId == evaluacion.ProyectoId && r.Estado == true).ToList();
                double suma = 0;

                foreach (var ev in listaev)
                {
                    suma += ev.Puntuacion;
                }

                if (listaev.Count > 0)
                {
                    EvaluacionInfo.Media = (float)suma / listaev.Count;
                }
                else
                {
                    EvaluacionInfo.Media = -1;
                }

                //Añade el objeto en la lista
                EvaluacionesInformativas.Add(EvaluacionInfo);
            }

            return EvaluacionesInformativas;
        }

        //Recoge todas las evaluaciones
        public IEnumerable<EvaluacionEntity> GetEvaluaciones()
        {
            return _context.Evaluaciones.ToList();
        }

        //Recoge todas las evaluaciones de un proyecto
        public IEnumerable<EvaluacionEntity> GetEvaluacionesFromProject(int IdProject)
        {
            return _context.Evaluaciones.Where(e => e.ProyectoId == IdProject).ToList();
        }

        //Devuelve una evaluacion si existiera una inacabada en la base de datos filtrado por id de projecto
        public EvaluacionEntity EvaluationIncompletaFromProject(int IdProject)
        {
            return _context.Evaluaciones.Where(e => e.ProyectoId == IdProject && !e.Estado).OrderByDescending(e => e.Fecha).FirstOrDefault();
        }

        public EvaluacionEntity EvaluationIncompletaFromProjectAndAssessment(int projectId, int assessmentId)
        {
            return _context.Evaluaciones.Where(e => e.ProyectoId == projectId && e.AssessmentId == assessmentId && !e.Estado).OrderByDescending(e => e.Fecha).FirstOrDefault();
        }


        //Incluye una nueva evaluación a la base de datos
        public void IncluirEvaluacion(EvaluacionEntity evaluacion)
        {
            evaluacion.Respuestas = _context.Preguntas.Where(p => p.AsignacionEntity.SectionEntity.AssessmentId == evaluacion.AssessmentId)
            .Select(p => new RespuestaEntity { EvaluacionId = evaluacion.Id, PreguntaId = p.Id }).ToList();

            _context.Evaluaciones.Add(evaluacion);
            this.SaveChanges();
        }

        //Modifica una evaluacion
        public bool ModificarEvaluacion(EvaluacionEntity evaluacion)
        {
            EvaluacionEntity evaluacionAnterior = _context.Evaluaciones.Where(e => e.Id == evaluacion.Id).FirstOrDefault();
            evaluacionAnterior.NotasEvaluacion = evaluacion.NotasEvaluacion;
            evaluacionAnterior.NotasObjetivos = evaluacion.NotasObjetivos;
            evaluacionAnterior.UserNombre = evaluacion.UserNombre;
            evaluacionAnterior.Fecha = DateTime.Now;

            //Se finaliza una evaluacion y se calcula su puntuacion
            if (evaluacionAnterior.Estado == false && evaluacion.Estado == true)
            {
                evaluacionAnterior.Puntuacion = CalculaPuntuacion(evaluacionAnterior.Id, evaluacionAnterior.AssessmentId);
            }

            evaluacionAnterior.Estado = evaluacion.Estado;

            return SaveChanges();
        }

        //Guarda todos los cambios en la base de datos
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        //Metodo que devuelve un filtrado de evaluaciones y proyecto paginada
        public List<EvaluacionInfoDto> GetEvaluationInfoAndPageFiltered(int IdProject, int pageNumber, EvaluacionInfoPaginationDto Evaluacion)
        {
            //Recogemos las evaluaciones y la paginamos
            List<EvaluacionInfoDto> EvaluacionesInformativas = new List<EvaluacionInfoDto>();
            List<EvaluacionEntity> Evaluaciones;
            if (Evaluacion.Estado != null && Evaluacion.Estado != "")
            {
                Evaluaciones = _context.Evaluaciones.
                Include(r => r.ProyectoEntity).
                ThenInclude(p => p.UserEntity).
                Include(a => a.Assessment).
                Where(e => e.ProyectoId == IdProject &&
                e.Estado == Boolean.Parse(Evaluacion.Estado) &&
                (Evaluacion.AssessmentId == 0 || e.AssessmentId == Evaluacion.AssessmentId) &&
                e.Fecha.Date.ToString("dd/MM/yyyy").Contains(Evaluacion.Fecha) //&&
                                                                               //e.ProyectoEntity.UserNombre.ToLower().Contains(Evaluacion.UserNombre.ToLower())
                ).OrderByDescending(e => e.Fecha).ToList();
            }
            else
            {
                Evaluaciones = _context.Evaluaciones.
                Include(r => r.ProyectoEntity).
                ThenInclude(p => p.UserEntity).
                Include(a => a.Assessment).
                Where(e => e.ProyectoId == IdProject &&
                (Evaluacion.AssessmentId == 0 || e.AssessmentId == Evaluacion.AssessmentId) &&
                e.Fecha.Date.ToString("dd/MM/yyyy").Contains(Evaluacion.Fecha) //&&
                                                                               //e.ProyectoEntity.UserNombre.ToLower().Contains(Evaluacion.UserNombre.ToLower())
                ).OrderByDescending(e => e.Fecha).ToList();
            }
            //Encuentra la informacion de la evaluacion y lo introduce en un objeto
            foreach (var evaluacion in Evaluaciones)
            {
                EvaluacionInfoDto EvaluacionInfo = new EvaluacionInfoDto
                {
                    Id = evaluacion.Id,
                    Fecha = evaluacion.Fecha,
                    Estado = evaluacion.Estado,
                    Nombre = evaluacion.ProyectoEntity.Nombre,
                    UserNombre = evaluacion.UserNombre,
                    NotasEvaluacion = evaluacion.NotasEvaluacion,
                    NotasObjetivos = evaluacion.NotasObjetivos,
                    AssessmentName = evaluacion.Assessment.AssessmentName,
                    AssessmentId = evaluacion.AssessmentId,
                    Puntuacion = (float)evaluacion.Puntuacion
                };


                //if(evaluacion.Estado == false)
                //{
                //EvaluacionInfo.Puntuacion = CalculaPuntuacion(evaluacion.Id, evaluacion.AssessmentId);
                //}
                // else
                // {
                //   EvaluacionInfo.Puntuacion = evaluacion.Puntuacion;
                // }

                // if (0 < _context.NotasAsignaciones.Where(r => r.EvaluacionId == evaluacion.Id && r.Notas != null && r.Notas != "").Count())
                // {
                //   EvaluacionInfo.FlagNotasAsig = true;
                // }
                // else
                // {
                //   EvaluacionInfo.FlagNotasAsig = false;

                // }


                // if (0 < _context.NotasSections.Where(r => r.EvaluacionId == evaluacion.Id && r.Notas != null && r.Notas != "").Count())
                // {
                //   EvaluacionInfo.FlagNotasSec = true;
                // }
                // else
                // {
                //   EvaluacionInfo.FlagNotasSec = false;

                // }

                // var listaev = _context.Evaluaciones.Where(r => r.ProyectoId == evaluacion.ProyectoId && r.Estado == true).ToList();
                // double suma = 0;

                // foreach (var ev in listaev)
                // {
                //   suma += ev.Puntuacion;
                // }

                // if (listaev.Count > 0)
                // {
                //   EvaluacionInfo.Media = (float)suma / listaev.Count;
                // }
                // else
                // {
                //   EvaluacionInfo.Media = -1;
                // }


                //Añade el objeto en la lista
                EvaluacionesInformativas.Add(EvaluacionInfo);
            }


            return EvaluacionesInformativas.ToList();//.Skip(10 * pageNumber)
        }

        public List<EvaluacionInfoWithSectionsDto> GetEvaluationsWithSectionsInfo(int IdProject, EvaluacionInfoPaginationDto Evaluacion, int codigoIdioma)
        {
            //Recogemos las evaluaciones y la paginamos
            List<EvaluacionInfoWithSectionsDto> EvaluacionesInformativas = new List<EvaluacionInfoWithSectionsDto>();
            List<EvaluacionEntity> Evaluaciones;
            if (Evaluacion.Estado != null && Evaluacion.Estado != "")
            {
                Evaluaciones = _context.Evaluaciones.
                Include(r => r.ProyectoEntity).
                ThenInclude(p => p.UserEntity).
                Include(a => a.Assessment).
                Where(e => e.ProyectoId == IdProject &&
                e.Estado == Boolean.Parse(Evaluacion.Estado) &&
                (Evaluacion.AssessmentId == 0 || e.AssessmentId == Evaluacion.AssessmentId) &&
                e.Fecha.Date.ToString("dd/MM/yyyy").Contains(Evaluacion.Fecha) //&&
                                                                               //e.ProyectoEntity.UserNombre.ToLower().Contains(Evaluacion.UserNombre.ToLower())
                ).OrderByDescending(e => e.Fecha).ToList();
            }
            else
            {
                Evaluaciones = _context.Evaluaciones.
                Include(r => r.ProyectoEntity).
                ThenInclude(p => p.UserEntity).
                Include(a => a.Assessment).
                Where(e => e.ProyectoId == IdProject &&
                (Evaluacion.AssessmentId == 0 || e.AssessmentId == Evaluacion.AssessmentId) &&
                e.Fecha.Date.ToString("dd/MM/yyyy").Contains(Evaluacion.Fecha) //&&
                                                                               //e.ProyectoEntity.UserNombre.ToLower().Contains(Evaluacion.UserNombre.ToLower())
                ).OrderByDescending(e => e.Fecha).ToList();
            }
            //Encuentra la informacion de la evaluacion y lo introduce en un objeto
            foreach (var evaluacion in Evaluaciones)
            {
                EvaluacionInfoWithSectionsDto EvaluacionInfo = new EvaluacionInfoWithSectionsDto
                {
                    Id = evaluacion.Id,
                    Fecha = evaluacion.Fecha,
                    Estado = evaluacion.Estado,
                    Nombre = evaluacion.ProyectoEntity.Nombre,
                    UserNombre = evaluacion.UserNombre,
                    AssessmentName = evaluacion.Assessment.AssessmentName,
                    AssessmentId = evaluacion.AssessmentId,
                    //NotasEvaluacion = evaluacion.NotasEvaluacion
                    //NotasObjetivos = evaluacion.NotasObjetivos
                };


                EvaluacionInfo.SectionsInfo = new List<SectionInfoDto>();


                List<SectionConAsignacionesDto> sectionsConAsignaciones = new List<SectionConAsignacionesDto>();

                // calculo de puntuaciones por seccion

                List<SectionEntity> sections = _context.Sections.Where(r => r.AssessmentId == evaluacion.AssessmentId).ToList();
                foreach (var s in sections)
                {
                    SectionConAsignacionesDto sectionConAsignacion = new SectionConAsignacionesDto();
                    sectionConAsignacion.EvaluacionId = evaluacion.Id;
                    sectionConAsignacion.SectionId = s.Id;
                    // sectionConAsignacion.Nombre = s.Nombre;
                    sectionConAsignacion.Nombre = "";
                    sectionConAsignacion.Peso = s.Peso;
                    sectionConAsignacion.PesoNivel1 = s.PesoNivel1;
                    sectionConAsignacion.PesoNivel2 = s.PesoNivel2;
                    sectionConAsignacion.PesoNivel3 = s.PesoNivel3;

                    List<AsignacionConPreguntaNivelDto> asignacionesConPreguntaNivel = new List<AsignacionConPreguntaNivelDto>();

                    List<AsignacionEntity> asignaciones = _context.Asignaciones.Where(a => a.SectionId == s.Id).ToList();
                    foreach (var a in asignaciones)
                    {
                        AsignacionConPreguntaNivelDto asignacionConPreguntaNivel = new AsignacionConPreguntaNivelDto();
                        asignacionConPreguntaNivel.Id = a.Id;
                        //asignacionConPreguntaNivel.Nombre = a.Nombre;
                        asignacionConPreguntaNivel.Nombre = "";
                        asignacionConPreguntaNivel.Peso = a.Peso;

                        List<PreguntaRespuestaNivelDto> preguntasRespuestaNivel = new List<PreguntaRespuestaNivelDto>();
                        List<RespuestaEntity> preguntas = new List<RespuestaEntity>();
                        preguntas = _context.Respuestas.
                        Include(r => r.PreguntaEntity).Where(p => p.EvaluacionId == evaluacion.Id && p.PreguntaEntity.AsignacionId == a.Id).ToList();

                        foreach (RespuestaEntity p in preguntas)
                        {
                            PreguntaRespuestaNivelDto preguntaRespuestaNivel = new PreguntaRespuestaNivelDto();
                            preguntaRespuestaNivel.Id = p.PreguntaEntity.Id;
                            preguntaRespuestaNivel.Nivel = p.PreguntaEntity.Nivel;
                            preguntaRespuestaNivel.Peso = p.PreguntaEntity.Peso;
                            preguntaRespuestaNivel.Estado = p.Estado;
                            preguntaRespuestaNivel.Correcta = p.PreguntaEntity.Correcta;

                            preguntasRespuestaNivel.Add(preguntaRespuestaNivel);
                        }
                        asignacionConPreguntaNivel.Preguntas = preguntasRespuestaNivel;

                        asignacionesConPreguntaNivel.Add(asignacionConPreguntaNivel);
                    }
                    sectionConAsignacion.Asignaciones = asignacionesConPreguntaNivel;

                    sectionsConAsignaciones.Add(sectionConAsignacion);
                }

                float sumSections = 0;
                foreach (SectionConAsignacionesDto seccion in sectionsConAsignaciones)
                {
                    //calculamos los niveles individuales para cada asignacion
                    foreach (AsignacionConPreguntaNivelDto asignacion in seccion.Asignaciones)
                    {
                        var maxLevel = asignacion.Preguntas.Max(x => x.Nivel);
                        bool nivelCompleto = true;
                        for (int i = 1; i <= maxLevel && nivelCompleto; i++)
                        {
                            nivelCompleto = false;
                            var preguntas = asignacion.Preguntas.Where(p => p.Nivel == i);

                            var preguntasCorrectas = asignacion.Preguntas
                            .Where(p => p.Nivel == i && ((p.Estado == 1 && p.Correcta == "Si") || (p.Estado == 2 && p.Correcta == "No")));

                            if (preguntas.Count() == preguntasCorrectas.Count())
                            {
                                nivelCompleto = true;
                            }

                            asignacion.NivelAlcanzado = i;
                            asignacion.Puntuacion = preguntasCorrectas.Sum(x => x.Peso);
                        }
                    }

                    //comparamos los niveles de cada asignacion y cogemos el mas bajo
                    var minLevel = seccion.Asignaciones.Min(x => x.NivelAlcanzado);
                    float sumaPesosAsignaciones = 0;
                    foreach (AsignacionConPreguntaNivelDto asignacion in seccion.Asignaciones)
                    {
                        asignacion.NivelAlcanzado = minLevel;
                        asignacion.Puntuacion = asignacion.Preguntas
                        .Where(p => p.Nivel == minLevel && ((p.Estado == 1 && p.Correcta == "Si") || (p.Estado == 2 && p.Correcta == "No")))
                        .Sum(p => p.Peso);

                        sumaPesosAsignaciones += asignacion.Puntuacion * asignacion.Peso;
                    }

                    seccion.NivelAlcanzado = minLevel;
                    seccion.Puntuacion = sumaPesosAsignaciones;

                    if (seccion.Puntuacion == 0 && seccion.NivelAlcanzado > 1)
                    {
                        seccion.NivelAlcanzado = seccion.NivelAlcanzado - 1;
                        seccion.Puntuacion = 100;
                    }

                    SectionInfoDto sec = new SectionInfoDto();
                    sec.Puntuacion = seccion.Puntuacion;
                    sec.NivelAlcanzado = seccion.NivelAlcanzado;
                    //sec.Nombre = seccion.Nombre;
                    sec.Nombre = _context.TraduccionesSections.Where(t => t.SectionsId == seccion.SectionId && t.IdiomaId == codigoIdioma).Select(t => t.Traduccion).FirstOrDefault();
                    EvaluacionInfo.SectionsInfo.Add(sec);
                    Console.WriteLine("SOY" + seccion.SectionId);
                    if (seccion.NivelAlcanzado == 3)
                    {
                        seccion.Puntuacion = seccion.PesoNivel1 + seccion.PesoNivel2 + (seccion.Puntuacion * ((float)seccion.PesoNivel3 / 100));
                        seccion.Puntuacion = (seccion.Puntuacion * seccion.Peso) / 100;
                    }

                    if (seccion.NivelAlcanzado == 2)
                    {
                        seccion.Puntuacion = seccion.PesoNivel1 + (seccion.Puntuacion * ((float)seccion.PesoNivel2 / 100));
                        seccion.Puntuacion = (seccion.Puntuacion * seccion.Peso) / 100;
                    }

                    if (seccion.NivelAlcanzado == 1)
                    {
                        seccion.Puntuacion = ((float)seccion.PesoNivel1 / 100) * seccion.Puntuacion;
                        seccion.Puntuacion = (seccion.Puntuacion * seccion.Peso) / 100;
                    }

                    sumSections += seccion.Puntuacion;
                }

                EvaluacionInfo.Puntuacion = (float)Math.Round(sumSections, 1);


                //Añade el objeto en la lista
                EvaluacionesInformativas.Add(EvaluacionInfo);
            }


            return EvaluacionesInformativas.ToList();
        }

        public List<EvaluacionInfoWithProgressDto> GetEvaluationsWithProgress(int IdProject, EvaluacionInfoPaginationDto Evaluacion)
        {
            //Recogemos las evaluaciones y la paginamos
            List<EvaluacionInfoWithProgressDto> EvaluacionesInformativas = new List<EvaluacionInfoWithProgressDto>();
            List<EvaluacionEntity> Evaluaciones;
            if (Evaluacion.Estado != null && Evaluacion.Estado != "")
            {
                Evaluaciones = _context.Evaluaciones.
                Include(r => r.ProyectoEntity).
                ThenInclude(p => p.UserEntity).
                Include(a => a.Assessment).
                Where(e => e.ProyectoId == IdProject &&
                e.Estado == Boolean.Parse(Evaluacion.Estado) &&
                (Evaluacion.AssessmentId == 0 || e.AssessmentId == Evaluacion.AssessmentId) &&
                e.Fecha.Date.ToString("dd/MM/yyyy").Contains(Evaluacion.Fecha) //&&
                                                                               //e.ProyectoEntity.UserNombre.ToLower().Contains(Evaluacion.UserNombre.ToLower())
                ).OrderByDescending(e => e.Fecha).ToList();
            }
            else
            {
                Evaluaciones = _context.Evaluaciones.
                Include(r => r.ProyectoEntity).
                ThenInclude(p => p.UserEntity).
                Include(a => a.Assessment).
                Where(e => e.ProyectoId == IdProject &&
                (Evaluacion.AssessmentId == 0 || e.AssessmentId == Evaluacion.AssessmentId) &&
                e.Fecha.Date.ToString("dd/MM/yyyy").Contains(Evaluacion.Fecha) //&&
                                                                               //e.ProyectoEntity.UserNombre.ToLower().Contains(Evaluacion.UserNombre.ToLower())
                ).OrderByDescending(e => e.Fecha).ToList();
            }
            //Encuentra la informacion de la evaluacion y lo introduce en un objeto
            foreach (var evaluacion in Evaluaciones)
            {
                EvaluacionInfoWithProgressDto EvaluacionInfo = new EvaluacionInfoWithProgressDto
                {
                    Id = evaluacion.Id,
                    Fecha = evaluacion.Fecha,
                    Nombre = evaluacion.ProyectoEntity.Nombre,
                    UserNombre = evaluacion.UserNombre,
                    AssessmentName = evaluacion.Assessment.AssessmentName,
                    AssessmentId = evaluacion.AssessmentId,
                    numQuestions = _context.Respuestas.
                    Include(r => r.PreguntaEntity).
                    ThenInclude(rp => rp.AsignacionEntity).
                    ThenInclude(rpa => rpa.SectionEntity).
                    Where(r => r.EvaluacionId == evaluacion.Id && r.PreguntaEntity.AsignacionEntity.SectionEntity.AssessmentId == evaluacion.AssessmentId).Count(),
                    progress = CalculateEvaluationProgress(evaluacion.Id, evaluacion.AssessmentId)
                };
                //Añade el objeto en la lista
                EvaluacionesInformativas.Add(EvaluacionInfo);
            }


            return EvaluacionesInformativas.ToList();
        }

        public List<EvaluacionInfoWithProgressDto> GetAllEvaluationsWithProgress(EvaluacionInfoPaginationDto EvaluacionParaFiltrar, string userNombre)
        {
            UserEntity user = _context.Users.Where(u => u.Nombre == userNombre).FirstOrDefault();
            List<UserProyectoEntity> equiposUsuario = new List<UserProyectoEntity>();
            List<int> equipos = new List<int>();
            if (user.RoleId == (int)Roles.User)
            {
                equiposUsuario = _context.UserProyectos.Where(up => up.UserNombre == userNombre).ToList();
                foreach (var eu in equiposUsuario)
                {
                    equipos.Add(eu.ProyectoId);
                }
            }

            List<EvaluacionInfoWithProgressDto> EvaluacionesInformativas = new List<EvaluacionInfoWithProgressDto>();
            List<EvaluacionEntity> Evaluaciones;

            Evaluaciones = _context.Evaluaciones.
            Include(r => r.ProyectoEntity).
            ThenInclude(p => p.UserEntity).
            Include(a => a.Assessment).
            Where(e =>
            (equipos.Count > 0 ? (equipos.Contains(e.ProyectoId)) : 1 == 1) &&
            e.Estado == Boolean.Parse(EvaluacionParaFiltrar.Estado) &&
            e.ProyectoEntity.TestProject == false &&
            (EvaluacionParaFiltrar.Oficinas.Length > 0 ? (Array.Exists(EvaluacionParaFiltrar.Oficinas, element => element == e.ProyectoEntity.Oficina)) : 1 == 1) &&
            (EvaluacionParaFiltrar.Equipos.Length > 0 ? (Array.Exists(EvaluacionParaFiltrar.Equipos, element => element == e.ProyectoId)) : 1 == 1) //&&
            //(EvaluacionParaFiltrar.Nombre != "" ? (e.ProyectoEntity.Nombre == EvaluacionParaFiltrar.Nombre) : 1 == 1)
            ).OrderByDescending(e => e.Fecha).ToList();

            //Encuentra la informacion de la evaluacion y lo introduce en un objeto
            foreach (var evaluacion in Evaluaciones)
            {
                EvaluacionInfoWithProgressDto EvaluacionInfo = new EvaluacionInfoWithProgressDto
                {
                    Id = evaluacion.Id,
                    Fecha = evaluacion.Fecha,
                    Nombre = evaluacion.ProyectoEntity.Nombre,
                    UserNombre = evaluacion.UserNombre,
                    AssessmentName = evaluacion.Assessment.AssessmentName,
                    AssessmentId = evaluacion.AssessmentId,
                    oficina = evaluacion.ProyectoEntity.Oficina,
                    numQuestions = _context.Respuestas.
                    Include(r => r.PreguntaEntity).
                    ThenInclude(rp => rp.AsignacionEntity).
                    ThenInclude(rpa => rpa.SectionEntity).
                    Where(r => r.EvaluacionId == evaluacion.Id && r.PreguntaEntity.AsignacionEntity.SectionEntity.AssessmentId == evaluacion.AssessmentId).Count(),
                    progress = CalculateEvaluationProgress(evaluacion.Id, evaluacion.AssessmentId)
                };
                //Añade el objeto en la lista
                EvaluacionesInformativas.Add(EvaluacionInfo);
            }
            return EvaluacionesInformativas.ToList();
        }

        //Metodo que devuelve un filtrado de evaluaciones paginada sin proyectos
        public List<EvaluacionInfoDto> GetEvaluationInfoAndPageFilteredAdmin(int pageNumber, EvaluacionInfoPaginationDto Evaluacion)
        {
            //Recogemos las evaluaciones y la paginamos
            List<EvaluacionInfoDto> EvaluacionesInformativas = new List<EvaluacionInfoDto>();
            List<EvaluacionEntity> Evaluaciones;
            if (Evaluacion.Estado != null && Evaluacion.Estado != "")
            {
                Evaluaciones = _context.Evaluaciones.
                Include(r => r.ProyectoEntity).
                ThenInclude(p => p.UserEntity).
                 Include(a => a.Assessment).
                Where(e => e.Estado == Boolean.Parse(Evaluacion.Estado) &&
                e.Fecha.Date.ToString("dd/MM/yyyy").Contains(Evaluacion.Fecha) //&&
                                                                               //e.ProyectoEntity.UserNombre.ToLower().Contains(Evaluacion.UserNombre.ToLower())
                ).OrderByDescending(e => e.Fecha).ToList();
            }
            else
            {
                Evaluaciones = _context.Evaluaciones.
                Include(r => r.ProyectoEntity).
                ThenInclude(p => p.UserEntity).
                 Include(a => a.Assessment).
                Where(e => e.Fecha.Date.ToString("dd/MM/yyyy").Contains(Evaluacion.Fecha) &&
                e.ProyectoEntity.Nombre.Contains(Evaluacion.Nombre) //&&
                                                                    //e.ProyectoEntity.UserNombre.ToLower().Contains(Evaluacion.UserNombre.ToLower())
                ).OrderByDescending(e => e.Fecha).ToList();
            }


            //Encuentra la informacion de la evaluacion y lo introduce en un objeto
            foreach (var evaluacion in Evaluaciones)
            {
                EvaluacionInfoDto EvaluacionInfo = new EvaluacionInfoDto
                {
                    Id = evaluacion.Id,
                    Fecha = evaluacion.Fecha,
                    Estado = evaluacion.Estado,
                    Nombre = evaluacion.ProyectoEntity.Nombre,
                    UserNombre = evaluacion.UserNombre,
                    NotasEvaluacion = evaluacion.NotasEvaluacion,
                    NotasObjetivos = evaluacion.NotasObjetivos,
                    AssessmentName = evaluacion.Assessment.AssessmentName,
                    AssessmentId = evaluacion.AssessmentId,
                    Puntuacion = (float)evaluacion.Puntuacion
                };

                // if (evaluacion.Estado == false)
                // {
                //   EvaluacionInfo.Puntuacion = CalculaPuntuacion(evaluacion.Id, evaluacion.AssessmentId);
                // }
                // else
                // {
                //   EvaluacionInfo.Puntuacion = (float)evaluacion.Puntuacion;
                // }


                if (0 < _context.NotasAsignaciones.Where(r => r.EvaluacionId == evaluacion.Id && r.Notas != null && r.Notas != "").Count())
                {
                    EvaluacionInfo.FlagNotasAsig = true;
                }
                else
                {
                    EvaluacionInfo.FlagNotasAsig = false;

                }


                if (0 < _context.NotasSections.Where(r => r.EvaluacionId == evaluacion.Id && r.Notas != null && r.Notas != "").Count())
                {
                    EvaluacionInfo.FlagNotasSec = true;
                }
                else
                {
                    EvaluacionInfo.FlagNotasSec = false;

                }

                var listaev = _context.Evaluaciones.Where(r => r.ProyectoId == evaluacion.ProyectoId && r.Estado == true).ToList();
                double suma = 0;

                foreach (var ev in listaev)
                {
                    suma += ev.Puntuacion;
                }

                if (listaev.Count > 0)
                {
                    //EvaluacionInfo.Media = (float)suma / listaev.Count;
                }
                else
                {
                    EvaluacionInfo.Media = -1;
                }


                //Añade el objeto en la lista
                EvaluacionesInformativas.Add(EvaluacionInfo);
            }
            return EvaluacionesInformativas;
        }

        //Devuelve el número de proyectos de un proyecto o de todos los proyectos
        public int GetNumEval(int idProject)
        {
            //Si es distinto de cero filtra sino filtra por todos
            if (idProject != 0)
            {
                return _context.Evaluaciones.Where(e => e.ProyectoId == idProject).Count();
            }
            else
            {
                return _context.Evaluaciones.Count();
            }
        }


        //Este metodo nos permite eliminar una evaluacion en concreto 
        public bool EvaluationDelete(int evaluationId)
        {
            //Las notas de las preguntas se borran junto a las respuestas
            _context.Respuestas.RemoveRange(_context.Respuestas.Where(e => e.EvaluacionId == evaluationId));
            _context.NotasAsignaciones.RemoveRange(_context.NotasAsignaciones.Where(e => e.EvaluacionId == evaluationId));
            _context.NotasSections.RemoveRange(_context.NotasSections.Where(e => e.EvaluacionId == evaluationId));
            _context.Evaluaciones.Remove(_context.Evaluaciones.Where(e => e.Id == evaluationId).FirstOrDefault());
            return SaveChanges();
        }

        private float CalculaPuntuacion(int idEvaluacion, int AssessmentId)
        {
            List<SectionConAsignacionesDto> sectionsConAsignaciones = new List<SectionConAsignacionesDto>();


            // calculo de puntuaciones por seccion



            List<SectionEntity> sections = _context.Sections.Where(r => r.AssessmentId == AssessmentId).ToList();
            foreach (var s in sections)
            {
                SectionConAsignacionesDto sectionConAsignacion = new SectionConAsignacionesDto();
                sectionConAsignacion.EvaluacionId = idEvaluacion;
                sectionConAsignacion.SectionId = s.Id;
                // sectionConAsignacion.Nombre = s.Nombre;
                sectionConAsignacion.Nombre = "";
                sectionConAsignacion.Peso = s.Peso;
                sectionConAsignacion.PesoNivel1 = s.PesoNivel1;
                sectionConAsignacion.PesoNivel2 = s.PesoNivel2;
                sectionConAsignacion.PesoNivel3 = s.PesoNivel3;

                List<AsignacionConPreguntaNivelDto> asignacionesConPreguntaNivel = new List<AsignacionConPreguntaNivelDto>();

                List<AsignacionEntity> asignaciones = _context.Asignaciones.Where(a => a.SectionId == s.Id).ToList();
                foreach (var a in asignaciones)
                {
                    AsignacionConPreguntaNivelDto asignacionConPreguntaNivel = new AsignacionConPreguntaNivelDto();
                    asignacionConPreguntaNivel.Id = a.Id;
                    //asignacionConPreguntaNivel.Nombre = a.Nombre;
                    asignacionConPreguntaNivel.Nombre = "";
                    asignacionConPreguntaNivel.Peso = a.Peso;

                    List<PreguntaRespuestaNivelDto> preguntasRespuestaNivel = new List<PreguntaRespuestaNivelDto>();
                    List<RespuestaEntity> preguntas = new List<RespuestaEntity>();
                    preguntas = _context.Respuestas.
                    Include(r => r.PreguntaEntity).Where(p => p.EvaluacionId == idEvaluacion && p.PreguntaEntity.AsignacionId == a.Id).ToList();

                    foreach (RespuestaEntity p in preguntas)
                    {
                        PreguntaRespuestaNivelDto preguntaRespuestaNivel = new PreguntaRespuestaNivelDto();
                        preguntaRespuestaNivel.Id = p.PreguntaEntity.Id;
                        preguntaRespuestaNivel.Nivel = p.PreguntaEntity.Nivel;
                        preguntaRespuestaNivel.Peso = p.PreguntaEntity.Peso;
                        preguntaRespuestaNivel.Estado = p.Estado;
                        preguntaRespuestaNivel.Correcta = p.PreguntaEntity.Correcta;

                        preguntasRespuestaNivel.Add(preguntaRespuestaNivel);
                    }
                    asignacionConPreguntaNivel.Preguntas = preguntasRespuestaNivel;

                    asignacionesConPreguntaNivel.Add(asignacionConPreguntaNivel);
                }
                sectionConAsignacion.Asignaciones = asignacionesConPreguntaNivel;

                sectionsConAsignaciones.Add(sectionConAsignacion);
            }

            float sumSections = 0;
            foreach (SectionConAsignacionesDto seccion in sectionsConAsignaciones)
            {
                //calculamos los niveles individuales para cada asignacion
                foreach (AsignacionConPreguntaNivelDto asignacion in seccion.Asignaciones)
                {
                    var maxLevel = asignacion.Preguntas.Max(x => x.Nivel);
                    bool nivelCompleto = true;
                    for (int i = 1; i <= maxLevel && nivelCompleto; i++)
                    {
                        nivelCompleto = false;
                        var preguntas = asignacion.Preguntas.Where(p => p.Nivel == i);

                        var preguntasCorrectas = asignacion.Preguntas
                        .Where(p => p.Nivel == i && ((p.Estado == 1 && p.Correcta == "Si") || (p.Estado == 2 && p.Correcta == "No")));

                        if (preguntas.Count() == preguntasCorrectas.Count())
                        {
                            nivelCompleto = true;
                        }

                        asignacion.NivelAlcanzado = i;
                        asignacion.Puntuacion = preguntasCorrectas.Sum(x => x.Peso);
                    }
                }

                //comparamos los niveles de cada asignacion y cogemos el mas bajo
                var minLevel = seccion.Asignaciones.Min(x => x.NivelAlcanzado);
                float sumaPesosAsignaciones = 0;
                foreach (AsignacionConPreguntaNivelDto asignacion in seccion.Asignaciones)
                {
                    asignacion.NivelAlcanzado = minLevel;
                    asignacion.Puntuacion = asignacion.Preguntas
                    .Where(p => p.Nivel == minLevel && ((p.Estado == 1 && p.Correcta == "Si") || (p.Estado == 2 && p.Correcta == "No")))
                    .Sum(p => p.Peso);

                    sumaPesosAsignaciones += asignacion.Puntuacion * asignacion.Peso;
                }

                seccion.NivelAlcanzado = minLevel;
                seccion.Puntuacion = sumaPesosAsignaciones;

                if (seccion.Puntuacion == 0 && seccion.NivelAlcanzado > 1)
                {
                    seccion.NivelAlcanzado = seccion.NivelAlcanzado - 1;
                    seccion.Puntuacion = 100;
                }

                //Calculamos el Total segun los pesos de los niveles

                if (seccion.NivelAlcanzado == 3)
                {
                    seccion.Puntuacion = seccion.PesoNivel1 + seccion.PesoNivel2 + (seccion.Puntuacion * ((float)seccion.PesoNivel3 / 100));
                    seccion.Puntuacion = (seccion.Puntuacion * seccion.Peso) / 100;
                }

                if (seccion.NivelAlcanzado == 2)
                {
                    seccion.Puntuacion = seccion.PesoNivel1 + (seccion.Puntuacion * ((float)seccion.PesoNivel2 / 100));
                    seccion.Puntuacion = (seccion.Puntuacion * seccion.Peso) / 100;
                }

                if (seccion.NivelAlcanzado == 1)
                {
                    seccion.Puntuacion = ((float)seccion.PesoNivel1 / 100) * seccion.Puntuacion;
                    seccion.Puntuacion = (seccion.Puntuacion * seccion.Peso) / 100;
                }

                sumSections += seccion.Puntuacion;


            }


            return (float)Math.Round(sumSections, 1);
        }

        //Metodo encargado de calcular el porcentaje respondido de la evaluacion
        public float CalculateEvaluationProgress(int idEvaluation, int idAssessment)
        {
            float progress;
            //Id de asignaciones asociadas al actual assessment
            var assignmentId = _context.Asignaciones
              .Where(x => x.SectionEntity.AssessmentId == idAssessment)
              .Select(x => x.Id)
              .ToList();

            //Id de preguntas habilitantes asociadas a la actual evaluacion
            var enablingQuestions = _context.Preguntas
              .Where(x => assignmentId.Contains(x.AsignacionId)
                      && x.EsHabilitante)
              .Select(x => x.Id)
              .ToList();

            //Total de preguntas que NO son habilitantes ni necesitan de una habilitante
            var genericQuestions = _context.Preguntas
              .Count(x => assignmentId.Contains(x.AsignacionId)
                      && x.PreguntaHabilitanteId == null
                      && !x.EsHabilitante);

            //Id de preguntas habilitantes que han sido respondidas con un Si
            var enablingQuestionsYes = _context.Respuestas
              .Where(x => x.EvaluacionId == idEvaluation
                      && x.Estado == 1
                      && enablingQuestions.Contains(x.PreguntaId))
              .Select(x => x.PreguntaId)
              .ToList();

            //Total de preguntas que dependen de una habilitante y YA estan habilitadas para responder (cuya habilitante se ha respondido con un Si)
            var totalEnableQuestions = _context.Preguntas
              .Count(x => assignmentId.Contains(x.AsignacionId)
                      && x.PreguntaHabilitanteId != null
                      && enablingQuestionsYes.Contains(x.PreguntaHabilitanteId.Value));

            //Total de preguntas necesarias para responder
            var totalRequired = genericQuestions + enablingQuestions.Count + totalEnableQuestions;

            //Total de preguntas respondidas validas hasta la actualidad
            var totalAnswered = _context.Respuestas
              .Count(x => x.EvaluacionId == idEvaluation
                      && x.Estado != 0);

            //Se retorna el porcentaje progreso actual de la evaluacion
            progress = ((float)totalAnswered / (float)totalRequired) * 100;

            return (float)Math.Round(progress, 2);
        }

        //Metodo encargado generar un objeto SectionInfoDto a partir de una evaluationId
        public IEnumerable<SectionInfoDto> GetSectionsInfoFromEval(int idEvaluacion)
        {
            //Recoge las respuestas de la evaluación
            List<SectionInfoDto> ListadoSectionInformacion = new List<SectionInfoDto>();
            var Respuestas = _context.Respuestas.
              Include(r => r.PreguntaEntity).
                ThenInclude(rp => rp.AsignacionEntity).
                  ThenInclude(rpa => rpa.SectionEntity).
              Where(r => r.EvaluacionId == idEvaluacion).ToList();

            //Saca las en que secciones estuvo en ese momento
            var SectionsUtilizadas = Respuestas.Select(r => r.PreguntaEntity.AsignacionEntity.SectionEntity).Distinct().ToList();


            //Rellena los datos y los añade a la lista para cada sección
            foreach (var section in SectionsUtilizadas)
            {
                SectionInfoDto SectionAdd = new SectionInfoDto
                {
                    Id = section.Id,
                    // Nombre = section.Nombre,
                    Nombre = "",
                    Preguntas = Respuestas.Where(r => r.PreguntaEntity.AsignacionEntity.SectionEntity.Id == section.Id).Count(),
                };

                var notasSec = _context.NotasSections.Where(r => r.SectionId == section.Id && r.EvaluacionId == idEvaluacion).FirstOrDefault();

                if (notasSec == null)
                {
                    notasSec = new NotasSectionsEntity
                    {
                        EvaluacionEntity = _context.Evaluaciones.Where(s => s.Id == idEvaluacion).FirstOrDefault(),
                        SectionEntity = _context.Sections.Where(s => s.Id == section.Id).FirstOrDefault()
                    };

                    _context.NotasSections.Add(notasSec);
                }

                SectionAdd.Notas = notasSec.Notas;

                //Para calcular el progreso
                var listaAsignaciones = _context.Asignaciones.Where(r => r.SectionId == section.Id).ToList();

                int contestadas = 0;
                foreach (AsignacionEntity asig in listaAsignaciones)
                {
                    var respuestasAsig = Respuestas.Where(p => p.PreguntaEntity.AsignacionEntity.Id == asig.Id).ToList();

                    //Para ver si la primera es de las que habilitan a las demás o no
                    //y si está contestada a NO (para contar las demás como contestadas
                    bool flag = false;
                    if (respuestasAsig[0].PreguntaEntity.Correcta == null && respuestasAsig[0].Estado == 2)
                    {
                        flag = true;
                    }


                    foreach (RespuestaEntity resp in respuestasAsig)
                    {
                        if (flag)
                        {
                            contestadas++;
                        }
                        else if (resp.Estado != 0)
                        {
                            contestadas++;
                        }
                    }
                }

                SectionAdd.Contestadas = contestadas;
                SectionAdd.Progreso = Math.Round(((double)SectionAdd.Contestadas / SectionAdd.Preguntas) * 100, 1);


                var listaRespuestas = Respuestas.Where(r => r.PreguntaEntity.AsignacionEntity.SectionEntity.Id == section.Id).ToList();
                double suma = 0;
                double puntosCorrectos = 0;

                foreach (var resp in listaRespuestas)
                {
                    if (resp.PreguntaEntity.Correcta != null)
                    {
                        var maxPuntos = Respuestas.Where(r => r.PreguntaEntity.Correcta != null && r.PreguntaEntity.AsignacionId == resp.PreguntaEntity.AsignacionId).Count();
                        var puntos = (double)resp.PreguntaEntity.AsignacionEntity.Peso / maxPuntos;

                        puntosCorrectos += puntos;

                        if (resp.Estado == 1 && resp.PreguntaEntity.Correcta.Equals("Si"))
                        {
                            suma += puntos;
                        }

                        else if (resp.Estado == 2 && resp.PreguntaEntity.Correcta.Equals("No"))
                        {
                            suma += puntos;
                        }

                    }
                }

                SectionAdd.RespuestasCorrectas = Math.Round(100 * suma / puntosCorrectos, 1);

                ListadoSectionInformacion.Add(SectionAdd);
            }

            return ListadoSectionInformacion;
        }
    }
}
