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
    [Authorize(Roles = "Administrator, InsuranceCompany")]
    public class InsuranceCompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public InsuranceCompaniesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: InsuranceCompanies
        public async Task<IActionResult> Index()
        {
            return View(await _context.InsuranceCompany.ToListAsync());
        }

        // GET: InsuranceCompanies/Details without Route-id
        public async Task<IActionResult> SelfDetails()
        {
            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            IList<string> roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("InsuranceCompany"))
            {
                return NotFound();
            }
            var insCom = _context.InsuranceCompany.Where(i => i.UserId.Equals(user.Id)).Single();
            if (insCom == null)
            {
                return NotFound();
            }

            return View("Details", insCom);
        }

        // GET: InsuranceCompanies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuranceCompany = await _context.InsuranceCompany
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuranceCompany == null)
            {
                return NotFound();
            }

            return View(insuranceCompany);
        }

        // GET: InsuranceCompanies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InsuranceCompanies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Address,Fax,UserId,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] InsuranceCompany insuranceCompany)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insuranceCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(insuranceCompany);
        }

        // GET: InsuranceCompanies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuranceCompany = await _context.InsuranceCompany.FindAsync(id);
            if (insuranceCompany == null)
            {
                return NotFound();
            }
            return View(insuranceCompany);
        }

        // POST: InsuranceCompanies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Address,Fax,UserId,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] InsuranceCompany insuranceCompany)
        {
            if (id != insuranceCompany.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insuranceCompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceCompanyExists(insuranceCompany.Id))
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
            return View(insuranceCompany);
        }

        // GET: InsuranceCompanies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuranceCompany = await _context.InsuranceCompany
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuranceCompany == null)
            {
                return NotFound();
            }

            return View(insuranceCompany);
        }

        // POST: InsuranceCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insuranceCompany = await _context.InsuranceCompany.FindAsync(id);
            _context.InsuranceCompany.Remove(insuranceCompany);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceCompanyExists(int id)
        {
            return _context.InsuranceCompany.Any(e => e.Id == id);
        }
    }
}
