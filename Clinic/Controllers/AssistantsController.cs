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
    [Authorize]
    public class AssistantsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AssistantsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Assistants
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Assistant.Include(a => a.Doctor).Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Assistants/Details without Route-id
        public async Task<IActionResult> SelfDetails()
        {
            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            IList<string> roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Assistant"))
            {
                return NotFound();
            }
            var assistant = _context.Assistant.Where(a => a.UserId.Equals(user.Id)).Single();
            if (assistant == null)
            {
                return NotFound();
            }

            return View("Details", assistant);
        }

        // GET: Assistants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assistant = await _context.Assistant
                .Include(a => a.Doctor)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assistant == null)
            {
                return NotFound();
            }

            return View(assistant);
        }

        // GET: Assistants/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Firstname");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Assistants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Firstname,Lastname,DoctorId,UserId,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] Assistant assistant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assistant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Firstname", assistant.DoctorId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", assistant.UserId);
            return View(assistant);
        }

        // GET: Assistants/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assistant = await _context.Assistant.FindAsync(id);
            if (assistant == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Firstname", assistant.DoctorId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", assistant.UserId);
            return View(assistant);
        }

        // POST: Assistants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Firstname,Lastname,DoctorId,UserName,Email,PasswordHash,PhoneNumber")] Assistant assistant)
        {
            Assistant newAssistant = _context.Assistant.Where(a => a.Id.Equals(id)).Single();
            Task<IdentityUser> user = _userManager.FindByIdAsync(newAssistant.UserId);

            if (user.Result == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();
                    var PasswordHash = hasher.HashPassword(user.Result, assistant.PasswordHash);
                    user.Result.PasswordHash = PasswordHash;
                    user.Result.UserName = assistant.UserName;
                    user.Result.Email = assistant.Email;
                    user.Result.PhoneNumber = assistant.PhoneNumber;
                    await _userManager.UpdateAsync(user.Result);


                    newAssistant.Firstname = assistant.Firstname;
                    newAssistant.Lastname = assistant.Lastname;
                    newAssistant.UserName = assistant.UserName;
                    newAssistant.Email = assistant.Email;
                    newAssistant.PhoneNumber = assistant.PhoneNumber;
                    newAssistant.PasswordHash = assistant.PasswordHash;
                    _context.Update(newAssistant);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssistantExists(assistant.Id))
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
            return View(assistant);
        }

        // GET: Assistants/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assistant = await _context.Assistant
                .Include(a => a.Doctor)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assistant == null)
            {
                return NotFound();
            }

            return View(assistant);
        }

        // POST: Assistants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assistant = await _context.Assistant.FindAsync(id);
            _context.Assistant.Remove(assistant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssistantExists(int id)
        {
            return _context.Assistant.Any(e => e.Id == id);
        }
    }
}
