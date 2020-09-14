using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BankApp.Models;
using BankApp.Services;
using BankApp.ViewModels;
using Newtonsoft.Json;

namespace BankApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStartService _service;

        public HomeController(IStartService service, ILogger<HomeController> logger)
        {
            _service = service;
            _logger = logger;
        }
        
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
        public IActionResult Index()
        {
            var model = new StartPageViewModel();
            model.AccountsCount = _service.CountAllAccounts();
            model.CustomersCount = _service.CountAllCustomers();
            model.BalanceCountSum = _service.SumAllAcountsBalance();
            model.CustomersCountry = _service.CustomersCountry();
            model.AccountsCountByYear = _service.CountAccountsByYear();
            model.BalanceByYear = _service.SumBalanceByYear();
            return View(model);
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
