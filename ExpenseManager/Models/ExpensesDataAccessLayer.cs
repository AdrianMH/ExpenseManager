using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseManager.Models
{
    public class ExpensesDataAccessLayer
    {
        ExpenseDBContext dbContext;

        public ExpensesDataAccessLayer(ExpenseDBContext context)
        {
            dbContext = context;
        }

        public IEnumerable<ExpenseReport> GetAllExpenses()
        {
            try
            {
                return dbContext.ExpenseReports.ToList();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<ExpenseReport> GetSearchResult(string searchString)
        {
            try
            {
                List<ExpenseReport> exp = GetAllExpenses().ToList();
                return exp.Where(e => e.ItemName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1);
            }
            catch
            {
                throw;
            }
        }

        public void AddExpense(ExpenseReport expense)
        {
            try
            {
                dbContext.ExpenseReports.Add(expense);
                dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public int UpdateExpense(ExpenseReport expense)
        {
            try
            {
                dbContext.Entry(expense).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                dbContext.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
                throw;
            }
        }

        public ExpenseReport GetExpenseData(int? id)
        {
            try
            {
                return dbContext.ExpenseReports.Find(id);
            }
            catch
            {
                throw;
            }
        }

        public void DeleteExpense(int id)
        {
            try
            {
                dbContext.ExpenseReports.Remove(dbContext.ExpenseReports.Find(id));
                dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public Dictionary<string, decimal> CalculateMonthlyExpense()
        {
            var dictMonthlySum = new Dictionary<string, decimal>();

            decimal foodSum = dbContext.ExpenseReports.Where(f => f.Category == "Food" && (f.Date > DateTime.Now.AddMonths(-6)))
                .Select(f => f.Amount)
                .Sum();

            decimal shoppingSum = dbContext.ExpenseReports.Where(s => s.Category == "Shopping" && (s.Date > DateTime.Now.AddMonths(-6)))
                .Select(s => s.Amount)
                .Sum();

            decimal travelingSum = dbContext.ExpenseReports.Where(t => t.Category == "Traveling" && (t.Date > DateTime.Now.AddMonths(-6)))
                .Select(t => t.Amount)
                .Sum();

            dictMonthlySum.Add("Food", foodSum);
            dictMonthlySum.Add("Shopping", shoppingSum);
            dictMonthlySum.Add("Traveling", travelingSum);

            return dictMonthlySum;
        }

        public Dictionary<string, decimal> CalculateWeeklyExpense()
        {
            var dictWeeklyExpense = new Dictionary<string, decimal>();

            decimal foodSum = dbContext.ExpenseReports.Where(f => f.Category == "Food" && (f.Date > DateTime.Now.AddDays(-7)))
                .Select(f => f.Amount)
                .Sum();

            decimal shoppingSum = dbContext.ExpenseReports.Where(s => s.Category == "Shopping" && (s.Date > DateTime.Now.AddDays(-7)))
                .Select(s => s.Amount)
                .Sum();

            decimal travelingSum = dbContext.ExpenseReports.Where(t => t.Category == "Traveling" && (t.Date > DateTime.Now.AddDays(-7)))
                .Select(t => t.Amount)
                .Sum();

            dictWeeklyExpense.Add("Food", foodSum);
            dictWeeklyExpense.Add("Shopping", shoppingSum);
            dictWeeklyExpense.Add("Traveling", travelingSum);

            return dictWeeklyExpense;
        }
    }
}
