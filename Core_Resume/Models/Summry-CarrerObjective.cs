using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Resume.Models
{
    public class Summry_CarrerObjective
    {
        [Key]
        public int id { get; set; }
        public string Username { get; set; }
        public string CarrerObjective { get; set; }
        public string Summry { get; set; }
    }
}
