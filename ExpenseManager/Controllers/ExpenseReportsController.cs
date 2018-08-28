using ExpenseManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ExpenseManager.Controllers
{
    public class ExpenseReportsController : Controller
    {
        ExpensesDataAccessLayer expDataAccess = new ExpensesDataAccessLayer();

        public ExpenseReportsController(ExpenseDBContext context)
        {
            expDataAccess.dbContext = context;
        }

        //TODO Decide if this mode or the one in article

        // GET: ExpenseReports
        public IActionResult Index()
        {
            return View(expDataAccess.GetAllExpenses().ToList());
        }

        // GET: ExpenseReports/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseReport = expDataAccess.GetAllExpenses()
                .FirstOrDefault(m => m.ItemId == id);
            if (expenseReport == null)
            {
                return NotFound();
            }

            return View(expenseReport);
        }

        public IActionResult AddEditExpenses(int itemId)
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
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ItemId,ItemName,Amount,Date,Description,Category")] ExpenseReport expenseReport)
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

        // GET: ExpenseReports/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseReport = expDataAccess.GetExpenseData(id);
            if (expenseReport == null)
            {
                return NotFound();
            }
            return View(expenseReport);
        }

        // POST: ExpenseReports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ItemId,ItemName,Amount,Date,Description,Category")] ExpenseReport expenseReport)
        {
            if (id != expenseReport.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    expDataAccess.UpdateExpense(expenseReport);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseReportExists(expenseReport.ItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(expenseReport);
        }

        // GET: ExpenseReports/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseReport = expDataAccess.GetExpenseData(id);
            if (expenseReport == null)
            {
                return NotFound();
            }

            return View(expenseReport);
        }

        // POST: ExpenseReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            expDataAccess.DeleteExpense(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ExpenseSummary()
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
