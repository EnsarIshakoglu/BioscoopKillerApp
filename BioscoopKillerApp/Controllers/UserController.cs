﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Claims;
using System.Threading.Tasks;
using Logic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Models;
using Models.Enums;

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
            if (!ModelState.ContainsKey("Password") && !ModelState.ContainsKey("Username"))
            {
                TempData["alertMessage"] = "Please fill in all the fields!";
                return View("LogIn");
            }
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
            //RemoveCookies();
            RemoveSessionData();


            return RedirectToAction("Index", "Movie");
        }

        [HttpPost]
        public IActionResult CreateAccount([Bind("Password, Name, SurName, Email")] User user)
        {
            if (!ModelState.IsValid)
            {
                TempData["alertMessage"] = "Please fill in all the fields!";
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

        private void InitUser(User user)
        {
            var userId = _userLogic.GetUserId(user);
            var roles = _userLogic.GetUserRoles(user);
            var roleIds = new List<Roles>();


            foreach (var role in roles)
            {
                Enum.TryParse(role, out Roles userRole);

                roleIds.Add(userRole);
            }

            
            this.HttpContext.Session.Set("roles", ConvertListToByteArray(roleIds));
            this.HttpContext.Session.SetInt32("userId", userId);
        }

        private byte[] ConvertListToByteArray<T>(List<T> listToConvert)
        {
            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();
            binFormatter.Serialize(mStream, listToConvert);

            return mStream.ToArray();
        }

        private void RemoveSessionData()
        {
            var keys = this.HttpContext.Session.Keys;

            for (int x = 0; x < keys.Count(); x++)
            {
                this.HttpContext.Session.Remove(keys.ElementAt(x));
            }
        }
    }
}