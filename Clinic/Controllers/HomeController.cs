using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Clinic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Clinic.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _signInManager = _serviceProvider.GetRequiredService<SignInManager<IdentityUser>>();
            _userManager = _serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [AllowAnonymous]
        public IActionResult Welcome()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
