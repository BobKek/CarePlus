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
using Clinic.Models.BindingModels;

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

        //CreatePeople takes us to the view called Add to create any type of User
        [HttpGet]
        public IActionResult CreatePeople()
        {
            var DoctorsList = _context.Doctor.ToListAsync();
            SelectList selectListItems = new SelectList(DoctorsList.Result, "Id", "Firstname");
            ViewBag.DoctorsList = selectListItems;
            ViewBag.DoctorsSwitch = "active in";
            ViewBag.AssistantsSwitch = "";
            ViewBag.CompaniesSwitch = "";
            return View("Add");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDoctor(DoctorBindingModel doctorBindingModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser doctorUser = CreateIdentityUser(doctorBindingModel);
                if (doctorUser != null)
                {
                    var result = _userManager.AddToRoleAsync(doctorUser, "Doctor");
                    result.Wait();
                    Doctor doctor = new Doctor();

                    //Adding attributes from IdentityUser
                    doctor.Email = doctorUser.Email;
                    doctor.UserName = doctorUser.UserName;
                    doctor.PasswordHash = doctorUser.PasswordHash;
                    doctor.EmailConfirmed = doctorUser.EmailConfirmed;
                    doctor.PhoneNumberConfirmed = doctorUser.PhoneNumberConfirmed;
                    doctor.TwoFactorEnabled = false;
                    doctor.LockoutEnabled = true;
                    doctor.AccessFailedCount = doctorUser.AccessFailedCount;
                    doctor.NormalizedEmail = doctorUser.NormalizedEmail;
                    doctor.NormalizedUserName = doctorUser.NormalizedUserName;
                    doctor.PhoneNumber = doctorUser.PhoneNumber;

                    //Adding Doctor attributes
                    doctor.Firstname = doctorBindingModel.Firstname;
                    doctor.Lastname = doctorBindingModel.Lastname;
                    doctor.Specialty = doctorBindingModel.Specialty;
                    doctor.Gender = doctorBindingModel.Gender;
                    doctor.Address = doctorBindingModel.Address;
                    doctor.User = doctorUser;
                    _context.Add(doctor);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Doctors");
            }
            var DoctorsList = _context.Doctor.ToListAsync();
            SelectList selectListItems = new SelectList(DoctorsList.Result, "Id", "Firstname");
            ViewBag.DoctorsList = selectListItems;
            ViewBag.DoctorsSwitch = "active in";
            ViewBag.AssistantsSwitch = "";
            ViewBag.CompaniesSwitch = "";
            return View("Add");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAssistant(AssistantBindingModel assistantBindingModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser assistantUser = CreateIdentityUser(assistantBindingModel);
                if (assistantUser != null)
                {
                    var result = _userManager.AddToRoleAsync(assistantUser, "Assistant");
                    result.Wait();
                    Assistant assistant = new Assistant();

                    //Adding attributes from IdentityUser
                    assistant.Email = assistantUser.Email;
                    assistant.UserName = assistantUser.UserName;
                    assistant.PasswordHash = assistantUser.PasswordHash;
                    assistant.EmailConfirmed = assistantUser.EmailConfirmed;
                    assistant.PhoneNumberConfirmed = assistantUser.PhoneNumberConfirmed;
                    assistant.TwoFactorEnabled = false;
                    assistant.LockoutEnabled = true;
                    assistant.AccessFailedCount = assistantUser.AccessFailedCount;
                    assistant.NormalizedEmail = assistantUser.NormalizedEmail;
                    assistant.NormalizedUserName = assistantUser.NormalizedUserName;
                    assistant.PhoneNumber = assistantUser.PhoneNumber;

                    //Adding Assistant attributes
                    assistant.Firstname = assistantBindingModel.Firstname;
                    assistant.Lastname = assistantBindingModel.Lastname;
                    assistant.DoctorId = assistantBindingModel.DoctorId;
                    assistant.User = assistantUser;
                    _context.Add(assistant);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Assistants");
            }
                
            var DoctorsList = _context.Doctor.ToListAsync();
            SelectList selectListItems = new SelectList(DoctorsList.Result, "Id", "Firstname");
            ViewBag.DoctorsList = selectListItems;
            ViewBag.DoctorsSwitch = "";
            ViewBag.AssistantsSwitch = "active in";
            ViewBag.CompaniesSwitch = "";
            return View("Add");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInsuranceCompany(InsuranceCompanyBindingModel insuranceCompanyBindingModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser insuranceUser = CreateIdentityUser(insuranceCompanyBindingModel);
                if (insuranceUser != null)
                {
                    var result = _userManager.AddToRoleAsync(insuranceUser, "InsuranceCompany");
                    result.Wait();
                    InsuranceCompany insurance = new InsuranceCompany();

                    //Adding attributes from IdentityUser
                    insurance.Email = insuranceUser.Email;
                    insurance.UserName = insuranceUser.UserName;
                    insurance.PasswordHash = insuranceUser.PasswordHash;
                    insurance.EmailConfirmed = insuranceUser.EmailConfirmed;
                    insurance.PhoneNumberConfirmed = insuranceUser.PhoneNumberConfirmed;
                    insurance.TwoFactorEnabled = false;
                    insurance.LockoutEnabled = true;
                    insurance.AccessFailedCount = insuranceUser.AccessFailedCount;
                    insurance.NormalizedEmail = insuranceUser.NormalizedEmail;
                    insurance.NormalizedUserName = insuranceUser.NormalizedUserName;
                    insurance.PhoneNumber = insuranceUser.PhoneNumber;

                    //Adding Assistant attributes
                    insurance.Name = insuranceCompanyBindingModel.Name;
                    insurance.Address = insuranceCompanyBindingModel.Address;
                    insurance.Fax = insuranceCompanyBindingModel.Fax;
                    insurance.User = insuranceUser;
                    _context.Add(insurance);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "InsuranceCompanies");
            }

            var DoctorsList = _context.Doctor.ToListAsync();
            SelectList selectListItems = new SelectList(DoctorsList.Result, "Id", "Firstname");
            ViewBag.DoctorsList = selectListItems;
            ViewBag.DoctorsSwitch = "";
            ViewBag.AssistantsSwitch = "";
            ViewBag.CompaniesSwitch = "active in";
            return View("Add");
        }

        public async Task<IActionResult> Inbox()
        {
            return View(await _context.Message.ToListAsync());
        }

        // GET: Admins/Details without Route-id to display self details
        public async Task<IActionResult> SelfDetails()
        {
            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            IList<string> roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Administrator"))
            {
                return NotFound();
            }
            var admin = _context.Admin.Where(a => a.UserId.Equals(user.Id)).Single();
            if (admin == null)
            {
                return NotFound();
            }

            return View("Details", admin);
        }

        private bool AdminExists(int id)
        {
            return _context.Admin.Any(e => e.Id == id);
        }

        public IdentityUser CreateIdentityUser(UserCreationBindingModel userCreationBindingModel)
        {
            IdentityUser user = new IdentityUser();
            user.Email = userCreationBindingModel.Email;
            user.UserName = userCreationBindingModel.UserName;
            var Password = userCreationBindingModel.Password;
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

            var createUser = _userManager.CreateAsync(user);
            createUser.Wait();
            if(createUser.Result != null)
            {
                return user;
            }
            return null;
        }
    }
}