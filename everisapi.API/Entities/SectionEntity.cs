using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Entities
{
    public class SectionEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string Nombre { get; set; }

        [Required]
        public ICollection<AsignacionEntity> Asignaciones { get; set; }
        = new List<AsignacionEntity>();


        [Required]
        public int AssessmentId { get; set; }
        [ForeignKey("AssessmentId")]
        public AssessmentEntity Assessment { get; set; }

        [Required]
        [MaxLength(50)]
        public int Peso { get; set; }

        public int PesoNivel1 { get; set; }
        public int PesoNivel2 { get; set; }
        public int PesoNivel3 { get; set; }
    }
}
