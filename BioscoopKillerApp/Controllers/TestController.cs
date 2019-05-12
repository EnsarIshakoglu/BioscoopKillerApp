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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Models;
using Models.Enums;

namespace BioscoopKillerApp.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Privacy", "Home");

        }
    }
}