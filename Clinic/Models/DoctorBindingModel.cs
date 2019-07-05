using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models
{
    [BindProperties]
    public class DoctorBindingModel : PageModel
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
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int UserId { get; set; }
    }
}
