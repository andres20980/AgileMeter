using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Entities
{
    public class LineaEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LineaId { get; set; }

        [Required]
        [MaxLength(50)]
        public string LineaNombre { get; set; }

        public int UnidadId { get; set; }
        [ForeignKey("UnidadId")]
        public UnidadEntity UnidadEntity { get; set; }
    }    
}