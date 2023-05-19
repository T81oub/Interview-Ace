using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Resume.Models
{
    public class Persnola
    {
        [Key]
        public int ID { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Your First Name ")]

        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Your Last Name ")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please Your Date Of Birth ")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please Your Nationality ")]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "Select Your Educational Level ")]
        public string EducationalLevel { get; set; }

        [Required(ErrorMessage = "Please Your Address ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Your Phone Number ")]
        [RegularExpression(@"^[789]\d{9}$")]
        [Display(Name="Telephone")]
        public string Tel { get; set; }

      

        [Required(ErrorMessage = "Select gender")]
        public string Gender { get; set; }
        [Required]
        public string marital_status {get;set;}
        public byte[] photo { get; set; }

       
    } 
}
