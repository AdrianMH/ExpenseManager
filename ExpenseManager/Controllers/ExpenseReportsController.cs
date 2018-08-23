using ExpenseManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManager.Controllers
{
    public class ExpenseReportsController : Controller
    {
        private readonly ExpenseDBContext _context;
        ExpensesDataAccessLayer expDataAccess = new ExpensesDataAccessLayer();

        public ExpenseReportsController(ExpenseDBContext context)
        {
            _context = context;
            expDataAccess.dbContext = context;
        }

        //TODO Decide if this mode or the one in article

        // GET: ExpenseReports
        public ActionResult<IActionResult> Index()
        {
            return View(expDataAccess.GetAllExpenses().ToList());
        }

        // GET: ExpenseReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseReport = await _context.ExpenseReports
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (expenseReport == null)
            {
                return NotFound();
            }

            return View(expenseReport);
        }

        // GET: ExpenseReports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExpenseReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,ItemName,Amount,Date,Description,Category")] ExpenseReport expenseReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expenseReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expenseReport);
        }

        // GET: ExpenseReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseReport = await _context.ExpenseReports.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,ItemName,Amount,Date,Description,Category")] ExpenseReport expenseReport)
        {
            if (id != expenseReport.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expenseReport);
                    await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseReport = await _context.ExpenseReports
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (expenseReport == null)
            {
                return NotFound();
            }

            return View(expenseReport);
        }

        // POST: ExpenseReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expenseReport = await _context.ExpenseReports.FindAsync(id);
            _context.ExpenseReports.Remove(expenseReport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseReportExists(int id)
        {
            return _context.ExpenseReports.Any(e => e.ItemId == id);
        }
    }
}
