using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Logic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        public IActionResult LogIn([Bind("Password, Email")] User user)
        {
            if (_userLogic.Login(user))
            {
                InitUser(user);
                return RedirectToAction("Index", "Movie");
            }
            else
            {
                TempData["alertMessage"] = "Incorrect username or password, please try again!";
                return View("LogIn");
            }
        }

        public IActionResult LogOut()
        {
            RemoveCookies();

            return RedirectToAction("Index", "Movie");
        }

        [HttpPost]
        public IActionResult CreateAccount([Bind("Password, Name, SurName, Email")] User user)
        {
            if (_userLogic.IsEmailInUse(user))
            {
                TempData["alertMessageRegister"] = "Email is already in use, please choose another one.";
            }
            else
            {
                if (_userLogic.CreateAccount(user))
                {
                    TempData["alertMessageRegister"] = "Account has been created! You can log in now.";
                }
                else
                {
                    TempData["alertMessageRegister"] = "Could not create account, please try again later or ask for help.";
                }
            }

            return View("LogIn", user);
        }

        private async void InitUser(User user)
        { 
            var roles = _userLogic.GetUserRoles(user);

            var claims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

            ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
            var authProp = new AuthenticationProperties
            {
                IsPersistent = false
            };

            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProp);
        }

        private async void RemoveCookies()
        {
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}