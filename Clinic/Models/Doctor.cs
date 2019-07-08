using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models
{
    public class Doctor : IdentityUser<int>
    {
        [Required]
        [MaxLength(256, ErrorMessage = "Max Length is 256 Chars")]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(256, ErrorMessage = "Max Length is 256 Chars")]
        public string Lastname { get; set; }

        [MaxLength(6, ErrorMessage = "Max length is 6")]
        public string Gender { get; set; }

        public string Specialty { get; set; }

        [MaxLength(256, ErrorMessage = "Max Length is 256 Chars")]
        public string Address { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

        public string UserId { get; set; }
    }
}
