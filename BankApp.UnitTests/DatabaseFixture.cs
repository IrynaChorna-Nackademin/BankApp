using BankApp.Data;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.UnitTests
{
    public class DatabaseFixture : IDisposable
    {
        internal readonly DbContextOptions<ApplicationDbContext> Options;

        public DatabaseFixture()
        {
            var databaseName = Guid.NewGuid().ToString();

            Options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName).Options;
        }

        public ApplicationDbContext ApplicationDbContext => NewContext();

        private ApplicationDbContext NewContext() 
        {
            return new ApplicationDbContext(Options);
        }

        public void Dispose()
        {                        
        }
    }
}
