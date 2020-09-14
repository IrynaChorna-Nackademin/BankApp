using BankApp.Data;
using BankApp.Services;
using BankApp.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using Xunit;

namespace BankApp.UnitTests.Service
{
    [Trait("Category", "UnitTest")]
    [Trait("Category", "Service")]
    public class CashierService_SetTransaction : IClassFixture<DatabaseFixture>
    {
        private readonly ApplicationDbContext _inMemoryDbContext;

        public CashierService_SetTransaction(DatabaseFixture databaseFixture)
        {
            _inMemoryDbContext = databaseFixture.ApplicationDbContext;
        }

        [Fact]
        public void SetTransaction_SaveTransactionToDatabase_ShouldBeOkSaved()
        {
            //Arrange
            int accountId = 1;

            var model = new AccountDetailsViewModel()
            {
                Transaction = new TransactionsViewModel() 
                { 
                    AccountId = accountId,
                    Amount = 555,
                    Balance = 1000,
                    Date = DateTime.ParseExact("2020-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Operation = "Withdraw with cash",
                    Symbol = string.Empty,
                    Type = "Credit"
                }
            };

            var service = new CashierService(_inMemoryDbContext);

            //Act
            service.SetTransaction(model);

            var result = service.NextTwentyFrom(accountId, 0);

            //Assert            
            Assert.NotNull(result);
            Assert.Single(result);
            //Assert.Equal(1, result.Count());
            Assert.Equal(555, result.ToList().FirstOrDefault().Amount);
            Assert.Equal(1000, result.ToList().FirstOrDefault().Balance);
        }
    }

}
