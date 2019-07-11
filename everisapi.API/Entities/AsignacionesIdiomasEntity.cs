using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Entities
{
    public class AsignacionesIdiomasEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Asignacion { get; set; }             
        
        public string CodigoIdioma { get; set; }
        [ForeignKey("Codigo")]
        public IdiomasEntity IdiomasEntity { get; set; }

        [Required]        
        public int AsignacionesId { get; set; }
        [ForeignKey("ID")]
        public AsignacionEntity AsignacionEntity {get;set;}

    }    
}
