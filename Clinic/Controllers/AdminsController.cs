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
    [Authorize(Roles = "Administrator")]
    public class AdminsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admins
        public async Task<IActionResult> Index()
        {
            return View(await _context.Admin.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePeople(DoctorBindingModel DoctorModel)
        {
            RegistrationBinding BindingModel = new RegistrationBinding();
            BindingModel.DoctorModel = DoctorModel;
            Doctor doctor = new Doctor();
            doctor.Email = BindingModel.DoctorModel.Email;
            doctor.Firstname = BindingModel.DoctorModel.Firstname;
            doctor.Lastname = BindingModel.DoctorModel.Lastname;
            doctor.Gender = BindingModel.DoctorModel.Gender;
            doctor.Specialty = BindingModel.DoctorModel.Specialty;
            doctor.Address = BindingModel.DoctorModel.Address;
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
            return View("Add", BindingModel);
        }

        public IActionResult CreatePeople()
        {
            return View("Add", new RegistrationBinding());
        }

        // GET: Admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admin
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admin.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Firstname,Lastname,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] Admin admin)
        {
            if (id != admin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.Id))
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
            return View(admin);
        }

        public IActionResult CreateDoctor()
        {
            return View();
        }

        //public async Task<IActionResult> CreateDoctor(DoctorBindingModel bindingModel)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var admin = await _context.Admin.FindAsync(id);
        //    if (admin == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(admin);
        //}

        private bool AdminExists(int id)
        {
            return _context.Admin.Any(e => e.Id == id);
        }
    }
}