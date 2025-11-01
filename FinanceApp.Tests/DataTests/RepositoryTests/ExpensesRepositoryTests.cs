using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using FinanceApp.Controllers;
using FinanceApp.Data;
using FinanceApp.Data.Repository;
using FinanceApp.Data.Service;
using FinanceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FinanceApp.Tests.DataTests.RepositoryTests
{
    public class ExpensesRepositoryTests
    {
        private readonly FinanceAppContext _context;
        private readonly ExpensesRepository _repository;

        public ExpensesRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<FinanceAppContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new FinanceAppContext(options);
            _repository = new ExpensesRepository(_context);
        }

        [Fact]
        public async Task AddExpense_SavesToDatabase()
        {
            // Arrange
            var expense = new Expense { Category = "Food", Amount = 12.5, Date = DateTime.Now, Description = "Lunch" };

            // Act
            await _repository.AddExpense(expense);

            // Assert
            var saved = await _context.Expenses.FirstOrDefaultAsync(e => e.Description == "Lunch");
            Assert.NotNull(saved);
            Assert.Equal(12.5, saved.Amount);
        }
    }
}