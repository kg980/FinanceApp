using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using FinanceApp.Controllers;
using FinanceApp.Data.Service;
using FinanceApp.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;


/// Why using FakeItEasy to 'Mock' external dependencies?
/// Faking it because I want the dependency to always have the same value, 
/// so when I run tests, there isnt an extra variable affecting my results. 
/// (Is my test failing because of a change in my code, 
/// or because of a change in my repository?)

namespace FinanceApp.Tests.Controller
{
    public class ExpensesControllerTests
    {
        private readonly IExpensesService _expensesService;
        private readonly ExpensesController _expensesController;

        public ExpensesControllerTests()
        {
            // Inject dependencies for tests (by Mocking)
            _expensesService = A.Fake<IExpensesService>();

            // SUT: System Under Test
            _expensesController = new ExpensesController(_expensesService);
        }


        [Fact]
        public async Task ExpensesController_Index_ReturnsViewResult()
        {
            // Arrange
            // original code runs: 'var expenses = await _expensesService.GetAll();'
            // we fake the result of that call instead of really running it
            // we just need a return of an object of type <IEnumerable<Expense>> to meet the same criteria as the original, so fake an object of that return type
            var expenses = A.Fake<IEnumerable<Expense>>();
            // and then "run" the '.GetAll();' method to return the value into the expected 'expenses' variable so it matches the original code
            A.CallTo(() => _expensesService.GetAllExpenses()).Returns(Task.FromResult(expenses));
            // So we know that when the controller calls the service, it will always get back a variable called 'expenses' which will be of the correct type '<IEnumerable<Expense>>'
            // without actually calling the real service/repository
            // but we can still follow the steps in the debugger to see how the controller works, 'as if' the method was really called.

            // Act
            var result = await _expensesController.Index();

            // Assert
            //Assert.IsType<Microsoft.AspNetCore.Mvc.ViewResult>(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(expenses, viewResult.Model);
            Assert.NotNull(result);

            result.Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeEquivalentTo(expenses);
        }

        [Fact]
        public void GetChart_ReturnsJsonResult()
        {
            // Arrange
            var fakeData = A.Fake<IQueryable<Expense>>();
            A.CallTo(() => _expensesService.GetChartData()).Returns(fakeData);

            // Act
            var result = _expensesController.GetChart();

            // Assert
            result.Should().BeOfType<JsonResult>()
                .Which.Value.Should().BeEquivalentTo(fakeData);
        }

        [Fact]
        public async Task ExpensesController_Create_ReturnsRedirectToIndex()
        {
            // Arrange - Set up the variable needed by the method, call to the method
            var expense = new Expense { Id = 1, Description = "Test Expense", Amount = 100 };
            A.CallTo(() => _expensesService.AddExpense(expense)).DoesNothing();

            // Act - actually 'call' the method into a result which I can assert
            var result = await _expensesController.Create(expense);

            // Assert - Assert it !!!!
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);

            result.Should().BeOfType<RedirectToActionResult>()
                .Which.ActionName.Should().Be("Index");

            A.CallTo(() => _expensesService.AddExpense(expense))
                .MustHaveHappenedOnceExactly();
        }


        // TODO: This functionality has been moved to Repository layer, move this test to RepositoryTests.ExpensesRepositoryTests
        //[Fact]
        //public void ExpensesController_GetChart_ReturnsJsonResult()
        //{
        //    // Arrange
        //    var data = A.Fake<IQueryable<Expense>>();
        //    A.CallTo(() => _expensesService.GetChartData()).Returns(data);

        //    // Act
        //    var result = _expensesController.GetChart();

        //    // Assert
        //    Assert.IsType<Microsoft.AspNetCore.Mvc.JsonResult>(result);
        //    Assert.Equal(data, ((Microsoft.AspNetCore.Mvc.JsonResult)result).Value);

        //    result.Should().BeOfType<Microsoft.AspNetCore.Mvc.JsonResult>()
        //        .Which.Value.Should().BeEquivalentTo(data);
        //    result.Should().NotBeNull();
        //}
        // but must replace with service/repository in place of controller/service
        //[Fact]
        //public void ExpensesService_GetChartData_ReturnsExpectedQueryable()
        //{
        //    // Arrange
        //    var expectedData = A.Fake<IQueryable>();  // or IQueryable<Expense> if typed
        //    A.CallTo(() => _repository.GetChartData()).Returns(expectedData);

        //    var service = new ExpensesService(_repository);

        //    // Act
        //    var result = service.GetChartData();

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.Should().BeEquivalentTo(expectedData);

        //    // Verify that the repository method was actually called once
        //    A.CallTo(() => _repository.GetChartData()).MustHaveHappenedOnceExactly();
        //}

    }

}