using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models
{
    public class Assistant : IdentityUser<int>
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        public int DoctorId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

        public string UserId { get; set; }

    }
}
