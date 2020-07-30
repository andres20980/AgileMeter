using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Models
{
    public class EvaluacionInfoWithSectionsDto
    {
    public int Id { get; set; }

    public string Nombre { get; set; }

    public float Puntuacion { get; set; }

    public DateTime Fecha { get; set; }

    public bool Estado { get; set; }

    public string Oficina { get; set; }
    
    public int AssessmentId {get;set;}

    public string AssessmentName {get;set;}

    public Boolean IsBinary {get; set;}
    public int AssessmentRange {get;set;}

    public string UserNombre { get; set; }

    public string NotasEvaluacion {get;set;}

    public string NotasObjetivos {get;set;}

    public ICollection<SectionInfoDto> SectionsInfo  { get; set; }
        = new List<SectionInfoDto>();
  }
}
