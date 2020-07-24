using everisapi.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Models
{
    public class AssessmentDto
    {
        public int AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public int AssessmentRange{ get; set; }
    }
}
