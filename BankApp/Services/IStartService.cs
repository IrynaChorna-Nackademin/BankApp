using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Services
{
    public interface IStartService
    {
        int CountAllCustomers();
        int CountAllAccounts();
        decimal SumAllAcountsBalance();
        Dictionary<string, int> CustomersCountry();
        Dictionary<string, int> CountAccountsByYear();
        Dictionary<string, decimal> SumBalanceByYear();
    }
}
