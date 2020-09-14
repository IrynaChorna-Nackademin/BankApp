using BankApp.Data;
using BankApp.Services;
using BankApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Xunit;

namespace BankApp.UnitTests.Service
{
    [Trait("Category", "UnitTest")]
    [Trait("Category", "Service")]
    public class CashierService_TryWithdraw : IClassFixture<DatabaseFixture>
    {
        private readonly ApplicationDbContext _inMemoryDbContext;

        public CashierService_TryWithdraw(DatabaseFixture databaseFixture)
        {
            _inMemoryDbContext = databaseFixture.ApplicationDbContext;
        }

        [Fact]
        public void TryWithdraw_AmountNegative_ReturnsFalse() 
        {            
            //Arrange            
            var amount = -1000;
            var balance = 500;
            var service = new CashierService(_inMemoryDbContext);

            //Act
            var result = service.TryWithdraw(amount, balance);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void TryWithdraw_AmountGreaterThanBalance_ReturnsFalse()
        {
            //Arrange
            var amount = 1000;
            var balance = 500;
            var service = new CashierService(_inMemoryDbContext);

            //Act
            var result = service.TryWithdraw(amount, balance);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void TryWithdraw_AmountPositiveAndLowerThanBalance_ReturnsTrue()
        {
            //Arrange
            var amount = 500;
            var balance = 1000;
            var service = new CashierService(_inMemoryDbContext);

            //Act
            var result = service.TryWithdraw(amount, balance);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void TryWithdraw_RetrieveBalanceFromTempDbInOrderToSimulateFlowWhichIsBiggerThanAmount_ReturnsTrue()
        {
            //Arrange
            var accountId = 1;

            //Simulate db
            var transactions = new List<BankApplication.Models.Transactions>() 
            { 
                new BankApplication.Models.Transactions 
                { 
                    AccountId = accountId,
                    Balance = 100,
                    TransactionId = 1                    
                },
                new BankApplication.Models.Transactions
                {
                    AccountId = accountId,
                    Balance = 500,
                    TransactionId = 2
                },
                new BankApplication.Models.Transactions
                {
                    AccountId = accountId,
                    Balance = 1000,
                    TransactionId = 3
                }
            };            

            _inMemoryDbContext.Transactions.AddRange(transactions);
            _inMemoryDbContext.SaveChanges();
                      
            var service = new CashierService(_inMemoryDbContext);
            
            var transaction = new TransactionsViewModel() { Amount = 500 };           

            //Act
            var model = service.GetAccountDetails(accountId);
            model.Transaction = transaction;

            var result = service.TryWithdraw(model.Transaction.Amount, model.AccountBalance);

            //Assert
            Assert.True(result);
        }        
    }

}
