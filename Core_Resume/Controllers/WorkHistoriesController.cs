using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core_Resume.Models;
using Microsoft.AspNetCore.Http;

namespace Core_Resume.Controllers
{
    public class WorkHistoriesController : Controller
    {
        private readonly DataBaseContext _context;

        public WorkHistoriesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: WorkHistories
        

        // GET: WorkHistories/Details/5
        public IActionResult Details()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "Login");
            }
            string username = (string)HttpContext.Session.GetString("Username");
            var workHistory = _context.WorkHistory.Where(m => m.username == username).FirstOrDefault();
            if (workHistory == null)
            {
                return RedirectToAction("Create");
            }


            return View(workHistory);
        }

        // GET: WorkHistories/Create
        public IActionResult Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        // POST: WorkHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkHistory workHistory)
        {
            if (ModelState.IsValid)
            {
                workHistory.username= HttpContext.Session.GetString("Username");
                _context.WorkHistory.Add(workHistory);
                _context.SaveChanges();
                return RedirectToAction("Details");
            }
            return View(workHistory);
        }

        // GET: WorkHistories/Edit/5
        public async Task<IActionResult> Edit()
        {
            string username = (string)HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var workHistory =  _context.WorkHistory.Where(u=>u.username==username).FirstOrDefault();
            if (workHistory == null)
            {
                return RedirectToAction("Create");
            }
            return View(workHistory);
        }

        // POST: WorkHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string username,WorkHistory workHistory)
        {
            if (username == null)
            {
                return View();
            }


            var query = _context.WorkHistory.OrderBy(u => u.username == username).FirstOrDefault().id;
            var query1 = _context.WorkHistory.Where(u => u.id == query).FirstOrDefault();

            _context.WorkHistory.Update(workHistory);
            _context.WorkHistory.Remove(query1);
            _context.SaveChanges();
            return RedirectToAction("Details");
        }

        // GET: WorkHistories/Delete/5
        public async Task<IActionResult> Delete()
        {
            string username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var workHistory = _context.WorkHistory.Where(m => m.username == username).FirstOrDefault();
            if (workHistory == null)
            {
                return RedirectToAction("Create");
            }

            return View(workHistory);
        }

        // POST: WorkHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var workHistory = await _context.WorkHistory.FindAsync(id);
            _context.WorkHistory.Remove(workHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Create");
        }

        private bool WorkHistoryExists(int id)
        {
            return _context.WorkHistory.Any(e => e.id == id);
        }
    }
}
