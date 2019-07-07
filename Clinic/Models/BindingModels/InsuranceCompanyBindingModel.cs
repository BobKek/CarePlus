using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models.BindingModels
{
    public class InsuranceCompanyBindingModel : UserCreationBindingModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Fax { get; set; }
    }
}
