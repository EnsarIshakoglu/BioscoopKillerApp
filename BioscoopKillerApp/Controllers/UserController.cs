using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Claims;
using System.Threading.Tasks;
using Logic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Models;
using Models.Enums;

namespace BioscoopKillerApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserLogic _userLogic = new UserLogic();
        private readonly ReviewLogic _reviewLogic = new ReviewLogic();
        private readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn([Bind("Password, Email")] User model)
        {
            if (model.Password == null || model.Email == null)
            {
                TempData["alertMessage"] = "Please fill in all the fields!";
                return View("LogIn");
            }

            var user = _userLogic.GetUserByEmail(model.Email);
            if (user == null)
            {
                TempData["alertMessage"] = "Incorrect username or password, please try again!";
                return View("LogIn");
            }

            if (_hasher.VerifyHashedPassword(user, user.Password, model.Password) == PasswordVerificationResult.Failed)
            {
                TempData["alertMessage"] = "Incorrect username or password, please try again!";
                return View("LogIn");
            }

            InitUser(user);
            return RedirectToAction("Index", "Movie");
        }
        [Authorize]
        public IActionResult LogOut()
        {
            RemoveCookies();

            return RedirectToAction("Index", "Movie");
        }

        [HttpPost]
        public IActionResult CreateAccount([Bind("Password, Name, SurName, Email")] User user)
        {
            user.Password = _hasher.HashPassword(user, user.Password);

            if (!ModelState.IsValid)
            {
                TempData["alertMessageRegister"] = "Please fill in all the fields!";
                return View("LogIn");
            }
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
        [Authorize]
        [HttpPost]
        public IActionResult PostReview([FromBody]Review review)
        {
            if (!ModelState.IsValid) return new JsonResult(new { validationMessage = $"Error creating review, please note that review subject (min 6 and max 100 chars long) and review text (min 6 and max 2000 chars long) are required to post a review." });

            _reviewLogic.SaveReview(review);

            return new JsonResult(new { message = $"Saved review!" });
        }

        private async void InitUser(User user)
        {
            user = _userLogic.InitUser(user);

            var claims = user.Roles.Select(role => new Claim(ClaimTypes.Role, role.ToString())).ToList();

            claims.Add(new Claim(ClaimTypes.Sid, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Name));

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
            var authProp = new AuthenticationProperties();

            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProp);
        }

        private async void RemoveCookies()
        {
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}