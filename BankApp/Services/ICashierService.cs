using BankApp.ViewModels;
using BankApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Services
{
    public interface ICashierService
    {
        IEnumerable<CustomerListViewModel.CustomersViewModel> GetCustomers(string currentpage, string pagesize, string searchWord);
        CustomerDetailsViewModel GetCustomerDetails(int customerId);
        IEnumerable<TransactionsViewModel> NextTwentyFrom(int accountId, int position=0);
        AccountDetailsViewModel GetAccountDetails(int accountId);
        CustomerListViewModel FetchCustomers(CustomerListViewModel model, string currentpage, string pagesize, string searchWord);
        int GetCustomerId(int id);
        int GetAccountId(int id);
        void SetTransaction(AccountDetailsViewModel model);
        bool TryDeposit(decimal amount, decimal balance);
        bool TryWithdraw(decimal amount, decimal balance);
        AccountDetailsViewModel CreditModel(AccountDetailsViewModel model);
        AccountDetailsViewModel DebitModel(AccountDetailsViewModel model);
        AccountDetailsViewModel TransferCreditModel(AccountDetailsViewModel model);
        AccountDetailsViewModel TransferDebitModel(AccountDetailsViewModel model);
        TransactionsViewModel GetTransaction(AccountDetailsViewModel model);
    }
}
