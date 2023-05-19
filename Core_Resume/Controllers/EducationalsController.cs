using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core_Resume.Models;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Mail;
using Newtonsoft.Json;
using System.IO;


namespace Core_Resume.Controllers
{
    public class EducationalsController : Controller
    {
        private readonly DataBaseContext _context;

        public EducationalsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Educationals
        public async Task<IActionResult> Index()
        {
            return View(await _context.Educational.ToListAsync());
        }

        // GET: Educationals/Details/5
        public IActionResult Details()
        
            {

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "Login");
            }
            string username = (string)HttpContext.Session.GetString("Username");
            var educational = _context.Educational.Where(u => u.Username == username).FirstOrDefault();
            // var educational = _context.Educational.Where(m => m.Username==username).FirstOrDefault();
            if (educational == null)
            {
                return View("Create");
            }

            return View(educational);
        }

        // GET: Educationals/Create
        public IActionResult Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        // POST: Educationals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Educational educational)
        {
            //if (ModelState.IsValid)
            {
                educational.Username = HttpContext.Session.GetString("Username");
                _context.Educational.Add(educational);
                _context.SaveChanges();
                return RedirectToAction("Details");
            }
            return View(educational);
        }

        // GET: Educationals/Edit/5
        public async Task<IActionResult> Edit()
        {
            string username = (string)HttpContext.Session.GetString("Username");
            if (username== null)
            {
                return RedirectToAction("Login", "Login");
            }
            var educational = _context.Educational.Where(u => u.Username == username).FirstOrDefault();
            if (educational == null)
            {
                return RedirectToAction("Create");
            }
            return View(educational);
        }

        // POST: Educationals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string username, Educational educational)
        {
            if (username == null)
            {
                return View();
            }
            var query = _context.Educational.OrderBy(u => u.Username == username).FirstOrDefault().id;
            var query1 = _context.Educational.Where(u => u.id == query).FirstOrDefault();
            
            _context.Educational.Update(educational);
            _context.Educational.Remove(query1);
            _context.SaveChanges();
            return RedirectToAction("Details");
            

           
        }

        // GET: Educationals/Delete/5
        public async Task<IActionResult> Delete()
        {
            string username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var educational = _context.Educational.Where(m => m.Username == username).FirstOrDefault();
            if (educational == null)
            {
                return RedirectToAction("Create");
            }

            return View(educational);
        }

        // POST: Educationals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var educational = await _context.Educational.FindAsync(id);
            _context.Educational.Remove(educational);
            await _context.SaveChangesAsync();
            return RedirectToAction("Create");
        }

        private bool EducationalExists(int id)
        {
            return _context.Educational.Any(e => e.id == id);
        }
    }
}
