using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Entities
{
    public class TraduccionesRolesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Traduccion { get; set; }

        public int IdiomaId { get; set; }
        [ForeignKey("IdiomaId")]
        public IdiomasEntity IdiomasEntity { get; set; }

        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public RoleEntity RoleEntity { get; set; }


    }
}
