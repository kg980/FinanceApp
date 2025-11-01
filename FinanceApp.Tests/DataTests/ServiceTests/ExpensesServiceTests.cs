using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using FinanceApp.Controllers;
using FinanceApp.Data.Repository;
using FinanceApp.Data.Service;
using FinanceApp.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Tests.DataTests.ServiceTests
{
    public class ExpensesServiceTests
    {
        private readonly IExpensesRepository _repository;
        private readonly ExpensesService _expensesService;

        public ExpensesServiceTests()
        {
            // Inject dependencies for tests (by Mocking)
            _repository = A.Fake<IExpensesRepository>();

            // SUT: System Under Test
            _expensesService = new ExpensesService(_repository);
        }



        // TODO: This functionality has been moved to Repository layer, move this test to RepositoryTests.ExpensesRepositoryTests
        [Fact]
        public void ExpensesController_GetChart_ReturnsJsonResult()
        {
            // Arrange
            var data = A.Fake<IQueryable<Expense>>();
            A.CallTo(() => _expensesService.GetChartData()).Returns(data);

            // Act
            var result = _expensesService.GetChartData();

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.JsonResult>(result);
            Assert.Equal(data, ((Microsoft.AspNetCore.Mvc.JsonResult)result).Value);

            result.Should().BeOfType<Microsoft.AspNetCore.Mvc.JsonResult>()
                .Which.Value.Should().BeEquivalentTo(data);
            result.Should().NotBeNull();
        }
        // but must replace with service/repository in place of controller/service
        [Fact]
        public void ExpensesService_GetChartData_ReturnsExpectedQueryable()
        {
            // Arrange
            var expectedData = A.Fake<IQueryable>();  // or IQueryable<Expense> if typed
            A.CallTo(() => _repository.GetChartData()).Returns(expectedData);

            var service = new ExpensesService(_repository);

            // Act
            var result = service.GetChartData();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedData);

            // Verify that the repository method was actually called once
            A.CallTo(() => _repository.GetChartData()).MustHaveHappenedOnceExactly();
        }

    }
}
