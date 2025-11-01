using FinanceApp.Data;
using FinanceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Data.Repository
{
    public class ExpensesRepository : IExpensesRepository
    {

        private readonly FinanceAppContext _context;

        public ExpensesRepository(FinanceAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expense>> GetAllExpenses()
            => await _context.Expenses.ToListAsync();

        public async Task AddExpense(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }

        public IQueryable GetChartData()
        {
            var data = _context.Expenses
                .GroupBy(e => e.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(e => e.Amount)
                });
            return data;
        }
    }
}
