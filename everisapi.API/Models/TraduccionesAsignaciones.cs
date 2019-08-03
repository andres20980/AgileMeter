using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Models
{
    public class TraduccionesAsignaciones
    {
        public int ID { get; set; }
        public string Pregunta { get; set; }
        public string IdiomaId { get; set; }
        public int AsignacionId { get; set; }
    }
}