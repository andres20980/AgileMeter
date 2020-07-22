using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Entities
{
    public class AssessmentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssessmentId { get; set; }

        [Required]
        [MaxLength(50)]
        public string AssessmentName { get; set; }

       public int AssessmentRange{ get; set; }
  }
}
