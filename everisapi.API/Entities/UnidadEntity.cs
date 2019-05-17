using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Entities
{
    public class UnidadEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UnidadId { get; set; }

        [Required]
        [MaxLength(50)]
        public string UnidadNombre { get; set; }

        public int OficinaId { get; set; }
        [ForeignKey("OficinaId")] 
        public OficinaEntity OficinaEntity { get; set; }

    }    
}