using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Models
{
  public class ProyectoDto
  {
    public int Id { get; set; }

    public string Nombre { get; set; }

    public string teamName { get; set; }

    public string officeName { get; set; }

    public string unityName { get; set; }

    public int projectSize { get; set; }

    public DateTime Fecha { get; set; }

    public int numFinishedEvals  { get; set; }

    public int numPendingEvals  { get; set; }
  }
}
