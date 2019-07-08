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
using Clinic.Models.BindingModels;
using Microsoft.AspNetCore.Authorization;

namespace Clinic.Controllers
{
    [Authorize]
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PatientsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            IList<string> roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Administrator"))
            {
                return View(await _context.Patient.ToListAsync());
            }
            if (roles.Contains("Doctor"))
            {
                Doctor doctor = _context.Doctor.Where(d => d.UserId.Equals(user.Id)).Single();
                var patients = GetPatientsListForDoctor(doctor.Id);
                return View(patients);
            }
            if (roles.Contains("Assistant"))
            {
                Assistant assistant = _context.Assistant.Where(a => a.UserId.Equals(user.Id)).Single();
                GetPatientsListForDoctor(assistant.DoctorId);

            }

            //return View(await _context.Patient.ToListAsync());

            var applicationDbContext = _context.Patient.Include(p => p.InsuranceCompany).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        private List<Patient> GetPatientsListForDoctor(int doctorId)
        {
            IQueryable<Consultation> consultations = _context.Consultation.Where(c => c.DoctorId.Equals(doctorId));
            List<Patient> patients = new List<Patient>();
            foreach (Consultation c in consultations)
            {
                patients.Add(_context.Patient.Where(p => p.Id.Equals(c.PatientId)).Single());
            }
            return patients;
        }

        // GET: Patients/Details without Route-id
        public async Task<IActionResult> SelfDetails()
        {
            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            IList<string> roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Patient"))
            {
                return NotFound();
            }
            var patient = _context.Patient.Where(p => p.UserId.Equals(user.Id)).Single();
            if (patient == null)
            {
                return NotFound();
            }

            return View("Details", patient);
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
                .Include(p => p.InsuranceCompany)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        [Authorize(Roles = "Administrator, Assistant")]
        public IActionResult Create()
        {
            ViewData["InsuranceId"] = new SelectList(_context.InsuranceCompany, "Id", "Name");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Assistant")]
        public async Task<IActionResult> Create(PatientBindingModel patientBindingModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser patientUser = CreateIdentityUser(patientBindingModel);
                if (patientUser != null)
                {
                    var result = _userManager.AddToRoleAsync(patientUser, "Patient");
                    result.Wait();
                    Patient patient = new Patient();

                    //Adding attributes from IdentityUser
                    patient.Email = patientUser.Email;
                    patient.UserName = patientUser.UserName;
                    patient.PasswordHash = patientUser.PasswordHash;
                    patient.EmailConfirmed = patientUser.EmailConfirmed;
                    patient.PhoneNumberConfirmed = patientUser.PhoneNumberConfirmed;
                    patient.TwoFactorEnabled = false;
                    patient.LockoutEnabled = true;
                    patient.AccessFailedCount = patientUser.AccessFailedCount;
                    patient.NormalizedEmail = patientUser.NormalizedEmail;
                    patient.NormalizedUserName = patientUser.NormalizedUserName;
                    patient.PhoneNumber = patientUser.PhoneNumber;

                    //Adding Patient attributes
                    patient.Firstname = patientBindingModel.Firstname;
                    patient.Lastname = patientBindingModel.Lastname;
                    patient.BloodType = patientBindingModel.BloodType;
                    patient.Address = patientBindingModel.Address;

                    InsuranceCompany insComp = _context.InsuranceCompany.Where(i => i.Id.Equals(patientBindingModel.InsuranceId)).Single();
                    patient.InsuranceCompany = insComp;

                    patient.User = patientUser;

                    _context.Add(patient);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Patients");
            }
            ViewData["InsuranceId"] = new SelectList(_context.InsuranceCompany, "Id", "Name", patientBindingModel.InsuranceId);
            return View();
        }

        // GET: Patients/Edit/5
        [Authorize(Roles = "Administrator, Assistant")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            ViewData["InsuranceId"] = new SelectList(_context.InsuranceCompany, "Id", "Name", patient.InsuranceId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Firstname", patient.UserId);
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Assistant")]
        public async Task<IActionResult> Edit(int id, [Bind("Firstname,Lastname,Address,BloodType,InsuranceId,UserName,Email,PasswordHash,PhoneNumber")] Patient patient)
        {
            Patient newPatient = _context.Patient.Where(p1 => p1.Id.Equals(id)).Single();
            Task<IdentityUser> user = _userManager.FindByIdAsync(newPatient.UserId);
            user.Wait();
            if (user.Result == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();
                    var PasswordHash = hasher.HashPassword(user.Result, patient.PasswordHash);
                    user.Result.PasswordHash = PasswordHash;
                    user.Result.UserName = patient.UserName;
                    user.Result.Email = patient.Email;
                    user.Result.PhoneNumber = patient.PhoneNumber;
                    await _userManager.UpdateAsync(user.Result);

                    newPatient.Firstname = patient.Firstname;
                    newPatient.Lastname = patient.Lastname;
                    newPatient.Address = patient.Address;
                    newPatient.BloodType = patient.BloodType;
                    newPatient.InsuranceId = patient.InsuranceId;
                    newPatient.UserName = patient.UserName;
                    newPatient.Email = patient.Email;
                    newPatient.PhoneNumber = patient.PhoneNumber;
                    newPatient.PasswordHash = patient.PasswordHash;
                    _context.Update(newPatient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            ViewData["InsuranceId"] = new SelectList(_context.InsuranceCompany, "Id", "Name", patient.InsuranceId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Firstname", patient.UserId);
            return View(patient);
        }

        // GET: Patients/Delete/5
        [Authorize(Roles = "Administrator, Assistant")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
                .Include(p => p.InsuranceCompany)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Assistant")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patient.FindAsync(id);
            _context.Patient.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patient.Any(e => e.Id == id);
        }

        public IdentityUser CreateIdentityUser(UserCreationBindingModel userCreationBindingModel)
        {
            IdentityUser user = new IdentityUser();
            user.Email = userCreationBindingModel.Email;
            user.UserName = userCreationBindingModel.UserName;
            var Password = "Admin123#";
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;
            user.TwoFactorEnabled = false;
            user.LockoutEnabled = true;
            user.AccessFailedCount = 0;
            user.NormalizedEmail = userCreationBindingModel.Email.Normalize();
            user.NormalizedUserName = userCreationBindingModel.UserName.Normalize();
            user.PhoneNumber = userCreationBindingModel.PhoneNumber;

            PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();
            var PasswordHash = hasher.HashPassword(user, Password);
            user.PasswordHash = PasswordHash;

            var createDoctor = _userManager.CreateAsync(user);
            createDoctor.Wait();
            if (createDoctor.Result != null)
            {
                return user;
            }
            return null;
        }
    }
}
