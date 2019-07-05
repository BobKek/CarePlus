using Clinic.Areas.Identity.Pages.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models
{
    public class RegistrationBinding
    {
        public DoctorBindingModel DoctorModel { get; set; } = new DoctorBindingModel();
    }
}
