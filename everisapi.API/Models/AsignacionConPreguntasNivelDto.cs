using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Models
{
    public class AsignacionConPreguntaNivelDto
    {
        public int Id {get; set;}

        public string Nombre { get; set; }

        public string Notas { get; set; }

        public int Peso { get; set; }

        public float Puntuacion { get; set; }

        public int NivelAlcanzado { get; set; }

        public ICollection<PreguntaRespuestaNivelDto> Preguntas { get; set; }
        = new List<PreguntaRespuestaNivelDto>();
  }
}