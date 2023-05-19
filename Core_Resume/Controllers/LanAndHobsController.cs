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
    public class LanAndHobsController : Controller
    {
        private readonly DataBaseContext _context;

        public LanAndHobsController(DataBaseContext context)
        {
            _context = context;
        }

        public ActionResult Create()
        {
            string username = HttpContext.Session.GetString("Username");
        
            IEnumerable<LanAndHob> l= _context.LanAndHobs.Where(l => l.Username == username);
            ViewBag.LanAndHobList = l;
            return View();
        }
        [HttpPost]
        public ActionResult Create(LanAndHob Lang)
        {
            string username = HttpContext.Session.GetString("Username");
 
            Lang.Username = username;
            _context.LanAndHobs.Add(Lang);
            _context.SaveChanges();

            IEnumerable<LanAndHob> l = _context.LanAndHobs.Where(l => l.Username == username );
            ViewBag.LanAndHobList = l;
            return View();
        }

    }

}