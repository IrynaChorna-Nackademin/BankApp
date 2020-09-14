using BankApp.Services;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using System.Transactions;
using BankApp.Data;
namespace BankApp.Controllers
{
    [Authorize(Roles = "Cashier")]
    public class CashierController : Controller
    {
        private readonly ICashierService _service;

        public CashierController(ICashierService service)
        {
            _service = service;
        }

        public IActionResult ViewCustomer(string searchId)
        {
            var model = new CustomerDetailsViewModel();

            if (!string.IsNullOrEmpty(searchId))
            {
                try
                {
                    if (int.TryParse(searchId, out int id))
                    {
                        model = _service.GetCustomerDetails(_service.GetCustomerId(id));
                        return View(model);
                    }
                }
                catch (NotFoundCustomerException e)
                {
                    ViewBag.Exception = "The customer (ID: " + e.Message + ") cannot be found";
                }
            }
            return View(model);
        }


        public IActionResult ViewAccount(int accountId)
        {
            var viewModel = new AccountDetailsViewModel();
            viewModel = _service.GetAccountDetails(accountId);
            viewModel.Transactions = _service.NextTwentyFrom(accountId, 0).ToList();
            return View(viewModel);
        }
        public IActionResult GetFrom(int accountId, int position)
        {
            return Json(_service.NextTwentyFrom(accountId, position).ToList());
        }

        public IActionResult SearchCustomers(string currentpage, string pagesize, string searchWord, CustomerListViewModel model)
        {
            model = _service.FetchCustomers(model, currentpage, pagesize, searchWord);
            if (model.Customers.Count() == 0)
            {
                return ViewBag.Message("Empty");
            }
            return View(model);
        }

        public IActionResult Transaction(string searchAccount)
        {
            var model = new AccountDetailsViewModel();

            if (!string.IsNullOrEmpty(searchAccount))
            {
                try
                {
                    if (int.TryParse(searchAccount, out int id))
                    {
                        model.AccountId = _service.GetAccountDetails(_service.GetAccountId(id)).AccountId;
                        model.AccountBalance = _service.GetAccountDetails(_service.GetAccountId(id)).AccountBalance;
                        model.Transaction = _service.GetAccountDetails(_service.GetAccountId(id)).Transaction;
                        return View(model);
                    }
                }
                catch (NotFoundCustomerException e)
                {
                    ViewBag.Exception = "The account (ID: " + e.Message + ") cannot be found";
                }
            }
            return View(model);
        }

        public IActionResult CreditCash(int id)
        {
            var model = new AccountDetailsViewModel();

            model.AccountId = _service.GetAccountDetails(_service.GetAccountId(id)).AccountId;
            model.AccountBalance = _service.GetAccountDetails(_service.GetAccountId(id)).AccountBalance;
            return View(model);
        }


        public IActionResult WithdrawalCash(int id)
        {
            var model = new AccountDetailsViewModel();

            model.AccountId = _service.GetAccountDetails(_service.GetAccountId(id)).AccountId;
            model.AccountBalance = _service.GetAccountDetails(_service.GetAccountId(id)).AccountBalance;
            return View(model);
        }

        public IActionResult Transfer(int id, string searchAccount)
        {
            var model = new AccountDetailsViewModel();
            model.AccountIdFrom = _service.GetAccountDetails(_service.GetAccountId(id)).AccountId;
            model.AccountBalanceFrom = _service.GetAccountDetails(_service.GetAccountId(id)).AccountBalance;

            if (!string.IsNullOrEmpty(searchAccount))
            {
                try
                {
                    if (int.TryParse(searchAccount, out int idTo))
                    {
                        model.AccountId = _service.GetAccountDetails(_service.GetAccountId(idTo)).AccountId;
                        model.AccountBalance = _service.GetAccountDetails(_service.GetAccountId(idTo)).AccountBalance;
                        return View(model);
                    }
                }
                catch (NotFoundCustomerException e)
                {
                    ViewBag.Exception = "The account (ID: " + e.Message + ") cannot be found";
                }
            }
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Transfer(AccountDetailsViewModel model)
        {
            if (_service.TryDeposit(model.Transaction.Amount, model.AccountBalance))
            {
                if (_service.TryWithdraw(model.Transaction.Amount, model.AccountBalanceFrom))
                {
                    if (ModelState.IsValid)
                    {
                        _service.SetTransaction(_service.TransferCreditModel(model));
                        _service.SetTransaction(_service.TransferDebitModel(model));

                        return RedirectToAction("ShowTransaction", model);
                    }
                }
                ViewBag.Exception = "The amount cannot be less then 0";
                return View(model);
            }
            ViewBag.Exception = "The amount cannot be less then 0";
            return View(model);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreditCash(AccountDetailsViewModel model)
        {
            if (_service.TryDeposit(model.Transaction.Amount, model.AccountBalance))
            {
                if (ModelState.IsValid)
                {
                    _service.SetTransaction(_service.CreditModel(model));

                    return RedirectToAction("ShowTransaction", model);
                }
                return View(model);
            }
            ViewBag.Exception = "The amount cannot be less then 0";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult WithdrawalCash(AccountDetailsViewModel model)
        {
            if (_service.TryWithdraw(model.Transaction.Amount, model.AccountBalance))
            {
                if (ModelState.IsValid)
                {

                    _service.SetTransaction(_service.DebitModel(model));
                    
                    return RedirectToAction("ShowTransaction", model);
                }

                return View(model);
            }
            ViewBag.Exception = "The amount cannot be less then 0 or more then the balace";

            return View(model);
        }

        public IActionResult ShowTransaction(AccountDetailsViewModel model)
        {
            var transaction = _service.GetTransaction(model);
            return View(transaction);
        }

    }
}