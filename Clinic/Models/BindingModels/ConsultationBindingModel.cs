using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Models.BindingModels
{
    public class ConsultationBindingModel
    {
        public int Id { get; set; }
        [EmailAddress]
        public string PatientEmail { get; set; }
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
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }
}