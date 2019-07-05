using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models
{
    public class Consultation
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public string Temp { get; set; }
        public string BloodPressure { get; set; }
        public string Cost { get; set; }
        public string Treatment { get; set; }
        public string InsuranceConfirmation { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }

    }
}
