using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Models
{
    public class TraduccionesRoles
    {
        public int ID { get; set; }
        public string Traduccion { get; set; }
        public string IdiomaId { get; set; }
        public int AsignacionId { get; set; }
    }
}