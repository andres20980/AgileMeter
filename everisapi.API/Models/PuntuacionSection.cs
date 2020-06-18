using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Models
{
    public class PuntuacionSection {
        public int Id { get; set; }

        public int SectionId { get; set; }

        public int EvaluacionId { get; set; }

        public float Puntuacion {get; set;}

        public int NivelAlcanzado {get; set;}

    }
}