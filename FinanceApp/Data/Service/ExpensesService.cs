using FinanceApp.Models;
using FinanceApp.Repository;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Data.Service
{
    public class ExpensesService : IExpensesService
    {
        // Moved context to the Repository layer for better separation of concerns
        //private readonly FinanceAppContext _context;
        //public ExpensesService(FinanceAppContext context) 
        //{
        //    _context = context;
        //} 

        private readonly IExpensesRepository _repository;

        public ExpensesService(IExpensesRepository repository)
        {
            _repository = repository;
        }

        public async Task AddExpense(Expense expense)
        {
            // Business logic before save
            if (expense.Amount <= 0)
                throw new ArgumentException("Expense amount must be positive.");

            await _repository.AddExpense(expense);

            // Maybe log or trigger events
            //_logger.LogInformation("Expense added successfully.");
        }

        public async Task<IEnumerable<Expense>> GetAllExpenses()
        {
            return await _repository.GetAllExpenses();
        }

        public IQueryable GetChartData()
        {
            return _repository.GetChartData();
        }


    }
}
