using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        public int PatientId { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        public int DoctorId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        public string PatientName { get; set; }
    }
}
