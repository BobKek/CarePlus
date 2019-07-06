using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models.BindingModels
{
    [BindProperties]
    public class UserCreationBindingModel
    {
        [Required]
        [MaxLength(2, ErrorMessage = "Max Length is 256 Chars")]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(256, ErrorMessage = "Max Length is 256 Chars")]
        public string Lastname { get; set; }

        [MaxLength(256, ErrorMessage = "Max Length is 256 Chars")]
        public string Address { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public int UserId { get; set; }
    }
}
