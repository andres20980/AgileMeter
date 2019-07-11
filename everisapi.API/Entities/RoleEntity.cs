using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Entities
{
    public class RoleEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleES { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleEN { get; set; }
    }
}
