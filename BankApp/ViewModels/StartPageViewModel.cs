using BankApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.ViewModels
{
    public class StartPageViewModel: CustomerDetailsViewModel
    {
        public int CustomersCount { get; set; }
        public int AccountsCount { get; set; }
        public decimal BalanceCountSum { get; set; }
        public Dictionary<string, int> CustomersCountry { get; set; }
        public Dictionary<string, int> AccountsCountByYear { get; set; }
        public Dictionary<string, decimal> BalanceByYear { get; set; }

    }
}
