using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Entities
{
    public class TraduccionesPreguntasEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(500)]
        public string Traduccion { get; set; }

        public int IdiomaId { get; set; }
        [ForeignKey("ID")]
        public IdiomasEntity IdiomasEntity { get; set; }

        [Required]
        public int PreguntaId { get; set; }
        [ForeignKey("ID")]
        public PreguntaEntity PreguntaEntity { get; set; }

    }
}
