using Core_Resume.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core_Resume.Controllers
{
    public class LoginController : Controller
    {
        public DataBaseContext _db;
        public string username = null;
        public LoginController(DataBaseContext db)
        {
            _db = db;
        }
        public IActionResult Login()
        {
           
            return View();
        }
        [HttpPost]
        public IActionResult Login(Login user)
        {
            
            if (user.UserName != null && user.Password != null)
            {
                var up = _db.Register.Where(u => u.UserName == user.UserName);
                var ue = up.Where(u => u.Pwd == user.Password).Count();
                if (ue == 1)
                {
                    //ViewBag.Message = "success";
                    //Save user Into session

                    HttpContext.Session.SetString("Username", user.UserName);
                    ViewBag.USername = HttpContext.Session.GetString("Username");
                    Response.Cookies.Append("LastLogin", DateTime.Now.ToString());
                    return RedirectToAction("Index", "Dashboard");
                }
                ViewBag.Message = "User name /Password Incorrect";
                return View();
            }
            ViewBag.Message = "Please Enter Feild";
            return View();
                
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }



        public IActionResult Forgot()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Forgot(Forgot user)
        {
            var User = _db.Register.Where(x => x.UserEmail == user.Email);
            if (User.Count() == 1)
            {
             
                string tomail = user.Email;
                string mailbody = "Hi,to Reset Your Password Click this link https://localhost:44385/Login/UpdatePassword?Uid=" + user.Email;
                MailMessage Resmail = new MailMessage("fineartgallery06@gmail.com", tomail);
                Resmail.Body = mailbody;
                Resmail.IsBodyHtml = true;
                Resmail.Subject = "Verification mail";
                Resmail.Priority = MailPriority.High;
                SmtpClient SMTP = new SmtpClient("smtp.gmail.com", 587);
                SMTP.DeliveryMethod = SmtpDeliveryMethod.Network;
                SMTP.UseDefaultCredentials = false;
                SMTP.Credentials = new NetworkCredential()
                {
                    UserName = "fineartgallery06@gmail.com",
                    Password = "Artgallery#1221"
                };
                SMTP.EnableSsl = true;
                SMTP.Send(Resmail);
                ViewBag.Mes = "We Sent You Mail to reset Your Password";
                return View();




            }
            ViewBag.Mes = "UserEmail is wrong/not register";
            return View();
        }
        public IActionResult UpdatePassword()
        {
           string UserEmail = HttpContext.Request.Query["Uid"];
            HttpContext.Session.SetString("UserEmail", UserEmail);
          //  var up = _db.Register.Where(u => u.UserEmail == UserEmail).FirstOrDefault();
          //  ViewBag.Message = up.UserEmail;
            return View();
        }
      
            [HttpPost]
         public IActionResult UpdatePassword(Forgot User)
            {
            var useremail = HttpContext.Session.GetString("UserEmail");

            if (User.Pwd !=null && User.ConfirmPwd!=null)
            {
                Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
                Match match = regex.Match(User.Pwd);
                if (match.Success)
                {
                    if(User.Pwd==User.ConfirmPwd)
                    {
                        var query = _db.Register.Where(u => u.UserEmail == useremail).FirstOrDefault();
                        query.Pwd = User.Pwd;
                        query.ConfirmPwd = User.ConfirmPwd;
                        _db.Update(query);
                        _db.SaveChanges();
                        //  ViewBag.Message = useremail;     

                        //  ViewBag.Message = up.Pwd;

                        return RedirectToAction("Login");
                    }
                    ViewBag.Message = "ConfirmPassword And Password does not same";
                    return View();
                }
                ViewBag.Message = "PasswordShould Contain at least one lower case,one upper case,one number,special character and minimum 8 charecter";
                return View();
            }

            ViewBag.Message = "Please Enter field";
            return View();
        }
        }
}
