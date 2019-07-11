using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Entities
{
    public class OficinaEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OficinaId { get; set; }

        [Required]
        [MaxLength(50)]
        public string OficinaNombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string OficinaNombreEs { get; set; }

        [Required]
        [MaxLength(50)]
        public string OficinaNombreEn { get; set; }
    }
}
