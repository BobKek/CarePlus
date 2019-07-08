using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models
{
    public class InsuranceCompany : IdentityUser<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public string Fax { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

        public string UserId { get; set; }

    }
}
