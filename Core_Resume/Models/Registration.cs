using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;



namespace Core_Resume.Models
{
    public class Registration
    {
        [Key]
        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please Enter Feild")]
        [Display(Name = "UserName")]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9_]{7,29}$")]
        [Remote("UserNameExists", "Registration", HttpMethod = "post", ErrorMessage = "Username is already taken.")]
        public string UserName { get; set; }

        [Display(Name = "UserEmail")]
        [Remote("UserEmaiExists", "Registration", HttpMethod = "post", ErrorMessage = "Useremail is already taken.")]
        [DataType(DataType.EmailAddress)]

        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Please Enter Feild")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "PasswordShould Contain at least one lower case,one upper case,one number,special character and minimum 8 charecter")]
        public string Pwd { get; set; }

        [Required(ErrorMessage = "Please Enter Feild")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("Pwd", ErrorMessage = "Password And Confirm Password Must be same")]
        public string ConfirmPwd { get; set; }
        public DateTime Expire { get; set; }
    }
}
