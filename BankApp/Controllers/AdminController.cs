using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
        [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult EmptyView()
        {
            return View();
        }
        public IActionResult CreateUser()
        {
            return View();
        }
    }
}