using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Models
{
    public class UsersWithRolesDto
    {
        public string Nombre { get; set; }

        //public string Password { get; set; }

        public RoleDto Role { get; set; }
        
    }
}
