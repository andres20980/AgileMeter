using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Models
{
    public class AsignacionesIdiomas
    {
        public int ID { get; set; }
        public string Pregunta { get; set; }
        public string CodigoIdioma { get; set; }
        public int AsignacionId { get; set; }
    }
}