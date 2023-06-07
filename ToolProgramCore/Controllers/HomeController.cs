using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System.Composition;
using System.Diagnostics;
using System.DirectoryServices.Protocols;
using ToolProgramCore.Models;

namespace ToolProgramCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {

            return View();
        }

        
        public IActionResult Privacy()
        {
            return View();
        }

        // Verify that the user is an admin
        public bool VerifyUser()
        {
            List<string> verifyiedUsers = new List<string>{ 
                "TTUSA\\rpalma", 
                "", } ;

            string username = base.User.Identities.ElementAt(0).Name ?? "";

            
            if( ! username.Equals("")) { 

                foreach(string verified in verifyiedUsers)
                {
                    if (username == verified)
                    {
                        return true;
                    }
                }
            }

            return false;
            
        }

        [Authorize]
        public IActionResult AdminLogIn()
        {
            if (VerifyUser())
            {
                return RedirectToAction("AdminLoggedIn");
            }

            return View();
        }

        [Authorize]
        public IActionResult AdminLoggedIn()
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