using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Models
{
    public class PreguntaRespuestaNivelDto
    {
        public int Id { get; set; }

        public int Estado { get; set; }

        public string Pregunta { get; set; }

        public string Correcta { get; set; }

        public string Notas { get; set; }

        public string NotasAdmin { get; set; }

        public float Peso { get; set; }

        public int Nivel { get; set; }

        public bool EsHabilitante { get; set; }
    }
}