using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Resume.Models
{
    public class Forgot
    {
        
        [Display(Name = "UserEmail")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Pwd { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confiem Password")]

        public string ConfirmPwd { get; set; }
        [DataType(DataType.Text)]
        public string OTP { get; set; }
    }

}
