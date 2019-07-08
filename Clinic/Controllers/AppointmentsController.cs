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
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AppointmentsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            IList<string> roles = await _userManager.GetRolesAsync(user);
            Doctor doctor = null;
            if (roles.Contains("Administrator"))
            {
                return View( await _context.Appointment.Include(a => a.Doctor).Include(a => a.Patient).ToListAsync());
            }
            else if (roles.Contains("Patient"))
            {
                Patient patient = _context.Patient.Where(p => p.UserId.Equals(user.Id)).Single();
                IQueryable<Appointment> patAppointments = _context.Appointment.Where(a => a.PatientId.Equals(patient.Id));
                return View(patAppointments);
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
            var appointments = _context.Appointment.Include(a => a.Doctor).Include(a => a.Patient);
            appointments.Where(a => a.DoctorId.Equals(doctor.Id));

            return View(await appointments.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        [Authorize(Roles = "Administrator, Assistant")]
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Firstname");
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Firstname");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Assistant")]
        public async Task<IActionResult> Create([Bind("Id,PatientId,DoctorId,Date,Time,PatientName")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //If this statement runs and finds result, then we cannot add the new appointment
                    Appointment a = _context.Appointment.Where(a1 => a1.Date.Equals(appointment.Date) && a1.Time.Equals(appointment.Time) && a1.DoctorId.Equals(appointment.DoctorId)).Single();
                    ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Firstname", appointment.DoctorId);
                    ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Firstname", appointment.PatientId);
                    return View(appointment);
                }
                catch(InvalidOperationException e)
                {
                    //Couldn't find the same appointment date, thus we can create the appointment no problem :)
                }
                
                
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Firstname", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Firstname", appointment.PatientId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        [Authorize(Roles = "Administrator, Assistant")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Firstname", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Firstname", appointment.PatientId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Assistant")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PatientId,DoctorId,Date,Time,PatientName")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Firstname", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Firstname", appointment.PatientId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        [Authorize(Roles = "Administrator, Assistant")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Assistant")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            _context.Appointment.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointment.Any(e => e.Id == id);
        }
    }
}
