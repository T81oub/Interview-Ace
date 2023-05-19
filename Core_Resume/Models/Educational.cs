using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Resume.Models
{
    public class Educational
    {
        [Key]
        public int id { get; set; }

        public string Username { get; set; }
        [Required]
        [Display(Name = "State / Province")]
        public string State { get; set; }
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }
        [Required]
        [Display(Name = "School / College")]
        public string School_Or_College  { get; set; }
        [Required]
        [Display(Name = "Board/University")]
        public string Board_or_Uni { get; set; }
        [Required]
        [Display(Name = "Class / Standard / Degree / Qualification ")]
        public string Standard_or_Degree{ get; set; }
        [Required]
        [Display(Name = " Field of study / Course / Branch / Stream")]
        public string Field { get; set; }
        [Required]
        [Display(Name = "Completed / Passing Year ")]
        [Range(1, int.MaxValue)]
        public int year { get; set; }
        [Required]
        [Display(Name = "Percentage / CGPA (Optional)")]
        [Range(0, 10)]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid")]
        public float cgpa { get; set; }
         [Display(Name = "Currently I'm Studying Here.")]
        public bool status { get; set; }


    }
}
