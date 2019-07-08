using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models.BindingModels
{
    public class PatientBindingModel : UserCreationBindingModel
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string BloodType { get; set; }

        public int InsuranceId { get; set; }
    }
}
