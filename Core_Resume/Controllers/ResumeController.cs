using Core_Resume.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace Core_Resume.Controllers
{
	public class ResumeController : Controller
	{
		private readonly DataBaseContext _context;

		public ResumeController(DataBaseContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			string username = (string)HttpContext.Session.GetString("Username"); ;
			if (username == null)
			{
				return RedirectToAction("Login", "Login");
			}
			else
			{
				dynamic mymodel = new ExpandoObject();
				mymodel.Registration = _context.Register.Where(u => u.UserName == "Nandini12").FirstOrDefault();
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
