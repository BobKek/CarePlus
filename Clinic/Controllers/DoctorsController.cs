using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinic.Data;
using Clinic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Clinic.Controllers
{
    [Authorize(Roles = "Doctor, Administrator, Patient")]
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DoctorsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Doctors
        [Authorize(Roles = "Administrator, Patient")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Doctor.ToListAsync());
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Patient")]
        public IActionResult SearchDoctor(string Keyword)
        {
            var foundDoctors = _context.Doctor.Where(d => d.Specialty.Contains(Keyword));
            var foundDoctorsList = foundDoctors.ToList();
            if (!foundDoctors.Any())
            {
                foundDoctors = _context.Doctor.Where(d => d.Email.Contains(Keyword));
                foundDoctorsList.AddRange(foundDoctors.ToList());
            }
            if (!foundDoctors.Any())
            {
                foundDoctors = _context.Doctor.Where(d => d.Firstname.Contains(Keyword));
                foundDoctorsList.AddRange(foundDoctors.ToList());
            }
            if (!foundDoctors.Any())
            {
                foundDoctors = _context.Doctor.Where(d => d.Lastname.Contains(Keyword));
                foundDoctorsList.AddRange(foundDoctors.ToList());
            }
            return View("Index", foundDoctorsList);
        }

        public IActionResult Consult()
        {
            return RedirectToAction("Create", "Consultations");
        }

        // GET: Doctors/Details without Route-id
        public async Task<IActionResult> SelfDetails()
        {
            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            IList<string> roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Doctor"))
            {
                return NotFound();
            }
            var doctor = _context.Doctor.Where(d => d.UserId.Equals(user.Id)).Single();
            if (doctor == null)
            {
                return NotFound();
            }

            return View("Details", doctor);
        }

        // GET: Doctors/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Firstname,Lastname,Gender,Specialty,Address,UserName,Email,PhoneNumber")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                IdentityUser doctorUser = new IdentityUser();
                doctorUser.Email = doctor.Email;
                doctorUser.UserName = doctor.UserName;
                var Password = "Admin123#";
                doctorUser.EmailConfirmed = true;
                doctorUser.PhoneNumberConfirmed = true;
                doctorUser.TwoFactorEnabled = false;
                doctorUser.LockoutEnabled = true;
                doctorUser.AccessFailedCount = 0;
                doctorUser.NormalizedEmail = doctor.Email.Normalize();
                doctorUser.NormalizedUserName = doctor.UserName.Normalize();
                doctorUser.PhoneNumber = doctor.PhoneNumber;

                PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();
                var PasswordHash = hasher.HashPassword(doctorUser, Password);
                doctorUser.PasswordHash = PasswordHash;

                var createDoctor = _userManager.CreateAsync(doctorUser);
                createDoctor.Wait();
                if (createDoctor.Result.Succeeded)
                {
                    var result = _userManager.AddToRoleAsync(doctorUser, "Doctor");
                    result.Wait();
                    doctor.UserId = doctorUser.Id;
                    _context.Add(doctor);
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Firstname,Lastname,Gender,Specialty,Address,UserName,Email,PasswordHash,PhoneNumber")] Doctor doctor)
        {
            Doctor newDoctor = _context.Doctor.Where(d => d.Id.Equals(id)).Single();
            Task<IdentityUser> user = _userManager.FindByIdAsync(newDoctor.UserId);

            if (user.Result == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();
                    var PasswordHash = hasher.HashPassword(user.Result, doctor.PasswordHash);
                    user.Result.PasswordHash = PasswordHash;
                    user.Result.UserName = doctor.UserName;
                    user.Result.Email = doctor.Email;
                    user.Result.PhoneNumber = doctor.PhoneNumber;
                    await _userManager.UpdateAsync(user.Result);


                    newDoctor.Firstname = doctor.Firstname;
                    newDoctor.Lastname = doctor.Lastname;
                    newDoctor.Gender = doctor.Gender;
                    newDoctor.Specialty = doctor.Specialty;
                    newDoctor.Address = doctor.Address;
                    newDoctor.UserName = doctor.UserName;
                    newDoctor.Email = doctor.Email;
                    newDoctor.PhoneNumber = doctor.PhoneNumber;
                    newDoctor.PasswordHash = doctor.PasswordHash;
                    _context.Update(newDoctor);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
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

            ViewBag.Doctors = new SelectList(_context.Doctor, "Id", "Firstname");
            return View(newDoctor);
        }

        // GET: Doctors/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctor.FindAsync(id);
            _context.Doctor.Remove(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctor.Any(e => e.Id == id);
        }
    }
}
