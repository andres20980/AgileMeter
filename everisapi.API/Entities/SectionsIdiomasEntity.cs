using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Entities
{
    public class SectionsIdiomasEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(120)]
        public string Sections { get; set; }
        
        public string CodigoIdioma { get; set; }
        [ForeignKey("Codigo")]
        public IdiomasEntity IdiomasEntity { get; set; }

        [Required]
        public int SectionsId { get; set; }
        [ForeignKey("ID")]
        public SectionEntity SectionEntity { get; set; }
    }
}
