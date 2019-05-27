using everisapi.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Models
{
  public class ProyectoWithEvaluacionesDto
  {
    public int Id { get; set;}

    public string Nombre { get; set;}

    public DateTime Fecha { get; set;}

    public Oficina OficinaEntity { get; set;}
    public Unidad UnidadEntity { get; set;}
    public Linea LineaEntity { get; set;}
    public int projectSize { get; set;}
    public string UserNombre{get;set;}
    public bool TestProject{get;set;}

    public ICollection<EvaluacionDto> Evaluaciones { get; set; }
    = new List<EvaluacionDto>();
  }
}
