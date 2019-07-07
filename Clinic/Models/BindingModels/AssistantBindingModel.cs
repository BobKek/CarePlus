using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models.BindingModels
{
    public class AssistantBindingModel : UserCreationBindingModel
    {
        [Required]
        [MaxLength(256, ErrorMessage = "Max Length is 256 Chars")]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(256, ErrorMessage = "Max Length is 256 Chars")]
        public string Lastname { get; set; }

        [Required]
        public int DoctorId { get; set; }
    }
}
