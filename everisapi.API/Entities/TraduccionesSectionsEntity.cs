using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Entities
{
    public class TraduccionesSectionsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(120)]
        public string Traduccion { get; set; }

        public int IdiomaId { get; set; }
        [ForeignKey("IdiomaId")]
        public IdiomasEntity IdiomasEntity { get; set; }

        [Required]
        public int SectionsId { get; set; }
        [ForeignKey("SectionsId")]
        public SectionEntity SectionEntity { get; set; }
    }
}
