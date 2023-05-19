using Core_Resume.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Resume.Controllers
{
    public class ProjectDetailsController : Controller
    {

        private readonly Models.DataBaseContext _context;

        public ProjectDetailsController(DataBaseContext context)
        {
            _context = context;
        }


        // GET: ProjectDetailsController/Create
        public ActionResult Create()
        {
            string username = HttpContext.Session.GetString("Username");         
            IEnumerable<ProjectDetails> l = _context.ProjectDetails.Where(l => l.Username == username);
            ViewBag.List = l;
            return View();
        }

        // POST: ProjectDetailsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectDetails proj)
        {
            try
            {

                string username = HttpContext.Session.GetString("Username");
                proj.Username = username;
                _context.ProjectDetails.Add(proj);
                _context.SaveChanges();
                IEnumerable<ProjectDetails> l = _context.ProjectDetails.Where(l => l.Username == username);
                ViewBag.List = l;
                return View();
            }
            catch
            {
                return View();
            }
        }

       
    }
}
