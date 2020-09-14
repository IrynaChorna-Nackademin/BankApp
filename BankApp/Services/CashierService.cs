using BankApp.Data;
using BankApp.ViewModels;
using BankApplication.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Services
{
    public class CashierService : ICashierService
    {
        private readonly ApplicationDbContext _context;
        public CashierService(ApplicationDbContext context)
        {
            _context = context;
        }
        public AccountDetailsViewModel GetAccountDetails(int accountId)
        {
            var accountDetails = new AccountDetailsViewModel();

            accountDetails.AccountId = accountId;
            accountDetails.AccountBalance = _context.Transactions
                .Where(a => a.AccountId == accountId)
                .OrderByDescending(d => d.TransactionId)
                .FirstOrDefault().Balance;
            
            return accountDetails;
        }
       
        public TransactionsViewModel GetTransaction(AccountDetailsViewModel model)
        {
            int id = model.AccountId;
            if (model.AccountIdFrom>0)
            {
                id = model.AccountIdFrom;
            }
            var transaction = _context.Transactions
                 .Where(a=>a.AccountId == id)
                 .OrderByDescending(d => d.TransactionId)
                 .Select(t=> new TransactionsViewModel 
                 {
                    Amount = t.Amount,
                    Balance = t.Balance,
                    Operation = t.Operation,
                    TransactionId = t.TransactionId,
                    Date = t.Date,
                    Type =t.Type
                 })
                 .FirstOrDefault();
            return transaction;
        }
        public CustomerDetailsViewModel GetCustomerDetails(int customerId)
        {
            var customerDetails = new CustomerDetailsViewModel();
            customerDetails = _context.Customers
                            .Select(a => new CustomerDetailsViewModel
                            {
                                Balance = a.Dispositions.Sum(a => a.Account.Balance),
                                CustomerId = a.CustomerId,
                                Birthday = a.Birthday,
                                City = a.City,
                                Country = a.Country,
                                CountryCode = a.CountryCode,
                                Emailaddress = a.Emailaddress,
                                Gender = a.Gender,
                                Givenname = a.Givenname,
                                NationalId = a.NationalId,
                                Streetaddress = a.Streetaddress,
                                Surname = a.Surname,
                                Telephonecountrycode = a.Telephonecountrycode,
                                Telephonenumber = a.Telephonenumber,
                                Zipcode = a.Zipcode
                            }).FirstOrDefault(r => r.CustomerId == customerId);

            customerDetails.CustomersAccounts = _context.Dispositions
                .Select(a => new CastomersAccount 
                { 
                    AccountId = a.AccountId, 
                    Type = a.Type, 
                    CustomerId = a.CustomerId 
                })
                .Where(r => r.CustomerId == customerId).ToList();
            return customerDetails;
        }

        public IEnumerable<CustomerListViewModel.CustomersViewModel> GetCustomers(string currentpage, string pagesize, string searchWord)
        {
            var customers = _context.Customers
                    .Select(c => new CustomerListViewModel.CustomersViewModel
                    {
                        CustomerId = c.CustomerId,
                        NationalId = c.NationalId,
                        Givenname = c.Givenname,
                        Surname = c.Surname,
                        Streetaddress = c.Streetaddress,
                        City = c.City
                    })
                    .OrderBy(c=>c.CustomerId)
                    .ToList();

            if (!string.IsNullOrEmpty(searchWord))
            {
                customers = customers.Where(i => i.Surname.ToUpper().Contains(searchWord.ToUpper()) 
                    || i.Givenname.ToUpper().Contains(searchWord.ToUpper())
                    || i.NationalId.ToUpper().Contains(searchWord.ToUpper())
                    || i.Streetaddress.ToUpper().Contains(searchWord.ToUpper())
                    || i.City.ToUpper().Contains(searchWord.ToUpper()))
                    .ToList();
            }

            return customers;
        }
        public CustomerListViewModel FetchCustomers(CustomerListViewModel model, string currentpage, string pagesize, string searchWord)
        {
            
            var customers = GetCustomers(currentpage, pagesize, searchWord);
            int pageSize = string.IsNullOrEmpty(pagesize) ? 50 : Convert.ToInt32(pagesize);
            int currentPage = string.IsNullOrEmpty(currentpage) ? 1 : Convert.ToInt32(currentpage);
            int pageCount = (customers.Count() + pageSize - 1) / pageSize;
            model.Customers = customers.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            model.PagingViewModel.MaxPages = pageCount;
            model.PagingViewModel.PageCount = pageCount;
            model.PagingViewModel.PageSize = pageSize;
            model.PagingViewModel.CurrentPage = currentPage;
            model.SearchWord = searchWord;
            return model;
        }

        public int GetCustomerId(int id)
        {
            if (!_context.Customers.Any(c => c.CustomerId == id))
            {
                throw new NotFoundCustomerException(id.ToString());
            }

            int customersId = _context.Customers.SingleOrDefault(c => c.CustomerId == id).CustomerId;
            return customersId;                           
        }
        public int GetAccountId(int id)
        {
            if (!_context.Accounts.Any(c => c.AccountId == id))
            {
                throw new NotFoundCustomerException(id.ToString());
            }

            int accountId = _context.Accounts.SingleOrDefault(c => c.AccountId == id).AccountId;
            return accountId;
        }
        public void SetTransaction(AccountDetailsViewModel model)
        {
            var tr = new Transactions
            {
                
                AccountId = model.Transaction.AccountId,
                Amount = model.Transaction.Amount,
                Balance = model.Transaction.Balance,
                Date = model.Transaction.Date,
                Operation = model.Transaction.Operation,
                Symbol = model.Transaction.Symbol,
                Type = model.Transaction.Type
            };
            _context.Transactions.Add(tr);
            _context.SaveChanges();

        }
        public IEnumerable<TransactionsViewModel> NextTwentyFrom(int accountId, int position=0)
        {
            var accountDetails = new List<TransactionsViewModel>();
            
            accountDetails = _context.Transactions
                    .Where(a => a.AccountId == accountId)
                    .Select(t => new TransactionsViewModel
                    {
                        Date = t.Date,
                        TransactionId = t.TransactionId,
                        AccountId = t.AccountId,
                        Account = t.Account,
                        Amount = t.Amount,
                        Balance = t.Balance,
                        Bank = t.Bank,
                        Operation = t.Operation,
                        Symbol = t.Symbol,
                        Type = t.Type
                    })
                    .OrderByDescending(d => d.Date)
                    .Skip(position)
                    .Take(20)
                    .ToList();
            return accountDetails;
        }
      

        public bool TryDeposit(decimal amount, decimal balance)
        {
            if (amount <= 0) 
            {
                return false;
            }
            
            //balance += amount;
            return true;
        }

        public bool TryWithdraw(decimal amount, decimal balance)
        {
            if (amount <= 0 || balance - amount < 0) 
            {
                return false;
            }
                
            //balance -= amount;

            return true;
        }

        public AccountDetailsViewModel CreditModel(AccountDetailsViewModel model)
        {
            model.Transaction.AccountId = model.AccountId;
            model.Transaction.Amount = model.Transaction.Amount;
            model.Transaction.Date = DateTime.Now;
            model.Transaction.Type = "Credit";
            model.Transaction.Operation = "Credit in Cash";
            model.Transaction.Symbol = " ";
            model.Transaction.Balance = model.AccountBalance + model.Transaction.Amount;
            
            return model;
        }
        public AccountDetailsViewModel DebitModel(AccountDetailsViewModel model)
        {
            model.Transaction.AccountId = model.AccountId;
            model.Transaction.Amount = 0-model.Transaction.Amount;
            model.Transaction.Date = DateTime.Now;
            model.Transaction.Type = "Debit";
            model.Transaction.Operation = "Withdrawal in Cash";
            model.Transaction.Symbol = " ";
            model.Transaction.Balance = model.AccountBalance + model.Transaction.Amount;

            return model;
        }

        public AccountDetailsViewModel TransferCreditModel(AccountDetailsViewModel model)
        {
            model.Transaction.AccountId = model.AccountId;
            model.Transaction.Amount = model.Transaction.Amount;
            model.Transaction.Date = DateTime.Now;
            model.Transaction.Type = "Credit";
            model.Transaction.Operation = "Transfer from account";
            model.Transaction.Symbol = " ";
            model.Transaction.Balance = model.AccountBalance + model.Transaction.Amount;

            return model;
        }
        public AccountDetailsViewModel TransferDebitModel(AccountDetailsViewModel model)
        {
            model.Transaction.AccountId = model.AccountIdFrom;
            model.Transaction.Amount = 0 - model.Transaction.Amount;
            model.Transaction.Date = DateTime.Now;
            model.Transaction.Type = "Debit";
            model.Transaction.Operation = "Transfer to account";
            model.Transaction.Symbol = " ";
            model.Transaction.Balance = model.AccountBalanceFrom + model.Transaction.Amount;

            return model;
        }
    }
}
