using BankApp.Controllers;
using BankApp.Services;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System;

namespace BankApp.UnitTests
{
    public class CashierControllerFixture: IDisposable
    {
        internal readonly ICashierService CashierService;
        public CashierController Controller;

        public CashierControllerFixture()
        {
            CashierService = Substitute.For<ICashierService>();
        }

        public void Dispose()
        {
            Controller = null;
        }

        public CashierController CreateCashierController() 
        {
            Controller = new CashierController(CashierService) 
                {
                    ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext 
                    { 
                        HttpContext = new DefaultHttpContext() 
                    }               
                };

            return Controller;
        }
    }
}