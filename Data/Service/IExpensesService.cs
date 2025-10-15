using FinanceApp.Models;

namespace FinanceApp.Data.Service
{
    public interface IExpensesService
    {
        Task<IEnumerable<Expense>> GetAll();
        //Task<Expense> GetById(int id);
        Task Add(Expense expense);
    }
}
