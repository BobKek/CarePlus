using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinic.Data;
using Clinic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Clinic.Controllers
{
    [Authorize(Roles = "Administrator, Doctor, Patient, Assistant")]
    public class ConsultationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ConsultationsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Consultations
        public async Task<IActionResult> Index()
        {
            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            IList<string> roles = await _userManager.GetRolesAsync(user);
            Doctor doctor = null;
            if (roles.Contains("Administrator"))
            {
                return View(_context.Consultation.Include(c => c.Doctor).Include(c => c.Patient));
            }
            else if (roles.Contains("Patient"))
            {
                Patient patient = _context.Patient.Where(p => p.UserId.Equals(user.Id)).Single();
                IQueryable<Consultation> patConsultations = _context.Consultation.Where(c => c.PatientId.Equals(patient.Id));
                return View(patConsultations);
            }
            else if (roles.Contains("Assistant"))
            {
                Assistant assistant = _context.Assistant.Where(a => a.UserId.Equals(user.Id)).Single();
                doctor = _context.Doctor.Where(d => d.Id.Equals(assistant.DoctorId)).Single();
            }
            else
            {
                doctor = _context.Doctor.Where(d => d.UserId.Equals(user.Id)).Single();
            }

            var consultations = _context.Consultation.Include(c => c.Doctor).Include(c => c.Patient).Where(c => c.Doctor.Id.Equals(doctor.Id));
            return View(consultations);
        }

        // GET: Consultations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultation
                .Include(c => c.Doctor)
                .Include(c => c.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consultation == null)
            {
                return NotFound();
            }

            return View(consultation);
        }

        // GET: Consultations/Create
        [Authorize(Roles = "Administrator, Doctor")]
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Firstname");
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Firstname");
            return View();
        }

        // POST: Consultations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Doctor")]
        public async Task<IActionResult> Create([Bind("Id,Title,Type,Date,Symptoms,Diagnosis,Temp,BloodPressure,Cost,Treatment,InsuranceConfirmation,PatientId,DoctorId")] Consultation consultation)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
                IList<string> roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Doctor") && !roles.Contains("Administrator"))
                {
                    Doctor doctor = _context.Doctor.Where(d => d.UserId.Equals(user.Id)).Single();
                    Patient patient = _context.Patient.Where(p => p.Id.Equals(consultation.PatientId)).Single();
                    consultation.Doctor = doctor;
                    consultation.Patient = patient;
                    consultation.DoctorId = doctor.Id;
                    consultation.PatientId = patient.Id;
                }
                _context.Add(consultation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Firstname", consultation.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Firstname", consultation.PatientId);
            return View(consultation);
        }

        // GET: Consultations/Edit/5
        [Authorize(Roles = "Administrator, Doctor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultation.FindAsync(id);
            if (consultation == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Firstname", consultation.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Firstname", consultation.PatientId);
            return View(consultation);
        }

        // POST: Consultations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Doctor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Type,Date,Symptoms,Diagnosis,Temp,BloodPressure,Cost,Treatment,InsuranceConfirmation,PatientId,DoctorId")] Consultation consultation)
        {
            if (id != consultation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consultation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultationExists(consultation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Firstname", consultation.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Firstname", consultation.PatientId);
            return View(consultation);
        }

        // GET: Consultations/Delete/5
        [Authorize(Roles = "Administrator, Doctor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultation
                .Include(c => c.Doctor)
                .Include(c => c.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consultation == null)
            {
                return NotFound();
            }

            return View(consultation);
        }

        // POST: Consultations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Doctor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consultation = await _context.Consultation.FindAsync(id);
            _context.Consultation.Remove(consultation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultationExists(int id)
        {
            return _context.Consultation.Any(e => e.Id == id);
        }
    }
}
