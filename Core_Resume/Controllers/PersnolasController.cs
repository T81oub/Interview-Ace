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
    public class PersnolasController : Controller
    {
        private readonly DataBaseContext _context;

        public PersnolasController(DataBaseContext context)
        {
            _context = context;
        }


        // GET: Persnolas/Details/5
        public IActionResult Details()
        {
           
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "Login");
            }
            
            string username = (string)HttpContext.Session.GetString("Username");
            var persnola =  _context.Persnol.Where(m => m.UserName==username).FirstOrDefault();

            if (persnola == null)
            {
                return View("Create");
            }
          
            HttpContext.Session.SetString("Tel", persnola.Tel);
            return View(persnola);
        }

        // GET: Persnolas/Create
        public IActionResult Create()
        {
           
            string username = (string)HttpContext.Session.GetString("Username");
            if (username != null)
            {
                ViewBag.Message= username;
            }
            ViewBag.Message= "Fail";
            return View();
        }

        // POST: Persnolas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public async Task<IActionResult> Create(Persnola persnola,List<IFormFile> photo)
        {

           
            string username = (string)HttpContext.Session.GetString("Username");
            persnola.UserName = username;
           
           
            photo p = new photo();
            string fileName = null;
            //if (ModelState.IsValid)
            {
                foreach(var i in photo)
                {
                    if(i.Length>0)
                    {
                         fileName = System.IO.Path.GetFileName(i.FileName);
                        using (var stream=new MemoryStream())
                        {
                            await i.CopyToAsync(stream);
                            p.Image = stream.ToArray();
                            
                        }
                    }
                }

                if (fileName == null || fileName.EndsWith(".PNG") || fileName.EndsWith(".JPG") || fileName.EndsWith(".png") || fileName.EndsWith(".jpg") || fileName.EndsWith(".JPEG") || fileName.EndsWith(".jpeg"))
                {
                    //do stuff here

                    p.Username = HttpContext.Session.GetString("Username");
                    _context.Images.Add(p);
                    _context.SaveChanges();
                    int otpValue = new Random().Next(100000, 999999);
                    HttpContext.Session.SetString("FirstName", persnola.FirstName);
                    HttpContext.Session.SetString("Lastname", persnola.LastName);
                    HttpContext.Session.SetString("Nationality", persnola.Nationality);
                    HttpContext.Session.SetString("EducationaLevel", persnola.EducationalLevel);
                    HttpContext.Session.SetString("Address", persnola.Address);
                    HttpContext.Session.SetString("Tel", persnola.Tel);
                    HttpContext.Session.SetString("Gender", persnola.Gender);
                    HttpContext.Session.SetString("marital_status", persnola.marital_status);
                    HttpContext.Session.SetString("Date", persnola.DateOfBirth.ToString());
                    Persnola details = new Persnola();
                    details.UserName = HttpContext.Session.GetString("Username");
                    details.FirstName = HttpContext.Session.GetString("FirstName");
                    details.LastName = HttpContext.Session.GetString("Lastname");
                    details.Nationality = HttpContext.Session.GetString("Nationality");
                    details.EducationalLevel = HttpContext.Session.GetString("EducationaLevel");
                    details.Address = HttpContext.Session.GetString("Address");
                    details.Tel = HttpContext.Session.GetString("Tel");
                    details.Gender = HttpContext.Session.GetString("Gender");
                    details.marital_status = HttpContext.Session.GetString("marital_status");
                    details.DateOfBirth = Convert.ToDateTime(HttpContext.Session.GetString("Date"));

                    var photoh = _context.Images.Where(p => p.Username == details.UserName).FirstOrDefault();
                    details.photo = photoh.Image;

                    _context.Images.Remove(photoh);
                    if (HttpContext.Session.GetString("Phone") != null)
                    {
                        string phone = (HttpContext.Session.GetString("Phone"));
                        var query2t = _context.Persnol.Where(u => u.Tel == phone).FirstOrDefault();
                        _context.Persnol.Remove(query2t);
                    }
                    _context.Persnol.Add(details);
                    _context.SaveChanges();
                    ViewBag.Message = "Successfully register";
                    return RedirectToAction("Details");
                }
                ViewBag.Message = "Image formate etither .png or .jpg";
            }

            return View();
        }

        // GET: Persnolas/Edit/5
        public async Task<IActionResult> Edit()
        {
            string username = (string)HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Register","Registration");
            }

            var persnola = _context.Persnol.Where(u => u.UserName == username).FirstOrDefault();
            if (persnola == null)
            {
                return RedirectToAction("Create");
            }
            ViewBag.Message = persnola.UserName;
            return View(persnola);
        }

        // POST: Persnolas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string username,Persnola persnola,List<IFormFile> photo)
        {


            if( username== null)
            {
                return View();
            }
            var query = _context.Persnol.Where(u => u.UserName == username).FirstOrDefault();
            var id = query.ID;
            photo p = new photo();
            string fileName = null;
            foreach (var i in photo)
            {
                if (i.Length > 0)
                {
                    fileName = System.IO.Path.GetFileName(i.FileName);
                    using (var stream = new MemoryStream())
                    {
                        await i.CopyToAsync(stream);
                       p.Image= stream.ToArray();

                    }
                }
            }
            if (p.Image == null)
            {
                
                var user = _context.Persnol.Where(u => u.UserName == username).FirstOrDefault();
                p.Image = user.photo;
            }
            if (fileName == null || fileName.EndsWith(".PNG") || fileName.EndsWith(".JPG") || fileName.EndsWith(".png") || fileName.EndsWith(".jpg") || fileName.EndsWith(".JPEG") || fileName.EndsWith(".jpeg"))
            {
                
                persnola.photo = p.Image;
              
                if(HttpContext.Session.GetString("Tel")!=persnola.Tel)
                {
                    p.Username = HttpContext.Session.GetString("Username");
                    _context.Images.Add(p);
                    _context.SaveChanges();
                    HttpContext.Session.SetString("Phone", HttpContext.Session.GetString("Tel"));
                    int otpValue = new Random().Next(100000, 999999);
                    HttpContext.Session.SetString("FirstName", persnola.FirstName);
                    HttpContext.Session.SetString("Lastname", persnola.LastName);
                    HttpContext.Session.SetString("Nationality", persnola.Nationality);
                    HttpContext.Session.SetString("EducationaLevel", persnola.EducationalLevel);
                    HttpContext.Session.SetString("Address", persnola.Address);
                    HttpContext.Session.SetString("Tel", persnola.Tel);
                    HttpContext.Session.SetString("Gender", persnola.Gender);
                    HttpContext.Session.SetString("marital_status", persnola.marital_status);
                    HttpContext.Session.SetString("Date", persnola.DateOfBirth.ToString());
                    
                    HttpContext.Session.SetString("ID", id.ToString());
                    Persnola details = new Persnola();
                    details.UserName = HttpContext.Session.GetString("Username");
                    details.FirstName = HttpContext.Session.GetString("FirstName");
                    details.LastName = HttpContext.Session.GetString("Lastname");
                    details.Nationality = HttpContext.Session.GetString("Nationality");
                    details.EducationalLevel = HttpContext.Session.GetString("EducationaLevel");
                    details.Address = HttpContext.Session.GetString("Address");
                    details.Tel = HttpContext.Session.GetString("Tel");
                    details.Gender = HttpContext.Session.GetString("Gender");
                    details.marital_status = HttpContext.Session.GetString("marital_status");
                    details.DateOfBirth = Convert.ToDateTime(HttpContext.Session.GetString("Date"));

                    var photoh = _context.Images.Where(p => p.Username == details.UserName).FirstOrDefault();
                    details.photo = photoh.Image;

                    _context.Images.Remove(photoh);
                    if (HttpContext.Session.GetString("Phone") != null)
                    {
                        string phone = (HttpContext.Session.GetString("Phone"));
                        var query2t = _context.Persnol.Where(u => u.Tel == phone).FirstOrDefault();
                        _context.Persnol.Remove(query2t);
                    }
                    _context.Persnol.Add(details);
                    _context.SaveChanges();
                    ViewBag.Message = "Successfully register";
                    return RedirectToAction("Details");

                }
                _context.Persnol.Update(persnola);
                var query2 = _context.Persnol.Where(u => u.ID == id).FirstOrDefault();
                _context.Persnol.Remove(query2);
                _context.SaveChanges();
                return RedirectToAction("Details");
            }
            ViewBag.Message = "Image formate etither .png or .jpg";
            return View();
          
        }

       
        public IActionResult Delete()
        {
            string username = (string)HttpContext.Session.GetString("Username");
            var persnola = _context.Persnol.Where(m => m.UserName == username).FirstOrDefault();

            if (persnola == null)
            {
                return View("Create");
            }

            
            return View(persnola);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string username,List<IFormFile> Photo)
        {
            var persnola =_context.Persnol.Where(u=>u.UserName==username).FirstOrDefault();

            _context.Persnol.Remove(persnola);
           _context.SaveChanges();
            return RedirectToAction("Details","Registration");
        }

        private bool PersnolaExists(int id)
        {
            return _context.Persnol.Any(e => e.ID == id);
        }

        public IActionResult checkOTP()
        {
            string OTP = HttpContext.Session.GetString("OTP");
            ViewBag.Message = OTP;
            return View();
        }
        [HttpPost]
       
        public IActionResult checkOTP(checkNumber user)
        {
            string OTP =( HttpContext.Session.GetString("OTP"));
           // user.OTP = OTP;
            if (user.OTP != null)
            {
                if(String.Equals(user.OTP, OTP))
                { 
                    
                Persnola details = new Persnola();
                details.UserName = HttpContext.Session.GetString("Username");
                details.FirstName = HttpContext.Session.GetString("FirstName");
                details.LastName = HttpContext.Session.GetString("Lastname");
                details.Nationality = HttpContext.Session.GetString("Nationality");
                details.EducationalLevel = HttpContext.Session.GetString("EducationaLevel");
                details.Address = HttpContext.Session.GetString("Address");
                details.Tel = HttpContext.Session.GetString("Tel");
                details.Gender = HttpContext.Session.GetString("Gender");
                details.marital_status = HttpContext.Session.GetString("marital_status");
                details.DateOfBirth = Convert.ToDateTime(HttpContext.Session.GetString("Date"));
                    
                 var photo = _context.Images.Where(p => p.Username == details.UserName).FirstOrDefault();
                details.photo = photo.Image;

                 _context.Images.Remove(photo);
                    if (HttpContext.Session.GetString("Phone") != null)
                    {
                        string phone = (HttpContext.Session.GetString("Phone"));
                        var query2 = _context.Persnol.Where(u => u.Tel == phone).FirstOrDefault();
                        _context.Persnol.Remove(query2);
                    }
                    _context.Persnol.Add(details);
                    _context.SaveChanges();
                    ViewBag.Message = "Successfully register";
                    return RedirectToAction("Details");
            }
                ViewBag.Message = "OTP does not same";
                return View();
            }
           
            ViewBag.Message = "Please enter OTP";
            return View();
        }

        public IActionResult Remove()
        {
            string str = HttpContext.Session.GetString("Username");
            var query = _context.Persnol.Where(u => u.UserName == str).FirstOrDefault();

            return View(query);
        }
        [HttpPost]
        public IActionResult Remove(Persnola user)
        {
            string str = HttpContext.Session.GetString("Username");
            var query = _context.Persnol.Where(u => u.UserName == str).FirstOrDefault();
            query.photo = null;
            _context.Persnol.Update(query);
            _context.SaveChanges();
            return RedirectToAction("Details");
        }
    }

}


   
