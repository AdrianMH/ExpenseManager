using ExpenseManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ExpenseManager.Controllers
{
    public class ExpenseReportsController : Controller
    {
        ExpensesDataAccessLayer expDataAccess;

        public ExpenseReportsController(ExpenseDBContext context)
        {
            expDataAccess = new ExpensesDataAccessLayer(context);
        }
        // GET: ExpenseReports
        public IActionResult Index(string searchString)
        {
            var reports = expDataAccess.GetAllExpenses().ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                reports = expDataAccess.GetSearchResult(searchString).ToList();
            }
            return View(reports);
        }

        public ActionResult AddEditExpenses(int itemId)
        {
            var model = new ExpenseReport();
            if (itemId > 0)
            {
                model = expDataAccess.GetExpenseData(itemId);
            }
            return PartialView("_expenseForm", model);
        }

        // POST: ExpenseReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(ExpenseReport expenseReport)
        {
            if (ModelState.IsValid)
            {
                if (expenseReport.ItemId > 0)
                {
                    expDataAccess.UpdateExpense(expenseReport);
                }
                else
                {
                    expDataAccess.AddExpense(expenseReport);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            expDataAccess.DeleteExpense(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: ExpenseReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            expDataAccess.DeleteExpense(id);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult ExpenseSummary()
        {
            return PartialView("_expenseReport");
        }

        public JsonResult GetMonthlyExpense()
        {
            return new JsonResult(expDataAccess.CalculateMonthlyExpense());
        }

        public JsonResult GetWeeklyExpense()
        {
            return new JsonResult(expDataAccess.CalculateWeeklyExpense());
        }

        private bool ExpenseReportExists(int id)
        {
            return expDataAccess.GetAllExpenses().Any(e => e.ItemId == id);
        }
    }
}
