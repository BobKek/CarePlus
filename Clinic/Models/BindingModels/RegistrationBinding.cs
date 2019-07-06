using Clinic.Areas.Identity.Pages.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models.BindingModels
{
    public class RegistrationBinding
    {
        public DoctorBindingModel DoctorModel { get; set; }
        public string MyString { get; set; }
    }
}
