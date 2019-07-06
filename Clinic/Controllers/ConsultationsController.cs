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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Clinic.Models.BindingModels;
using System.Collections;

namespace Clinic.Controllers
{
    [Authorize(Roles = "Administrator, Doctor, Assistant, Patient")]
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
            if (roles.Contains("Administrator"))
            {
                return View(await _context.Consultation.ToListAsync());
            }
            else if (roles.Contains("Patient"))
            {
                Patient patient = _context.Patient.Where(p => p.Id.Equals(user.Id)).Single();
                IQueryable<Consultation> patConsultations = _context.Consultation.Where(c => c.PatientId.Equals(patient.Id));
                return View(patConsultations);
            }
            Doctor doctor = _context.Doctor.Where(d => d.UserId.Equals(user.Id)).Single();

            var consultations = _context.Consultation.Where(c => c.DoctorId.Equals(doctor.Id));
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consultation == null)
            {
                return NotFound();
            }

            return View(consultation);
        }

        // GET: Consultations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Consultations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConsultationBindingModel model)
        {
            if (ModelState.IsValid)
            {
                Consultation consultation = new Consultation();
                consultation.BloodPressure = model.BloodPressure;
                consultation.Cost = model.Cost;
                consultation.Date = model.Date;
                consultation.Diagnosis = model.Diagnosis;
                consultation.InsuranceConfirmation = model.InsuranceConfirmation;
                consultation.Symptoms = model.Symptoms;
                consultation.Temp = model.Temp;
                consultation.Title = model.Title;
                consultation.Treatment = model.Treatment;
                consultation.Type = model.Type;
                
                Patient patient = _context.Patient.Where(p => p.Email.Equals(model.PatientEmail)).Single();
                if (patient != null)
                {
                    consultation.PatientId = patient.Id;
                    IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
                    Doctor doctor = _context.Doctor.Where(d => d.UserId.Equals(user.Id)).Single();

                    consultation.DoctorId = doctor.Id;
                    _context.Add(consultation);
                    await _context.SaveChangesAsync();
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Consultations/Edit/5
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
            return View(consultation);
        }

        // POST: Consultations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            return View(consultation);
        }

        // GET: Consultations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultation
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
