using FinanceApp.Models;

namespace FinanceApp.Data.Service
{
    public interface IExpensesService
    {
        Task<IEnumerable<Expense>> GetAll();
        //Task<Expense> GetById(int id);
        Task Add(Expense expense);

        IQueryable GetChartData(); // Using IQueryable so Querying happens in db, not in memory -> better performance
    }
}
