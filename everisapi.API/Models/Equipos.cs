using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using everisapi.API.Entities;

namespace everisapi.API.Models
{
    public class Equipos
    {   
        public int Id { get; set; } 
        public string Nombre { get; set; }        
        public Linea LineaEntity { get; set; } 
        public Oficina OficinaEntity { get; set; }
        public Unidad UnidadEntity { get; set; } 
        public int ProjectSize { get; set; }       
        public string UserNombre {get;set;}        
    }  
}