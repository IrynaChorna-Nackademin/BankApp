using BankApp.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace BankApp.UnitTests
{
    [Trait("Category", "UnitTest")]
    [Trait("Category", "Controller")]
    public class CashierController_WithdrawalCash: IClassFixture<CashierControllerFixture>
    {
        private readonly CashierControllerFixture _fixture;
     
        public CashierController_WithdrawalCash(CashierControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void WithdrawalCash_NotEnoughMoney_ReturnsError()
        {
            //Arrange
            AccountDetailsViewModel model = new AccountDetailsViewModel()
            { 
                AccountBalance = 10000,
                AccountId = 1,
                Transaction = new TransactionsViewModel() 
                { 
                    Amount = 12000
                }
            };

            _fixture.CashierService.TryWithdraw(model.Transaction.Amount, model.AccountBalance).Returns(false);
            //_fixture.CashierService.TryWithdraw(Arg.Any<decimal>(), Arg.Any<decimal>()).Returns(false);


            //Act
            var result = _fixture.CreateCashierController().WithdrawalCash(model) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ViewData);
            Assert.NotNull(result.ViewData.Values);
            Assert.Equal("The amount cannot be less then 0 or more then the balace", result.ViewData.Values.FirstOrDefault());
        }

        [Fact]
        public void WithdrawalCash_EnoughMoney_IsRedirected()
        {
            //Arrange
            AccountDetailsViewModel model = new AccountDetailsViewModel()
            {
                AccountBalance = 10000,
                AccountId = 1,
                Transaction = new TransactionsViewModel()
                {
                    Amount = 8000
                }
            };

            _fixture.CashierService.TryWithdraw(model.Transaction.Amount, model.AccountBalance).Returns(true);

            _fixture.CashierService.SetTransaction(model);

            //Act
            var result = _fixture.CreateCashierController().WithdrawalCash(model) as RedirectToActionResult;

            //Assert
            //Redirected
            Assert.NotNull(result);
            Assert.Equal("ShowTransaction", result.ActionName);
        }
    }
}
