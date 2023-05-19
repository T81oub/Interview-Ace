using Core_Resume.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Resume.Controllers
{
    public class SkillsController : Controller
    {
        // GET: SkillsController
        private readonly Models.DataBaseContext _context;

        public SkillsController(DataBaseContext context)
        {
            _context = context;
        }


        // GET: SkillsController/Details/5

        // GET: SkillsController/Create
        public ActionResult Create()
        {
            string username = HttpContext.Session.GetString("Username");
            IEnumerable<Skills> l = _context.Skills.Where(l => l.Username == username);
             ViewBag.List = l;
            return View();
        }

        // POST: SkillsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Skills skill)
        {
            try
            {
                string username = HttpContext.Session.GetString("Username");
                skill.Username = username;
                _context.Skills.Add(skill);
                _context.SaveChanges();

                IEnumerable<Skills> l = _context.Skills.Where(l => l.Username == username);
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
