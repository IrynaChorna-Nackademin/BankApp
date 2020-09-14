using BankApp.Data;
using BankApp.Services;
using Xunit;

namespace BankApp.UnitTests.Service
{
    [Trait("Category", "UnitTest")]
    [Trait("Category", "Service")]
    public class CashierService_TryDeposit : IClassFixture<DatabaseFixture>
    {
        private readonly ApplicationDbContext _inMemoryDbContext;

        public CashierService_TryDeposit(DatabaseFixture databaseFixture)
        {
            _inMemoryDbContext = databaseFixture.ApplicationDbContext;
        }

        [Fact]
        public void TryDeposit_AmountNegative_ReturnsFalse()
        {
            //Arrange
            var amount = -1000;
            var balance = 500;
            var service = new CashierService(_inMemoryDbContext);

            //Act
            var result = service.TryDeposit(amount, balance);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void TryDeposit_AmountPositive_ReturnsTrue()
        {
            //Arrange
            var amount = 1000;
            var balance = 500;
            var service = new CashierService(_inMemoryDbContext);

            //Act
            var result = service.TryDeposit(amount, balance);

            //Assert
            Assert.True(result);
        }
    }

}
