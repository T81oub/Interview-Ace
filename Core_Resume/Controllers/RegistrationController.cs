using Core_Resume.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Core_Resume.Controllers;
namespace Core_Resume.Controllers
{
    public class RegistrationController : Controller
    {
        public DataBaseContext _db;
        public RegistrationController(DataBaseContext db)
        {
            _db = db;
        }
        [HttpPost]
        public JsonResult UserNameExists(string username)
        {
            var user = _db.Register.Where(x => x.UserName == username);
            if (user.Count() == 0)
            {
                return Json(true);

            }
            else
            {
                return Json(string.Format("{0} is not available.", username));
            }
        }
        public JsonResult UserEmaiExists(string useremail)
        {

            var user = _db.TempEmail.Where(x => x.UserEmail == useremail);
            var user1 = _db.Register.Where(x => x.UserEmail == useremail);
            if (user.Count() == 0 && user1.Count() == 0)
            {
                return Json(true);

            }
            else
            {
                return Json(string.Format("{0} is not available.", useremail));
            }

        }

        public IActionResult Verify()
        {
            DateTime today = DateTime.Now;
            var date1 = _db.TempEmail.Where(u => u.Created < today);
            foreach (var r in date1)
            {
                _db.TempEmail.Remove(r);

            }
            _db.SaveChanges();

            var date12 = _db.Register.Where(u => u.Expire < today && u.UserEmail == "Xyz@x.com");
            foreach (var r in date12)
            {
                _db.Register.Remove(r);

            }
            _db.SaveChanges();
            string Token = HttpContext.Request.Query["Uid"];
            ViewBag.essage = Token;
            var up = _db.TempEmail.Where(u => u.Token == Token).SingleOrDefault();
            //string email = up.UserEmail;
            if (up == null)
            {
                var word = Token.Split('.');
                var Username = "";
                if (word.Length > 1)
                    Username = word[word.Length - 3];
                var user = _db.Register.Where(u => u.UserName == Username);
                if (user.Count() == 0)
                {
                    ViewBag.essage = Username;
                    return View();
                }

            }
            else
            {
                var words = Token.Split('.');
                var username = "";
                username = words[words.Length - 3];

                //ViewBag.essage = username;
                var query = _db.Register.Where(u => u.UserName == username).SingleOrDefault();
                var email = words[words.Length - 2] + words[words.Length - 1];
                query.UserEmail = email;
                ViewBag.essage = query.UserEmail;
                _db.Register.Update(query);
                _db.SaveChanges();
                _db.TempEmail.Remove(up);
                _db.SaveChanges();
            }



            return View();


        }
        public IActionResult Register()
        {
            DateTime today = DateTime.Now;
            var date1 = _db.TempEmail.Where(u => u.Created < today);
            _db.TempEmail.RemoveRange(date1);
            _db.SaveChanges();

            var date12 = _db.Register.Where(u => u.Expire < today && u.UserEmail == "Xyz@x.com");
            _db.Register.RemoveRange(date12);
            _db.SaveChanges();

            return View();
        }

        [HttpPost]
        public IActionResult Register(Registration User)
        {
            HttpContext.Session.SetString("Username", User.UserName);
            string Token = Guid.NewGuid().ToString();
            string connection = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=InterviewsDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            System.TimeSpan duration = new System.TimeSpan(0, 0, 10, 0);
            System.DateTime newDate1 = DateTime.Now.Add(duration);
            using (SqlConnection con = new SqlConnection(connection))
            {
                string abc = "Xyz@x.com";
                string insert = "INSERT INTO Register(UserName, UserEmail, Pwd, ConfirmPwd, Expire) VALUES(@UserName, @UserEmail, @Pwd, @ConfirmPwd, @Expire)";
                using (SqlCommand com = new SqlCommand(insert, con))
                {
                    com.Parameters.AddWithValue("@UserName", User.UserName);
                    com.Parameters.AddWithValue("@UserEmail", abc);
                    com.Parameters.AddWithValue("@Pwd", User.Pwd);
                    com.Parameters.AddWithValue("@ConfirmPwd", User.ConfirmPwd);
                    com.Parameters.AddWithValue("@Expire", newDate1);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(connection))
            {
                string insert = "INSERT INTO TempEmail(UserEmail, UserName, Token, Created) VALUES(@UserEmail, @UserName, @Token, @Created)";
                using (SqlCommand com = new SqlCommand(insert, con))
                {
                    com.Parameters.AddWithValue("@UserEmail", User.UserEmail);
                    com.Parameters.AddWithValue("@UserName", User.UserName);
                    com.Parameters.AddWithValue("@Token", Token);
                    com.Parameters.AddWithValue("@Created", newDate1);
                    con.Open();
                    com.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Verification email sent to " + User.UserEmail);

            return RedirectToAction("Create", "Educationals");

        }

        public async Task<IActionResult> Details()
        {

            string username = (string)HttpContext.Session.GetString("Username");
            var persnola = _db.Register.Where(m => m.UserName == username).FirstOrDefault();

            if (persnola == null)
            {
                return View("Register");
            }

            return View(persnola);
        }
    }
}