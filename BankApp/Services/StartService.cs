using BankApp.Data;
using BankApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Services
{
    public class StartService : IStartService
    {
        private readonly ApplicationDbContext _context;

        public StartService(ApplicationDbContext context)
        {
            _context = context;
        }
        public int CountAllCustomers() => _context.Customers.Count();
        public Dictionary<string, int> CustomersCountry() 
        {
            var result = new Dictionary<string, int>();
            var listGrouped = _context.Customers
                .GroupBy(c => c.Country)
                .Select(n => Tuple.Create(n.Key, n.Count()));

            foreach (var item in listGrouped) 
            {
                result.Add(item.Item1, item.Item2);
            }
            return result;
        }
        public int CountAllAccounts() => _context.Accounts.Count();
        public Dictionary<string, int> CountAccountsByYear()
        {
            var result = new Dictionary<string, int>();

            var listGrouped = _context.Accounts
                .GroupBy(c => c.Created.Year)
                .OrderBy(o=>o.Key)
                .Select(n => Tuple.Create(n.Key, n.Count()));
            foreach (var item in listGrouped)
            {
                result.Add(item.Item1.ToString(), item.Item2);
            }
            return result;
        }
        public decimal SumAllAcountsBalance() => _context.Accounts.Sum(a => a.Balance);
        public Dictionary<string, decimal> SumBalanceByYear()
        {
            var result = new Dictionary<string, decimal>();

            var listGrouped = _context.Accounts
                .GroupBy(c => c.Created.Year)
                .OrderBy(o => o.Key)
                .Select(n => Tuple.Create(n.Key, n.Sum(s=>s.Balance)));

            foreach (var item in listGrouped)
            {
                result.Add(item.Item1.ToString(), item.Item2);
            }
            return result;
        }
    }
}
