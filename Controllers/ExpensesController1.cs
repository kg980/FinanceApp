using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Controllers
{
    public class ExpensesController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
