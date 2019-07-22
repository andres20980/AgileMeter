using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Models
{
    public class RoleDto
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public ICollection<TraduccionesRoles> Traducciones {get;set;} = new List<TraduccionesRoles>();
    }
}
