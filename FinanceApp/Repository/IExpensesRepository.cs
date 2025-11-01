using FinanceApp.Models;

namespace FinanceApp.Repository
{
    public interface IExpensesRepository
    {
        Task<IEnumerable<Expense>> GetAllExpenses();
        Task AddExpense(Expense expense);
        IQueryable GetChartData();
    }
}
