using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Models
{
    public class SectionConAsignacionesDto
    {
    public int EvaluacionId { get; set; }

    public int SectionId { get; set; }

    public string Notas { get; set; }

     public string Nombre { get; set; }

    public float Puntuacion { get; set; }

    public int NivelAlcanzado { get; set; }

    public int Peso { get; set; }

    public int PesoNivel1 { get; set; }

    public int PesoNivel2 { get; set; }

    public int PesoNivel3 { get; set; }

    public ICollection<AsignacionConPreguntaNivelDto> Asignaciones { get; set; }
        = new List<AsignacionConPreguntaNivelDto>();
    }
}