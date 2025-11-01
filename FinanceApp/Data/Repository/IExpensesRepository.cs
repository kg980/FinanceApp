using FinanceApp.Models;

namespace FinanceApp.Data.Repository
{
    public interface IExpensesRepository
    {
        Task<IEnumerable<Expense>> GetAllExpenses();
        Task AddExpense(Expense expense);
        IQueryable GetChartData();
    }
}
