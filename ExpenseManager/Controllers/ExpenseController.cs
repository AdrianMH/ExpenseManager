﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExpenseManager.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExpenseManager.Controllers
{
    public class ExpenseController : Controller
    {
        ExpensesDataAccessLayer expDataAccess = new ExpensesDataAccessLayer();
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}