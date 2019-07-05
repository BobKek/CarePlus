using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;

namespace Clinic.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Clinic.Models.Patient> Patient { get; set; }
        public DbSet<Clinic.Models.Admin> Admin { get; set; }
        public DbSet<Clinic.Models.Doctor> Doctor { get; set; }
        public DbSet<Clinic.Models.Appointment> Appointment { get; set; }
        public DbSet<Clinic.Models.Assistant> Assistant { get; set; }
        public DbSet<Clinic.Models.InsuranceCompany> InsuranceCompany { get; set; }
        public DbSet<Clinic.Models.Consultation> Consultation { get; set; }
    }
}
