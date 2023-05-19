using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Resume.Models
{
    public class WorkHistory
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string JobTitle { get; set; }
        public string Employer { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate{get;set;}
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [Display(Name = "I currently working here")]
        public bool status { get; set; }

    }
}
