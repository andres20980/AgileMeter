using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Entities
{
    public class PreguntasIdiomasEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(500)]
        public string Pregunta { get; set; }


        public string CodigoIdioma { get; set; }
        [ForeignKey("Codigo")]
        public IdiomasEntity IdiomasEntity { get; set; }

        [Required]
        public int PreguntaId { get; set; }
        [ForeignKey("ID")]
        public PreguntaEntity PreguntaEntity { get; set; }

    }
}
