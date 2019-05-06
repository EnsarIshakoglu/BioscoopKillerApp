using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace BioscoopKillerApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserLogic _userLogic = new UserLogic();

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn([Bind("Password, Username")] User user)
        {
            return _userLogic.Login(user) ? RedirectToAction("Privacy", "Home") : RedirectToAction("LogIn");
        }

        [HttpPost]
        public IActionResult CreateAccount([Bind("Password, Username, Email")] User user)
        {
            return View("LogIn");
        }
    }
}