using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core_Resume.Models;
using Microsoft.AspNetCore.Http;
using System.Dynamic;

namespace Core_Resume.Controllers
{
    public class Summry_CarrerObjectiveController : Controller
    {
        private readonly DataBaseContext _context;

        public Summry_CarrerObjectiveController(DataBaseContext context)
        {
            _context = context;
        }

        

        // GET: Summry_CarrerObjective/Details/5
        public IActionResult Details()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "Login");
            }
            string username = (string)HttpContext.Session.GetString("Username");

            var summry_CarrerObjective = _context.Summry_CarrerObjective.Where(m => m.Username == username).FirstOrDefault();
            if (summry_CarrerObjective == null)
            {
                return RedirectToAction("Create");
            }

            return View(summry_CarrerObjective);
        }

        // GET: Summry_CarrerObjective/Create
        public IActionResult Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        // POST: Summry_CarrerObjective/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Summry_CarrerObjective summry_CarrerObjective)
        {
            //if (ModelState.IsValid)
            {
                summry_CarrerObjective.Username = HttpContext.Session.GetString("Username");
                _context.Summry_CarrerObjective.Add(summry_CarrerObjective);
                _context.SaveChanges();
                return RedirectToAction("Details");
            }
            return View(summry_CarrerObjective);
        }

        // GET: Summry_CarrerObjective/Edit/5
        public async Task<IActionResult> Edit()
        {
            string username = (string)HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var summry_CarrerObjective = _context.Summry_CarrerObjective.Where(u => u.Username == username).FirstOrDefault();
            if (summry_CarrerObjective == null)
            {
                return RedirectToAction("Create");
            }
            return View(summry_CarrerObjective);
        }

        // POST: Summry_CarrerObjective/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string username, Summry_CarrerObjective summry_CarrerObjective)
        {
            if (username == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var query = _context.Summry_CarrerObjective.OrderBy(u => u.Username == username).FirstOrDefault().id;
            var query1 = _context.Summry_CarrerObjective.Where(u => u.id == query).FirstOrDefault();

            _context.Summry_CarrerObjective.Update(summry_CarrerObjective);
            _context.Summry_CarrerObjective.Remove(query1);
            _context.SaveChanges();
            return RedirectToAction("Details");
        }

        // GET: Summry_CarrerObjective/Delete/5
        public async Task<IActionResult> Delete()
        {
            string username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var summry_CarrerObjective = _context.Summry_CarrerObjective.Where(m => m.Username == username).FirstOrDefault();
            if (summry_CarrerObjective == null)
            {
                return RedirectToAction("Create");
            }

            return View(summry_CarrerObjective);
        }

        // POST: Summry_CarrerObjective/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var summry_CarrerObjective = await _context.Summry_CarrerObjective.FindAsync(id);
            _context.Summry_CarrerObjective.Remove(summry_CarrerObjective);
            await _context.SaveChangesAsync();
            return RedirectToAction("Create");
        }

        private bool Summry_CarrerObjectiveExists(int id)
        {
            return _context.Summry_CarrerObjective.Any(e => e.id == id);
        }








        public IActionResult Download()
        {
            string username = (string)HttpContext.Session.GetString("Username"); ;
            if (username == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                dynamic mymodel = new ExpandoObject();
                mymodel.Registration=_context.Register.Where(u => u.UserName == "Nandini12").FirstOrDefault();
                mymodel.Personal = _context.Persnol.Where(u => u.UserName == username).FirstOrDefault();
                mymodel.Education = _context.Educational.Where(u => u.Username == username).FirstOrDefault();
                mymodel.WorkHistory = _context.WorkHistory.Where(u => u.username == username).FirstOrDefault();
                mymodel.Summry = _context.Summry_CarrerObjective.Where(u => u.Username == username).FirstOrDefault();
                mymodel.Skills = _context.Skills.Where(u => u.Username == username);
                mymodel.Projects = _context.ProjectDetails.Where(u => u.Username == username);
                mymodel.Languages = _context.LanAndHobs.Where(u => u.Username == username);
                return View(mymodel);
            }
           
        }








        }
}














