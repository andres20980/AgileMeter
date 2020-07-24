using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Models
{
    public class EvaluacionInfoWithProgressDto
    {
    public int Id { get; set; }

    public string Nombre { get; set; }

    public DateTime Fecha { get; set; }

    public int AssessmentId {get;set;}

    public string AssessmentName {get;set;}

    public int AssessmentRange {get; set;}

    public string UserNombre { get; set; }

    public float progress { get; set; }

    public int numQuestions { get; set; }

    public string oficina { get; set; }
  }
}
