using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Models
{
    public class UsersDto
    {
        public string Nombre { get; set; }

        public string Password { get; set; }

        public ICollection<ProyectoDto> ProyectosDeUsuario { get; set; }
        = new List<ProyectoDto>();
        public string NombreCompleto { get; set; }

        public int RoleId { get; set; }

        public bool Activo { get; set; }

        public string IdiomaFavorito { get; set;}
    }
}
