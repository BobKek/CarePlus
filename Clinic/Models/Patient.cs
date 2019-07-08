using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models
{
    public class Patient : IdentityUser<int>
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string BloodType { get; set; }

        [ForeignKey("InsuranceId")]
        public InsuranceCompany InsuranceCompany { get; set; }

        public int InsuranceId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

        public string UserId { get; set; }
    }
}
